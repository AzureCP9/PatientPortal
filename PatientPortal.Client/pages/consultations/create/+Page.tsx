import { useData } from "vike-react/useData";
import type { PageData } from "./+data";
import { PageWrapper } from "@/components/ui/PageWrapper";
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";
import { ConsultationForm } from "@/components/consultations/ConsultationForm";
import { scheduleConsultationAsync } from "@/lib/api/consultations";

const Page = () => {
    const { patients } = useData<PageData>();

    return (
        <PageWrapper title="Schedule Consultation">
            <div className="mx-auto max-w-3xl">
                <Card>
                    <CardHeader>
                        <CardTitle>Consultation Form</CardTitle>
                        <p className="text-muted-foreground mt-1">
                            Fill in the details to schedule a consultation.
                        </p>
                    </CardHeader>
                    <CardContent>
                        <ConsultationForm
                            patients={patients ?? []}
                            onSubmit={(payload) =>
                                scheduleConsultationAsync(payload)
                            }
                        />
                    </CardContent>
                </Card>
            </div>
        </PageWrapper>
    );
};

export default Page;
