import type { ControllerRenderProps, FieldValues, Path } from "react-hook-form";
import { CircleX, Upload } from "lucide-react";
import { useRef } from "react";
import { useFormContext } from "react-hook-form";
import { MimeInfo } from "@/lib/models/common/mimeType";
import { Button } from "../button";
import { FormItem, FormLabel, FormMessage } from "./form";

interface FormMultiUploadProps<T extends FieldValues, U extends Path<T>> {
    field: ControllerRenderProps<T, U>;
    uploadHandler: (file: File) => Promise<string>;
    error?: string;
    label?: string;
    maxFiles?: number;
    acceptedMimeTypes?: MimeInfo[];
    maxFileSizeMb?: number;
    onUploadComplete?: (uploadedUris: string[]) => void;
    onUploadClear?: () => void;
    disabled?: boolean;
}

function getNameFromUri(uri: string): string {
    try {
        const path = uri.includes("://")
            ? new URL(uri).pathname
            : new URL(uri, "http://local").pathname;
        const last = path.split("/").filter(Boolean).pop();
        return decodeURIComponent(last ?? uri);
    } catch {
        const parts = uri.split("/").filter(Boolean);
        return decodeURIComponent(parts.pop() ?? uri);
    }
}

export function FormMultiUpload<T extends FieldValues, U extends Path<T>>({
    field,
    uploadHandler,
    error,
    label,
    maxFiles = 3,
    acceptedMimeTypes = [],
    maxFileSizeMb,
    onUploadComplete,
    onUploadClear,
    disabled = false,
}: FormMultiUploadProps<T, U>) {
    const fileInputRef = useRef<HTMLInputElement>(null);
    const { setError, clearErrors } = useFormContext();

    const value = (field.value ?? []) as string[];
    const hasFiles = value.length > 0;

    const safeSetClick = () => {
        if (disabled) return;
        fileInputRef.current?.click();
    };

    const handleFiles = async (e: React.ChangeEvent<HTMLInputElement>) => {
        if (disabled) return;
        const files = Array.from(e.target.files ?? []);
        const currentUris = value;
        const allowedCount = Math.max(0, maxFiles - currentUris.length);
        if (files.length > allowedCount) {
            setError(field.name, {
                type: "manual",
                message: `You can upload up to ${maxFiles} files.`,
            });
            return;
        }
        const filesToCheck = files.slice(0, allowedCount);

        if (maxFileSizeMb) {
            const maxBytes = maxFileSizeMb * 1024 * 1024;
            const oversized = filesToCheck.filter((f) => f.size > maxBytes);
            if (oversized.length) {
                setError(field.name, {
                    type: "manual",
                    message: `The following file(s) exceed ${maxFileSizeMb} MB: ${oversized.map((f) => f.name).join(", ")}.`,
                });
                return;
            }
        }

        if (acceptedMimeTypes.length > 0) {
            const valid = filesToCheck.filter((file) =>
                acceptedMimeTypes.some((mi) => file.type.startsWith(mi.mime))
            );
            if (valid.length < filesToCheck.length) {
                setError(field.name, {
                    type: "manual",
                    message: `Only the following types are accepted: ${acceptedMimeTypes
                        .map((t) => t.extensions.join(" "))
                        .join(" ")}.`,
                });
                return;
            }
        }

        try {
            clearErrors(field.name);
            const uploadedUris = await Promise.all(
                filesToCheck.map(uploadHandler)
            );
            onUploadComplete?.(uploadedUris);
            field.onChange([...currentUris, ...uploadedUris]);
        } catch {
            setError(field.name, {
                type: "manual",
                message: "One or more files failed to upload.",
            });
        } finally {
            if (fileInputRef.current) fileInputRef.current.value = "";
        }
    };

    const handleClear = () => {
        if (disabled) return;
        field.onChange([]);
        onUploadClear?.();
    };

    return (
        <FormItem
            className={disabled ? "cursor-not-allowed opacity-60" : undefined}
            aria-disabled={disabled}
        >
            {label && <FormLabel>{label}</FormLabel>}

            <div className="space-y-2">
                {!hasFiles ? (
                    <Button
                        variant="default"
                        type="button"
                        onClick={safeSetClick}
                        disabled={disabled}
                    >
                        <Upload className="h-4 w-4" />
                    </Button>
                ) : (
                    <div className="flex items-center gap-2">
                        <Button
                            variant="default"
                            type="button"
                            onClick={safeSetClick}
                            disabled={disabled}
                        >
                            <Upload className="h-4 w-4" />
                        </Button>
                        <Button
                            variant="destructive"
                            type="button"
                            onClick={handleClear}
                            disabled={disabled}
                        >
                            <CircleX className="h-4 w-4" />
                        </Button>
                    </div>
                )}

                <input
                    type="file"
                    multiple
                    accept={
                        acceptedMimeTypes.length > 0
                            ? acceptedMimeTypes.map((t) => t.mime).join(", ")
                            : undefined
                    }
                    className="hidden"
                    ref={fileInputRef}
                    onChange={handleFiles}
                    disabled={disabled}
                    aria-disabled={disabled}
                />

                {hasFiles && (
                    <ul className="text-muted-foreground space-y-1 text-sm">
                        {value.map((uri, idx) => (
                            <li key={`${uri}-${idx}`}>
                                <a
                                    href={uri}
                                    target="_blank"
                                    rel="noopener noreferrer"
                                    className="hover:text-foreground underline underline-offset-2"
                                >
                                    {getNameFromUri(uri)}
                                </a>
                            </li>
                        ))}
                    </ul>
                )}
            </div>

            {error && <FormMessage>{error}</FormMessage>}
        </FormItem>
    );
}
