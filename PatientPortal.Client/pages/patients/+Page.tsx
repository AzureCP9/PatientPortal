import { useData } from "vike-react/useData";
import type { PageData } from "./+data";
import { PatientsTable } from "@/components/patients/PatientsTable";
import { Button } from "@/components/ui/button";
import { Plus } from "lucide-react";
import { PageWrapper } from "@/components/ui/PageWrapper";

const Page = () => {
    const { patients } = useData<PageData>();

    return (
        <PageWrapper title="Patients">
            <div className="flex flex-col gap-4">
                <Button asChild className="self-end">
                    <a href="/patients/create">
                        <Plus className="mr-2 h-4 w-4" /> New patient
                    </a>
                </Button>
                <div className="w-full rounded-md border">
                    <PatientsTable data={patients ?? []} />
                </div>
            </div>
        </PageWrapper>
    );
};

export default Page;
