using AutoFixture;
using AwesomeAssertions;
using NodaTime;
using PatientPortal.Domain.Common;
using PatientPortal.Domain.Consultations;
using PatientPortal.Domain.Fixtures;
using PatientPortal.Domain.Fixtures.Consultations.Scenarios;
using PatientPortal.Domain.Patients;
using PatientPortal.Domain.Values;
using Xunit;

namespace PatientPortal.Domain.UnitTests.Consultations;

public class ConsultationTests
{
    private readonly IFixture _fixture = DomainFixture.Create();

    [Fact]
    public void ScheduleConsultation_WhenValid_Succeeds_AndSetsState()
    {
        var now = _fixture.Create<IClock>().GetCurrentInstant();
        var input = new ScheduleConsultation(
            Now: now,
            Id: _fixture.Create<ConsultationId>(),
            PatientId: _fixture.Create<PatientId>(),
            Schedule: _fixture.Create<ConsultationSchedule.Scheduled>(),
            Attachments: _fixture.Create<ConsultationAttachments>(),
            Notes: _fixture.Create<bool>() ? _fixture.Create<string>() : null);

        var result = Consultation.TrySchedule(input);

        result.IsSuccess.Should().BeTrue();
        var c = result.Value;
        c.Id.Should().Be(input.Id);
        c.PatientId.Should().Be(input.PatientId);
        c.Schedule.Should().Be(input.Schedule);
        c.Attachments.Should().BeEquivalentTo(input.Attachments);
        c.Notes.Should().Be(input.Notes);
        c.IsCancelled.Should().BeFalse();
    }

    [Fact]
    public void ScheduleConsultation_WhenStartInPast_Fails()
    {
        var clock = _fixture.Create<IClock>();
        var now = clock.GetCurrentInstant();
        var past = new TimeBlock(
            Start: now - Duration.FromMinutes(10),
            Duration: Duration.FromMinutes(30));

        var input = new ScheduleConsultation(
            Now: now,
            Id: _fixture.Create<ConsultationId>(),
            PatientId: _fixture.Create<PatientId>(),
            Schedule: new ConsultationSchedule.Scheduled(past),
            Attachments: _fixture.Create<ConsultationAttachments>(),
            Notes: null);

        var result = Consultation.TrySchedule(input);

        result.IsFailed.Should().BeTrue();
        result.Errors.Select(e => e.Message)
            .Should()
            .Contain(m => 
                m.Contains("Cannot schedule Consultation in the past"));
    }

    [Fact]
    public void ScheduleConsultation_WhenDurationTooShort_Fails()
    {
        var clock = _fixture.Create<IClock>();
        var now = clock.GetCurrentInstant();
        var timeBlock = new TimeBlock(
            Start: now + Duration.FromMinutes(5),
            Duration: Duration.FromMinutes(10)); 
        var input = new ScheduleConsultation(
            Now: now,
            Id: _fixture.Create<ConsultationId>(),
            PatientId: _fixture.Create<PatientId>(),
            Schedule: new ConsultationSchedule.Scheduled(timeBlock),
            Attachments: _fixture.Create<ConsultationAttachments>(),
            Notes: null);

        var result = Consultation.TrySchedule(input);

        result.IsFailed.Should().BeTrue();
        result.Errors.Select(e => e.Message)
            .Should().Contain(m => m.Contains("minimum"));
    }

    [Fact]
    public void ScheduleConsultation_WhenDurationTooLong_Fails()
    {
        var clock = _fixture.Create<IClock>();
        var now = clock.GetCurrentInstant();
        var timeBlock = new TimeBlock(
            Start: now + Duration.FromMinutes(5),
            Duration: Duration.FromHours(3)); 
        var input = new ScheduleConsultation(
            Now: now,
            Id: _fixture.Create<ConsultationId>(),
            PatientId: _fixture.Create<PatientId>(),
            Schedule: new ConsultationSchedule.Scheduled(timeBlock),
            Attachments: _fixture.Create<ConsultationAttachments>(),
            Notes: null);

        var result = Consultation.TrySchedule(input);

        result.IsFailed.Should().BeTrue();
        result.Errors.Select(e => e.Message)
            .Should().Contain(m => m.Contains("maximum"));
    }

