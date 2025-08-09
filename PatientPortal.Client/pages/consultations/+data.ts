import type { PageContextServer } from "vike/types";
import type { ConsultationResponseDto } from "@/lib/api/consultations/types";
import { getAllConsultationsAsync } from "@/lib/api/consultations";

export type PageData = { consultations: ConsultationResponseDto[] };

export const data = async (
    _pageContext: PageContextServer
): Promise<PageData> => {
    const consultations = await getAllConsultationsAsync();
    return { consultations };
};
