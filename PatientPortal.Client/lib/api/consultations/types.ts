export interface ConsultationScheduleDto {
    start: string;
    end: string;
    durationInMinutes: number;
    cancelledAt?: string | null;
    cancellationReason?: string | null;
}

export interface ConsultationResponseDto {
    id: string;
    patientId: string;
    patientName: string;
    schedule: ConsultationScheduleDto;
    attachmentUris: string[];
    notes?: string | null;
}

export interface ScheduleConsultationRequestDto {
    patientId: string;
    start: string;
    durationInMinutes: number;
    attachmentUris: string[];
    notes?: string | null;
}

export interface ScheduleConsultationResponseDto {
    id: string;
}

export interface UpdateConsultationDetailsRequestDto {
    start: string;
    durationInMinutes: number;
    attachmentUris: string[];
    notes?: string | null;
}
