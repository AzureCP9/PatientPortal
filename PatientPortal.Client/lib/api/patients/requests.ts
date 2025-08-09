import { handleResponse } from "../common/requests";
import type {
    PatientResponseDto,
    CreatePatientRequestDto,
    UpdatePatientDetailsRequestDto,
} from "./types";

const API_BASE_URL = import.meta.env.PUBLIC_ENV__API_BASE_URL;

export const getAllPatientsAsync = async (): Promise<PatientResponseDto[]> => {
    const res = await fetch(`${API_BASE_URL}/patients/all`);
    return handleResponse(res, "Failed to fetch Patients.");
};

export const getPatientByIdAsync = async (
    id: string
): Promise<PatientResponseDto> => {
    const res = await fetch(`${API_BASE_URL}/patients/${id}`);
    return handleResponse(res, "Failed to fetch Patient.");
};

export const createPatientAsync = async (
    data: CreatePatientRequestDto
): Promise<PatientResponseDto> => {
    const res = await fetch(`${API_BASE_URL}/patients`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
    });
    return handleResponse(res, "Failed to create Patient.");
};

export const updatePatientDetailsAsync = async (
    id: string,
    data: UpdatePatientDetailsRequestDto
): Promise<void> => {
    const res = await fetch(`${API_BASE_URL}/patients/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
    });

    if (!res.ok) {
        let errorMessage = "Failed to update patient.";
        try {
            const errorData = await res.json();
            if (errorData?.detail) {
                errorMessage = errorData.detail;
            } else if (errorData?.title) {
                errorMessage = errorData.title;
            }
        } catch {}
        throw new Error(`${errorMessage} ${res.status}`);
    }

    return;
};
