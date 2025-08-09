import * as React from "react";
import { useData } from "vike-react/useData";
import type { PageData } from "./+data";
import { PageWrapper } from "@/components/ui/PageWrapper";
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";
import { ConsultationForm } from "@/components/consultations/ConsultationForm";
import {
    updateConsultationDetailsAsync,
    cancelConsultationAsync,
} from "@/lib/api/consultations";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { CalendarX, ChevronLeft } from "lucide-react";
import { Link } from "@/components/Link";
import { toast } from "sonner";
import {
    AlertDialog,
    AlertDialogTrigger,
    AlertDialogContent,
    AlertDialogHeader,
    AlertDialogTitle,
    AlertDialogDescription,
    AlertDialogFooter,
    AlertDialogCancel,
    AlertDialogAction,
} from "@/components/ui/alert-dialog";
import { reload } from "vike/client/router";

const Page = () => {
    const { consultation, patients } = useData<PageData>();
    const isCancelled = Boolean(consultation.schedule.cancelledAt);
    const now = new Date();
    const end = new Date(consultation.schedule.end);
    const canCancel = !isCancelled && now < end;

    const [isCancelling, setIsCancelling] = React.useState(false);

    const cancel = async () => {
        try {
            setIsCancelling(true);
            await cancelConsultationAsync(consultation.id);
            toast.success("Consultation cancelled");
            await reload();
        } catch (err) {
            toast.error((err as Error).message ?? "Failed to cancel");
        } finally {
            setIsCancelling(false);
        }
    };

    return (
        <PageWrapper title="Edit Consultation">
            <div className="mx-auto max-w-3xl space-y-4">
                <div className="flex items-center justify-between">
                    <div className="flex items-center gap-3">
                        <Link
                            href="/consultations"
                            className="text-muted-foreground hover:text-foreground inline-flex items-center text-sm"
                        >
                            <ChevronLeft className="mr-1 h-4 w-4" />
                            Back
                        </Link>
                        <Badge
                            variant={isCancelled ? "destructive" : "secondary"}
                        >
                            {isCancelled ? "Cancelled" : "Scheduled"}
                        </Badge>
                    </div>

                    {canCancel && (
                        <AlertDialog>
                            <AlertDialogTrigger asChild>
                                <Button
                                    type="button"
                                    variant="destructive"
                                    size="sm"
                                    disabled={isCancelling}
                                    className="inline-flex items-center gap-1"
                                >
                                    <CalendarX className="h-4 w-4" />
                                    {isCancelling ? "Cancelling…" : "Cancel"}
                                </Button>
                            </AlertDialogTrigger>
                            <AlertDialogContent>
                                <AlertDialogHeader>
                                    <AlertDialogTitle>
                                        Cancel consultation?
                                    </AlertDialogTitle>
                                    <AlertDialogDescription>
                                        This action cannot be undone.
                                    </AlertDialogDescription>
                                </AlertDialogHeader>
                                <AlertDialogFooter>
                                    <AlertDialogCancel disabled={isCancelling}>
                                        Keep
                                    </AlertDialogCancel>
                                    <AlertDialogAction
                                        onClick={cancel}
                                        disabled={isCancelling}
                                    >
                                        {isCancelling
                                            ? "Cancelling…"
                                            : "Confirm Cancel"}
                                    </AlertDialogAction>
                                </AlertDialogFooter>
                            </AlertDialogContent>
                        </AlertDialog>
                    )}
                </div>

                <Card>
                    <CardHeader>
                        <CardTitle>Consultation</CardTitle>
                        <p className="text-muted-foreground mt-1">
                            Update the consultation details below.
                        </p>
                    </CardHeader>
                    <CardContent>
                        <ConsultationForm
                            initial={consultation}
                            patients={patients ?? []}
                            onSubmit={async (payload) => {
                                await updateConsultationDetailsAsync(
                                    consultation.id,
                                    {
                                        start: payload.start,
                                        durationInMinutes:
                                            payload.durationInMinutes,
                                        attachmentUris:
                                            payload.attachmentUris ?? [],
                                        notes: payload.notes ?? null,
                                    }
                                );
                            }}
                        />
                    </CardContent>
                </Card>
            </div>
        </PageWrapper>
    );
};

export default Page;
