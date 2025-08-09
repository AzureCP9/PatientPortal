import { useData } from "vike-react/useData";
import type { PageData } from "./+data";
import { ConsultationsTable } from "@/components/consultations/ConsultationsTable";
import { Button } from "@/components/ui/button";
import { Plus } from "lucide-react";
import { PageWrapper } from "@/components/ui/PageWrapper";

const Page = () => {
    const { consultations } = useData<PageData>();

    return (
        <PageWrapper title="Consultations">
            <div className="flex flex-col gap-4">
                <Button asChild className="self-end">
                    <a href="/consultations/create">
                        <Plus className="mr-2 h-4 w-4" /> New consultation
                    </a>
                </Button>
                <div className="w-full rounded-md border">
                    <ConsultationsTable data={consultations ?? []} />
                </div>
            </div>
        </PageWrapper>
    );
};

export default Page;
