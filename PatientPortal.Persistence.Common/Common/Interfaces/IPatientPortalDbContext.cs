using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PatientPortal.Persistence.Abstractions.Consultations;
using PatientPortal.Persistence.Abstractions.Patients;

namespace PatientPortal.Persistence.Abstractions.Common.Interfaces;

public interface IPatientPortalDbContext
{
    ChangeTracker ChangeTracker { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    EntityEntry<T> Entry<T>(T entity) where T : class;
    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    DbSet<PatientDbEntity> Patients { get; }
    DbSet<ConsultationDbEntity> Consultations { get; }
}