    [Fact]
    public void UpdateDetails_WhenCancelled_Fails()
    {
        var scenario = _fixture.Create<ConsultationScheduledScenario>();
        var clock = _fixture.Create<IClock>();
        var now = clock.GetCurrentInstant();

        var cancelled = scenario.Consultation.TryCancelConsultation(
            new CancelConsultation(
                CancelledAt: now,
                Reason: (NonEmptyString)"test")).Value;

        var update = new UpdateConsultationDetails(
            Now: now,
            Schedule: scenario.Input.Schedule,
            Attachments: scenario.Input.Attachments,
            Notes: "ignored");

        var result = cancelled.TryUpdateDetails(update);

        result.IsFailed.Should().BeTrue();
        result.Errors.Select(e => e.Message)
            .Should()
            .Contain(m => 
                m.Contains("Cannot update a cancelled Consultation."));
    }

    [Fact]
    public void UpdateDetails_WhenScheduleUnchanged_AndPastNow_AllowsNotesAndAttachmentsUpdate()
    {
        var scenario = _fixture.Create<ConsultationScheduledScenario>();
        var current = scenario.Consultation;

        var clock = _fixture.Create<IClock>();
        var now = scenario.Input.Schedule.Start + Duration.FromMinutes(1); 
        var newAttachments = _fixture.Create<ConsultationAttachments>();
        var newNotes = "updated";

        var update = new UpdateConsultationDetails(
            Now: now,
            Schedule: (ConsultationSchedule.Scheduled)current.Schedule,
            Attachments: newAttachments,
            Notes: newNotes);

        var result = current.TryUpdateDetails(update);

        result.IsSuccess.Should().BeTrue();
        result.Value.Schedule.Should().Be(current.Schedule);
        result.Value.Attachments.Should().BeEquivalentTo(newAttachments);
        result.Value.Notes.Should().Be(newNotes);
    }

    [Fact]
    public void UpdateDetails_WhenNewScheduleInPast_Fails()
    {
        var scenario = _fixture.Create<ConsultationScheduledScenario>();
        var current = scenario.Consultation;

        var clock = _fixture.Create<IClock>();
        var now = clock.GetCurrentInstant();

        var past = new ConsultationSchedule.Scheduled(new(
            Start: now - Duration.FromMinutes(5), 
            Duration: Duration.FromMinutes(30)));

        var update = new UpdateConsultationDetails(
            Now: now,
            Schedule: past,
            Attachments: scenario.Input.Attachments,
            Notes: scenario.Input.Notes);

        var result = current.TryUpdateDetails(update);

        result.IsFailed.Should().BeTrue();
        result.Errors.Select(e => e.Message)
            .Should()
            .Contain(m => 
                m.Contains("Cannot schedule Consultation in the past"));
    }

    [Fact]
    public void UpdateDetails_WhenDurationTooShort_Fails()
    {
        var scenario = _fixture.Create<ConsultationScheduledScenario>();
        var current = scenario.Consultation;

        var clock = _fixture.Create<IClock>();
        var now = clock.GetCurrentInstant();

        var shortSched = new ConsultationSchedule.Scheduled(
            new TimeBlock(
                Start: now.Plus(Duration.FromMinutes(5)),
                Duration: Duration.FromMinutes(10)));

        var update = new UpdateConsultationDetails(
            Now: now,
            Schedule: shortSched,
            Attachments: scenario.Input.Attachments,
            Notes: scenario.Input.Notes);

        var result = current.TryUpdateDetails(update);

        result.IsFailed.Should().BeTrue();
        result.Errors.Select(e => e.Message)
            .Should().Contain(m => m.Contains("minimum"));
    }

