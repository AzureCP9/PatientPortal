import { PageWrapper } from "@/components/ui/PageWrapper";
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";
import { PatientForm } from "@/components/patients/PatientForm";
import { createPatientAsync } from "@/lib/api/patients";

const Page = () => {
    return (
        <PageWrapper title="Create patient">
            <div className="mx-auto max-w-3xl">
                <Card>
                    <CardHeader>
                        <CardTitle>Patient Form</CardTitle>
                        <p className="text-muted-foreground mt-1">
                            Enter patient details below.
                        </p>
                    </CardHeader>
                    <CardContent>
                        <PatientForm
                            onSubmit={(payload) => createPatientAsync(payload)}
                        />
                    </CardContent>
                </Card>
            </div>
        </PageWrapper>
    );
};

export default Page;
