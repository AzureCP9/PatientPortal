import * as React from "react";
import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from "@/components/ui/select";
import type { PatientResponseDto } from "@/lib/api/patients/types";
import { cn } from "@/lib/utils";

type SelectTriggerProps = React.ComponentProps<typeof SelectTrigger>;

type Props = {
    value: string;
    onChange: (value: string) => void;
    patients: PatientResponseDto[];
    disabled?: boolean;
} & Pick<SelectTriggerProps, "className">;

export const PatientSelect: React.FC<Props> = ({
    value,
    onChange,
    patients,
    disabled,
    className,
}) => {
    return (
        <Select value={value} onValueChange={onChange} disabled={disabled}>
            <SelectTrigger className={cn("w-full", className)}>
                <SelectValue placeholder="Select a patient" />
            </SelectTrigger>
            <SelectContent>
                {patients.map((p) => (
                    <SelectItem key={p.id} value={p.id}>
                        {`${p.name.firstName} ${p.name.lastName}`}
                    </SelectItem>
                ))}
            </SelectContent>
        </Select>
    );
};
