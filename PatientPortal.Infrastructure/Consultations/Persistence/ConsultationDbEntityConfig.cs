using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatientPortal.Domain.Consultations;
using PatientPortal.Domain.Patients;
using PatientPortal.Infrastructure.Common.Persistence;
using PatientPortal.Infrastructure.Common.Persistence.Extensions;
using PatientPortal.Persistence.Abstractions.Consultations;

namespace PatientPortal.Infrastructure.Consultations.Persistence;

public class ConsultationDbEntityConfig
    : IEntityTypeConfiguration<ConsultationDbEntity>
{
    public void Configure(EntityTypeBuilder<ConsultationDbEntity> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasTypedIdConversion(ConsultationId.FromGuid, id => id);

        builder.Property(c => c.Notes)
            .HasMaxLength(DataStandards.FreeTextMaxLength);

        builder.Property(c => c.PatientId)
            .HasTypedIdConversion(PatientId.FromGuid, id => id)
            .IsRequired();

        builder.HasOne(c => c.Patient)
            .WithMany()
            .HasForeignKey(c => c.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(c => c.ScheduleStart)
            .HasColumnName("Schedule_Start")
            .IsRequired();

        builder.Property(c => c.ScheduleDuration)
            .HasColumnName("Schedule_Duration")
            .IsRequired();

        builder.Property(c => c.ScheduleEnd)
            .HasColumnName("Schedule_End")
            .IsRequired();

        builder.Property(c => c.ScheduleCancelledAt)
            .HasColumnName("Schedule_CancelledAt");

        builder.Property(c => c.ScheduleCancellationReason)
            .HasColumnName("Schedule_CancellationReason")
            .HasMaxLength(DataStandards.FreeTextMaxLength);

        builder.OwnsMany(c => c.Attachments, attachments =>
        {
            attachments.ToTable("ConsultationAttachments");

            attachments.WithOwner().HasForeignKey(DbFields.ConsultationId);

            attachments.Property(a => a.Uri)
                .IsRequired();

            attachments.Property<int>(DbFields.Id)
                .ValueGeneratedOnAdd();

            attachments.HasKey(DbFields.Id);
        });
    }
}
