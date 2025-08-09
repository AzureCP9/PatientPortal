import type { PageContextServer } from "vike/types";
import { getPatientByIdAsync } from "@/lib/api/patients";
import type { PatientResponseDto } from "@/lib/api/patients/types";

export type PageData = { patient: PatientResponseDto };

export const data = async (ctx: PageContextServer): Promise<PageData> => {
    const id = ctx.routeParams.id!;
    const patient = await getPatientByIdAsync(id);
    return { patient };
};
