import { useData } from "vike-react/useData";
import type { PageData } from "./+data";
import { PatientForm } from "@/components/patients/PatientForm";
import { updatePatientDetailsAsync } from "@/lib/api/patients";
import { PageWrapper } from "@/components/ui/PageWrapper";
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";

const Page = () => {
    const { patient } = useData<PageData>();

    return (
        <PageWrapper title="Edit patient">
            <div className="mx-auto max-w-3xl">
                <Card>
                    <CardHeader>
                        <CardTitle>Patient Form</CardTitle>
                        <p className="text-muted-foreground mt-1">
                            Update patient details below.
                        </p>
                    </CardHeader>
                    <CardContent>
                        <PatientForm
                            initial={patient}
                            onSubmit={(payload) =>
                                updatePatientDetailsAsync(patient.id, payload)
                            }
                        />
                    </CardContent>
                </Card>
            </div>
        </PageWrapper>
    );
};

export default Page;
