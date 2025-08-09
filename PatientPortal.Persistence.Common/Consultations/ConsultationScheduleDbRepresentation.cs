//using NodaTime;
//using PatientPortal.Domain.Common;
//using PatientPortal.Domain.Consultations;
//using PatientPortal.Domain.Values;

//namespace PatientPortal.Persistence.Abstractions.Consultations;

//public class ConsultationScheduleDbRepresentation
//{
//    public required ConsultationId ConsultationId { get; set; }
//    public required ConsultationScheduleDiscriminator Discriminator { get; set; }
    
//}

//public enum ConsultationScheduleDiscriminator
//{
//    Scheduled,
//    Cancelled
//}

//public static class ConsultationScheduleRepresentationExtensions
//{
//    public static Instant End(this ConsultationScheduleDbRepresentation self) =>
//        self.Start + self.Duration;

//    public static ConsultationScheduleDbRepresentation ToRepresentation(
//        this ConsultationSchedule self,
//        ConsultationId consultationId) =>
//            self switch
//            {
//                ConsultationSchedule.Scheduled scheduled => 
//                    new ConsultationScheduleDbRepresentation
//                    {
//                        ConsultationId = consultationId,
//                        Discriminator = 
//                            ConsultationScheduleDiscriminator.Scheduled,
//                        Start = scheduled.Start,
//                        Duration = scheduled.Duration
//                    },

//                ConsultationSchedule.Cancelled cancelled => 
//                    new ConsultationScheduleDbRepresentation
//                    {
//                        ConsultationId = consultationId,
//                        Discriminator = 
//                            ConsultationScheduleDiscriminator.Cancelled,
//                        Start = cancelled.Start,
//                        Duration = cancelled.Duration,
//                        CancelledAt = cancelled.CancelledAt,
//                        CancellationReason = cancelled.Reason
//                    },

//                _ => throw new ArgumentOutOfRangeException(
//                    nameof(self), "Unknown consultation schedule type.")
//            };

//    public static ConsultationSchedule ToDomain(
//        this ConsultationScheduleDbRepresentation self) =>
//            self.Discriminator switch
//            {
//                ConsultationScheduleDiscriminator.Scheduled =>
//                    new ConsultationSchedule.Scheduled(
//                        new(self.Start, self.Duration)),

//                ConsultationScheduleDiscriminator.Cancelled when
//                    self.CancelledAt.HasValue 
//                    && self.CancellationReason is not null =>
//                        new ConsultationSchedule.Cancelled(
//                            TimeBlock: new(self.Start, self.Duration),
//                            CancelledAt: self.CancelledAt.Value,
//                            Reason: (NonEmptyString)self.CancellationReason),

//                _ => throw new ArgumentOutOfRangeException(
//                    nameof(self.Discriminator),
//                    self.Discriminator,
//                    "Unknown schedule discriminator.")
//            };
//}