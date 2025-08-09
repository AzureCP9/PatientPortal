using NodaTime;
using PatientPortal.Domain.Consultations;
using PatientPortal.Domain.Values;

namespace PatientPortal.Domain.Fixtures.Consultations;

public class ConsultationScheduleCustomization(
    TimeBlock? withTimeBlock = null) 
    : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<ConsultationSchedule.Scheduled>(composer =>
            composer.FromFactory(() =>
            {
                var clock = fixture.Create<IClock>();
                var now = clock.GetCurrentInstant();

                var timeBlock = withTimeBlock 
                    ?? new TimeBlock(
                        Start: now.Plus(Duration.FromHours(1)),
                        Duration: Duration.FromMinutes(30));

                return new ConsultationSchedule.Scheduled(timeBlock);
            }).OmitAutoProperties());
    }
}