import { handleResponse } from "../common/requests";
import {
    ConsultationResponseDto,
    ScheduleConsultationRequestDto,
    ScheduleConsultationResponseDto,
    UpdateConsultationDetailsRequestDto,
} from "./types";

const API_BASE_URL = import.meta.env.PUBLIC_ENV__API_BASE_URL;

const jsonHeaders = {
    "content-type": "application/json",
    accept: "application/problem+json, application/json",
} as const;

export const getAllConsultationsAsync = async (): Promise<
    ConsultationResponseDto[]
> => {
    const res = await fetch(`${API_BASE_URL}/consultations/all`, {
        headers: { accept: "application/problem+json, application/json" },
    });
    return handleResponse(res, "Failed to fetch consultations.");
};

export const getConsultationByIdAsync = async (
    id: string
): Promise<ConsultationResponseDto> => {
    const res = await fetch(`${API_BASE_URL}/consultations/${id}`, {
        headers: { accept: "application/problem+json, application/json" },
    });
    return handleResponse(res, "Failed to fetch consultation.");
};

export const scheduleConsultationAsync = async (
    data: ScheduleConsultationRequestDto
): Promise<ScheduleConsultationResponseDto> => {
    const res = await fetch(`${API_BASE_URL}/consultations`, {
        method: "POST",
        headers: jsonHeaders,
        body: JSON.stringify(data),
    });
    return handleResponse(res, "Failed to schedule consultation.");
};

export const updateConsultationDetailsAsync = async (
    id: string,
    data: UpdateConsultationDetailsRequestDto
): Promise<void> => {
    const res = await fetch(`${API_BASE_URL}/consultations/${id}`, {
        method: "PUT",
        headers: jsonHeaders,
        body: JSON.stringify(data),
    });
    return handleResponse<void>(res, "Failed to update consultation.");
};

export const cancelConsultationAsync = async (id: string): Promise<void> => {
    const res = await fetch(`${API_BASE_URL}/consultations/${id}/cancel`, {
        method: "PUT",
        headers: { accept: "application/problem+json, application/json" },
    });
    return handleResponse<void>(res, "Failed to cancel consultation.");
};

export const uploadConsultationAttachmentAsync = async (
    file: File
): Promise<string> => {
    const formData = new FormData();
    formData.append("file", file);

    const res = await fetch(
        `${API_BASE_URL}/consultations/attachments/upload`,
        {
            method: "POST",
            // don't set content-type for FormData; the browser will set the boundary
            headers: { accept: "application/problem+json, application/json" },
            body: formData,
        }
    );

    const data = await handleResponse<{ url: string }>(
        res,
        "Failed to upload attachment."
    );
    return data.url;
};
