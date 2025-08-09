import type { PageContextServer } from "vike/types";
import type { PatientResponseDto } from "@/lib/api/patients/types";
import { getAllPatientsAsync } from "@/lib/api/patients";

export type PageData = { patients: PatientResponseDto[] };

export const data = async (
    _pageContext: PageContextServer
): Promise<PageData> => {
    const patients = await getAllPatientsAsync();
    return { patients };
};
