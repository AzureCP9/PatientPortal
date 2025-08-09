import * as React from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from "@/components/ui/Form/form";
import type {
    ScheduleConsultationRequestDto,
    ScheduleConsultationResponseDto,
    ConsultationResponseDto,
} from "@/lib/api/consultations/types";
import type { PatientResponseDto } from "@/lib/api/patients/types";
import { toast } from "sonner";
import { uploadConsultationAttachmentAsync } from "@/lib/api/consultations";
import { getMimeInfo } from "@/lib/models/common/mimeType";
import { FormMultiUpload } from "../ui/Form/FormMultiUpload";
import { PatientSelect } from "../patients/PatientSelect";
import { navigate } from "vike/client/router";

const schema = z.object({
    patientId: z.string().min(1, "Required."),
    date: z.string().min(1, "Required."),
    time: z.string().min(1, "Required."),
    notes: z.string().optional(),
    attachments: z
        .array(z.string().url())
        .max(5, "Up to 5 attachments allowed")
        .optional(),
});

type FormValues = z.infer<typeof schema>;

type Props = {
    initial?: ConsultationResponseDto | null;
    patients: PatientResponseDto[];
    onSubmit: (
        data: ScheduleConsultationRequestDto
    ) => Promise<ScheduleConsultationResponseDto | void> | void;
};

const pad = (n: number) => String(n).padStart(2, "0");
const toLocalDate = (iso: string) => {
    const d = new Date(iso);
    return `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}`;
};
const toLocalTime = (iso: string) => {
    const d = new Date(iso);
    return `${pad(d.getHours())}:${pad(d.getMinutes())}`;
};

const MessageRow = ({ children }: { children?: React.ReactNode }) => (
    <div className="mt-1 min-h-[16px] text-sm leading-tight">{children}</div>
);

