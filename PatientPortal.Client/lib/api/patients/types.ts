import { GenderEnum, PersonNameDto } from "../common";

export interface PatientResponseDto {
    id: string;
    name: PersonNameDto;
    gender: GenderEnum;
    age: number;
}

export interface CreatePatientRequestDto {
    name: PersonNameDto;
    gender: GenderEnum;
    age: number;
}

export interface UpdatePatientDetailsRequestDto {
    name: PersonNameDto;
    gender: GenderEnum;
    age: number;
}
