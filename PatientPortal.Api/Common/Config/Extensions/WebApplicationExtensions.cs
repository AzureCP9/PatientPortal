using PatientPortal.Api.Consultations;
using PatientPortal.Api.Patients;

namespace PatientPortal.Api.Common.Config.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureEndpoints(this WebApplication self)
    {
        self.MapPatientsEndpoints();
        self.MapConsultationsEndpoints();
        return self;
    }
}
