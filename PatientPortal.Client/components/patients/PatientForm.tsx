import * as React from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from "@/components/ui/Form/form";
import type {
    PatientResponseDto,
    CreatePatientRequestDto,
} from "@/lib/api/patients/types";
import { toast } from "sonner";
import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from "@/components/ui/select";
import { GenderEnum } from "@/lib/api/common";

const schema = z.object({
    name: z.object({
        firstName: z.string().min(1, "Required"),
        middleNamesText: z.string(),
        lastName: z.string().min(1, "Required"),
    }),
    gender: z
        .string()
        .min(1, "Required")
        .refine((val) => val === "Male" || val === "Female", {
            message: "Must select Male or Female",
        }),
    age: z
        .string()
        .min(1, "Required")
        .refine((v) => !Number.isNaN(Number(v)), "Must be a number")
        .refine((v) => {
            const n = Number(v);
            return Number.isInteger(n) && n >= 0 && n <= 130;
        }, "Out of range"),
});

type FormValues = z.infer<typeof schema>;

type Props = {
    initial?: PatientResponseDto | null;
    onSubmit: (
        data: CreatePatientRequestDto
    ) => Promise<void | PatientResponseDto> | void;
};

export const PatientForm: React.FC<Props> = ({ initial, onSubmit }) => {
    const isEdit = Boolean(initial);

    const form = useForm<FormValues>({
        resolver: zodResolver(schema),
        defaultValues: initial
            ? {
                  name: {
                      firstName: initial.name.firstName,
                      middleNamesText: (initial.name.middleNames ?? []).join(
                          " "
                      ),
                      lastName: initial.name.lastName,
                  },
                  gender: initial.gender,
                  age: String(initial.age),
              }
            : {
                  name: { firstName: "", middleNamesText: "", lastName: "" },
                  gender: "",
                  age: "",
              },
    });

    const handleSubmit = form.handleSubmit(async (values) => {
        try {
            const middleNames = values.name.middleNamesText
                .split(/[,\s]+/)
                .map((s) => s.trim())
                .filter(Boolean);

            const genderValue = values.gender as GenderEnum;

            const dto: CreatePatientRequestDto = {
                name: {
                    firstName: values.name.firstName,
                    middleNames,
                    lastName: values.name.lastName,
                },
                gender: genderValue,
                age: Number(values.age),
            };

            await onSubmit(dto);

            toast.success(
                isEdit ? "Patient details updated" : "Patient created"
            );

            if (!isEdit) {
                window.location.href = "/patients";
            }
        } catch {
            toast.error("Something went wrong");
        }
    });

    const { control, formState } = form;
    const { isSubmitting } = formState;

    return (
        <Form {...form}>
            <form onSubmit={handleSubmit} noValidate>
                <div className="flex flex-col">
                    <FormField
                        control={control}
                        name="name.firstName"
                        render={({ field }) => (
                            <FormItem >
                                <FormLabel>
                                    First name{" "}
                                    <span className="text-destructive">*</span>
                                </FormLabel>
                                <FormControl>
                                    <Input
                                        placeholder="John"
                                        disabled={isSubmitting}
                                        {...field}
                                    />
                                </FormControl>
                                <FormMessage/>
                            </FormItem>
                        )}
                    />

                    <FormField
                        control={control}
                        name="name.middleNamesText"
                        render={({ field }) => (
                            <FormItem >
                                <FormLabel>Middle names</FormLabel>
                                <FormControl>
                                    <Input
                                        placeholder="Middle Names Here"
                                        disabled={isSubmitting}
                                        {...field}
                                    />
                                </FormControl>
                                <FormMessage/>
                            </FormItem>
                        )}
                    />

                    <FormField
                        control={control}
                        name="name.lastName"
                        render={({ field }) => (
                            <FormItem >
                                <FormLabel>
                                    Last name{" "}
                                    <span className="text-destructive">*</span>
                                </FormLabel>
                                <FormControl>
                                    <Input
                                        placeholder="Doe"
                                        disabled={isSubmitting}
                                        {...field}
                                    />
                                </FormControl>
                                <FormMessage />
                            </FormItem>
                        )}
                    />
                </div>

                <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
                    <FormField
                        control={control}
                        name="gender"
                        render={({ field }) => (
                            <FormItem className="w-full">
                                <FormLabel>
                                    Gender{" "}
                                    <span className="text-destructive">*</span>
                                </FormLabel>
                                <Select
                                    value={field.value}
                                    onValueChange={field.onChange}
                                    disabled={isSubmitting}
                                >
                                    <FormControl>
                                        <SelectTrigger className="w-full">
                                            <SelectValue placeholder="Select gender" />
                                        </SelectTrigger>
                                    </FormControl>
                                    <SelectContent className="w-[var(--radix-select-trigger-width)]">
                                        <SelectItem value="Male">
                                            Male
                                        </SelectItem>
                                        <SelectItem value="Female">
                                            Female
                                        </SelectItem>
                                    </SelectContent>
                                </Select>
                                <FormMessage />
                            </FormItem>
                        )}
                    />

                    <FormField
                        control={control}
                        name="age"
                        render={({ field }) => (
                            <FormItem >
                                <FormLabel>
                                    Age{" "}
                                    <span className="text-destructive">*</span>
                                </FormLabel>
                                <FormControl>
                                    <Input
                                        type="number"
                                        inputMode="numeric"
                                        min={0}
                                        max={130}
                                        step={1}
                                        disabled={isSubmitting}
                                        {...field}
                                    />
                                </FormControl>
                                <FormMessage/>
                            </FormItem>
                        )}
                    />
                </div>

                <div className="flex justify-end gap-2">
                    <Button type="submit" disabled={isSubmitting}>
                        {isEdit ? "Save Changes" : "Create Patient"}
                    </Button>
                </div>
            </form>
        </Form>
    );
};
