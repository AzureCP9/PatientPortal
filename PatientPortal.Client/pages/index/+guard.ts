import { redirect, render } from "vike/abort";
import { GuardAsync } from "vike/types";

export const guard: GuardAsync = async (): ReturnType<GuardAsync> => {
    throw redirect("/consultations");
};