export const ConsultationForm: React.FC<Props> = ({
    initial,
    patients,
    onSubmit,
}) => {
    const isEdit = Boolean(initial);
    const isCancelled = Boolean(initial?.schedule.cancelledAt);

    const hasStarted =
        !!initial && new Date(initial.schedule.start) <= new Date();

    const form = useForm<FormValues>({
        resolver: zodResolver(schema),
        defaultValues: initial
            ? {
                  patientId: initial.patientId,
                  date: toLocalDate(initial.schedule.start),
                  time: toLocalTime(initial.schedule.start),
                  notes: initial.notes ?? "",
                  attachments: initial.attachmentUris ?? [],
              }
            : {
                  patientId: "",
                  date: "",
                  time: "",
                  notes: "",
                  attachments: [],
              },
    });

    const handleSubmit = form.handleSubmit(async (values) => {
        try {
            const local = new Date(`${values.date}T${values.time}:00`);
            const start = local.toISOString();
            const dto: ScheduleConsultationRequestDto = {
                patientId: values.patientId,
                start,
                durationInMinutes: 30,
                attachmentUris: values.attachments ?? [],
                notes: values.notes?.trim() || null,
            };
            await onSubmit(dto);
            toast.success(
                isEdit ? "Consultation updated" : "Consultation scheduled"
            );
            if (!isEdit) navigate('/consultations')
        } catch (error) {
            toast.error(
                error instanceof Error ? error.message : "Something went wrong"
            );
        }
    });

    const { control, formState } = form;
    const { isSubmitting, errors } = formState;
    const allDisabled = isSubmitting || isCancelled;

    return (
        <Form {...form}>
            <form onSubmit={handleSubmit} className="space-y-5" noValidate>
                <fieldset
                    disabled={allDisabled}
                    className={allDisabled ? "opacity-75" : undefined}
                >
                    {/* Patient */}
                    <FormField
                        control={control}
                        name="patientId"
                        render={({ field }) => (
                            <FormItem>
                                <FormLabel>
                                    Patient{" "}
                                    <span className="text-destructive">*</span>
                                </FormLabel>
                                <FormControl>
                                    <PatientSelect
                                        value={field.value}
                                        onChange={field.onChange}
                                        disabled={
                                            isSubmitting ||
                                            isEdit ||
                                            isCancelled
                                        }
                                        patients={patients}
                                        className={[
                                            errors.patientId
                                                ? "border-red-500 ring-red-500"
                                                : "",
                                            isEdit || isCancelled
                                                ? "pointer-events-none opacity-60"
                                                : "",
                                        ]
                                            .filter(Boolean)
                                            .join(" ")}
                                    />
                                </FormControl>
                                <MessageRow>
                                    <FormMessage />
                                </MessageRow>
                            </FormItem>
                        )}
                    />

                    {/* Date & Time */}
                    <div className="grid grid-cols-1 gap-5 sm:grid-cols-2">
                        <FormField
                            control={control}
                            name="date"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>
                                        Date{" "}
                                        <span className="text-destructive">
                                            *
                                        </span>
                                    </FormLabel>
                                    <FormControl>
                                        <Input
                                            type="date"
                                            {...field}
                                            disabled={
                                                hasStarted ||
                                                isCancelled ||
                                                isSubmitting
                                            }
                                            className={
                                                errors.date
                                                    ? "border-red-500 ring-red-500"
                                                    : ""
                                            }
                                        />
                                    </FormControl>
                                    <MessageRow>
                                        <FormMessage />
                                    </MessageRow>
                                </FormItem>
                            )}
                        />
                        <FormField
                            control={control}
                            name="time"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>
                                        Time{" "}
                                        <span className="text-destructive">
                                            *
                                        </span>
                                    </FormLabel>
                                    <FormControl>
                                        <Input
                                            type="time"
                                            {...field}
                                            disabled={
                                                hasStarted ||
                                                isCancelled ||
                                                isSubmitting
                                            }
                                            className={
                                                errors.time
                                                    ? "border-red-500 ring-red-500"
                                                    : ""
                                            }
                                        />
                                    </FormControl>
                                    <MessageRow>
                                        <FormMessage />
                                    </MessageRow>
                                </FormItem>
                            )}
                        />
                    </div>

                    {/* Notes */}
                    <FormField
                        control={control}
                        name="notes"
                        render={({ field }) => (
                            <FormItem>
                                <FormLabel>Notes</FormLabel>
                                <FormControl>
                                    <Textarea
                                        placeholder="Additional details…"
                                        {...field}
                                        className={
                                            errors.notes
                                                ? "border-red-500 ring-red-500"
                                                : ""
                                        }
                                    />
                                </FormControl>
                                <MessageRow>
                                    <FormMessage />
                                </MessageRow>
                            </FormItem>
                        )}
                    />

                    {/* Attachments */}
                    <FormField
                        control={control}
                        name="attachments"
                        render={({ field, fieldState }) => (
                            <div>
                                <FormMultiUpload
                                    field={field}
                                    error={fieldState.error?.message}
                                    label="Attachments"
                                    maxFiles={5}
                                    acceptedMimeTypes={getMimeInfo.fromLabels([
                                        "JPEG",
                                        "PNG",
                                        "PDF",
                                        "Word (DOCX)",
                                        "Word (DOC)",
                                    ])}
                                    maxFileSizeMb={10}
                                    uploadHandler={
                                        uploadConsultationAttachmentAsync
                                    }
                                    disabled={allDisabled}
                                />
                                <MessageRow>
                                    <FormMessage />
                                </MessageRow>
                            </div>
                        )}
                    />
                </fieldset>

                <div className="flex justify-end gap-2">
                    <Button
                        type="submit"
                        disabled={isSubmitting || isCancelled}
                    >
                        {isCancelled
                            ? "Cancelled"
                            : isEdit
                              ? "Save Changes"
                              : "Schedule Consultation"}
                    </Button>
                </div>
            </form>
        </Form>
    );
};
