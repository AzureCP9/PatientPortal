import type { PageContextServer } from "vike/types";
import { getConsultationByIdAsync } from "@/lib/api/consultations";
import { getAllPatientsAsync } from "@/lib/api/patients";
import type { ConsultationResponseDto } from "@/lib/api/consultations/types";
import type { PatientResponseDto } from "@/lib/api/patients/types";

export type PageData = {
    consultation: ConsultationResponseDto;
    patients: PatientResponseDto[];
};

export const data = async (ctx: PageContextServer): Promise<PageData> => {
    const id = ctx.routeParams.id!;
    const [consultation, patients] = await Promise.all([
        getConsultationByIdAsync(id),
        getAllPatientsAsync(),
    ]);

    return { consultation, patients };
};
