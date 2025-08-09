using Microsoft.Extensions.DependencyInjection;
using PatientPortal.Application.Consultations.Services;
using PatientPortal.Application.Consultations.Services.Interfaces;
using PatientPortal.Application.Patients.Services;
using PatientPortal.Application.Patients.Services.Interfaces;

namespace PatientPortal.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection self)
    {
        self.AddScoped<IPatientService, PatientService>();
        self.AddScoped<IConsultationService, ConsultationService>();

        return self;
    }
}