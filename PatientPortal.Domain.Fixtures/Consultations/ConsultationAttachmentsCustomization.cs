using PatientPortal.Domain.Common;
using PatientPortal.Domain.Consultations;

namespace PatientPortal.Domain.Fixtures.Consultations;

public class ConsultationAttachmentsCustomization(
    IEnumerable<AbsoluteUri>? withUploads = null
) : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<ConsultationAttachments>(composer =>
            composer.FromFactory(() =>
            {
                var uploads = withUploads
                    ?? fixture.CreateMany<AbsoluteUri>(2);

                return ConsultationAttachments.TryCreate(uploads).Value;
            }).OmitAutoProperties());
    }
}
