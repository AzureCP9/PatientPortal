export const mimeInfo = [
    { mime: "image/jpeg", label: "JPEG", extensions: [".jpg", ".jpeg"] },
    { mime: "image/png", label: "PNG", extensions: [".png"] },
    { mime: "application/pdf", label: "PDF", extensions: [".pdf"] },
    { mime: "application/msword", label: "Word (DOC)", extensions: [".doc"] },
    {
        mime: "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        label: "Word (DOCX)",
        extensions: [".docx"],
    },
] as const;

export type MimeInfo = (typeof mimeInfo)[number];
export type MimeType = MimeInfo["mime"];
export type MimeLabel = MimeInfo["label"];
export type MimeExtension = MimeInfo["extensions"][number];

export const mimeLabelMap: Record<MimeLabel, MimeInfo> = Object.fromEntries(
    mimeInfo.map((info) => [info.label, info])
) as Record<MimeLabel, MimeInfo>;

export const mimeTypeMap: Record<MimeType, MimeInfo> = Object.fromEntries(
    mimeInfo.map((info) => [info.mime, info])
) as Record<MimeType, MimeInfo>;

export const mimeExtensionMap: Record<MimeExtension, MimeInfo> =
    Object.fromEntries(
        mimeInfo.flatMap((info) => info.extensions.map((ext) => [ext, info]))
    ) as Record<MimeExtension, MimeInfo>;

export const getMimeInfo = {
    fromLabel: (value: MimeLabel) => mimeLabelMap[value],
    fromLabels: (values: MimeLabel[]) =>
        Array.from(new Set(values)).map((label) => mimeLabelMap[label]),

    fromType: (value: MimeType) => mimeTypeMap[value],
    fromTypes: (values: MimeType[]) =>
        Array.from(new Set(values)).map((type) => mimeTypeMap[type]),

    fromExtension: (value: MimeExtension) => mimeExtensionMap[value],
    fromExtensions: (values: MimeExtension[]) =>
        Array.from(new Set(values)).map((ext) => mimeExtensionMap[ext]),
};