    [Fact]
    public void UpdateDetails_WhenDurationTooLong_Fails()
    {
        var scenario = _fixture.Create<ConsultationScheduledScenario>();
        var current = scenario.Consultation;

        var clock = _fixture.Create<IClock>();
        var now = clock.GetCurrentInstant();

        var longSched = new ConsultationSchedule.Scheduled(new(
            Start: now.Plus( Duration.FromMinutes(5)),
            Duration: Duration.FromHours(3)));

        var update = new UpdateConsultationDetails(
            Now: now,
            Schedule: longSched,
            Attachments: scenario.Input.Attachments,
            Notes: scenario.Input.Notes);

        var result = current.TryUpdateDetails(update);

        result.IsFailed.Should().BeTrue();
        result.Errors.Select(e => e.Message)
            .Should().Contain(m => m.Contains("maximum"));
    }

    [Fact]
    public void UpdateDetails_WhenValid_UpdatesScheduleAttachmentsAndNotes()
    {
        var scenario = _fixture.Create<ConsultationScheduledScenario>();
        var current = scenario.Consultation;

        var clock = _fixture.Create<IClock>();
        var now = clock.GetCurrentInstant();

        var newSched = _fixture.Create<ConsultationSchedule.Scheduled>();
        var newAttachments = _fixture.Create<ConsultationAttachments>();
        var newNotes = "changed";

        var update = new UpdateConsultationDetails(
            Now: now,
            Schedule: newSched,
            Attachments: newAttachments,
            Notes: newNotes);

        var result = current.TryUpdateDetails(update);

        result.IsSuccess.Should().BeTrue();
        var updated = result.Value;
        updated.Schedule.Should().Be(newSched);
        updated.Attachments.Should().BeEquivalentTo(newAttachments);
        updated.Notes.Should().Be(newNotes);
        updated.Id.Should().Be(current.Id);
        updated.PatientId.Should().Be(current.PatientId);
    }

    [Fact]
    public void CancelConsultation_WhenAlreadyCancelled_Fails()
    {
        var scenario = _fixture.Create<ConsultationScheduledScenario>();
        var clock = _fixture.Create<IClock>();
        var now = clock.GetCurrentInstant();

        var first = scenario.Consultation.TryCancelConsultation(
            new CancelConsultation(now, (NonEmptyString)"first")).Value;

        var result = first.TryCancelConsultation(
            new CancelConsultation(now, (NonEmptyString)"second"));

        result.IsFailed.Should().BeTrue();
        result.Errors.Select(e => e.Message)
            .Should().Contain(m => m.Contains("already cancelled"));
    }

    [Fact]
    public void CancelConsultation_WhenAfterEnd_Fails()
    {
        var scenario = _fixture.Create<ConsultationScheduledScenario>();
        var c = scenario.Consultation;

        var cancelAt = c.Schedule.End + Duration.FromMinutes(1);

        var result = c.TryCancelConsultation(
            new CancelConsultation(cancelAt, (NonEmptyString)"too late"));

        result.IsFailed.Should().BeTrue();
        result.Errors.Select(e => e.Message)
            .Should().Contain(m => m.Contains("has ended"));
    }

    [Fact]
    public void CancelConsultation_WhenValid_SetsCancelledStateAndReason()
    {
        var scenario = _fixture.Create<ConsultationScheduledScenario>();
        var c = scenario.Consultation;

        var cancelAt = c.Schedule.Start;
        var reason = (NonEmptyString)"no longer needed";

        var result = c.TryCancelConsultation(
            new CancelConsultation(cancelAt, reason));

        result.IsSuccess.Should().BeTrue();
        var cancelled = result.Value;

        cancelled.IsCancelled.Should().BeTrue();
        cancelled.Schedule.Should().BeOfType<ConsultationSchedule.Cancelled>();

        var cancelledSchedule = 
            (ConsultationSchedule.Cancelled)cancelled.Schedule;

        cancelledSchedule.TimeBlock.Should().Be(c.Schedule.TimeBlock);
        cancelledSchedule.CancelledAt.Should().Be(cancelAt);
        cancelledSchedule.Reason.Should().Be(reason);
    }
}