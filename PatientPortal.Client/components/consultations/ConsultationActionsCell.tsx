import * as React from "react";
import {
    cancelConsultationAsync,
    ConsultationResponseDto,
} from "@/lib/api/consultations";
import { CalendarX, ChevronRight } from "lucide-react";
import { Button } from "../ui/button";
import { Link } from "../Link";
import {
    Tooltip,
    TooltipContent,
    TooltipProvider,
    TooltipTrigger,
} from "@/components/ui/tooltip";
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
import { toast } from "sonner";
import { reload } from 'vike/client/router'

const ConsultationActionsCell: React.FC<{ c: ConsultationResponseDto }> = ({
    c,
}) => {
    const [isCancelling, setIsCancelling] = React.useState(false);

    const now = new Date();
    const end = new Date(c.schedule.end);
    const isCancelled = Boolean(c.schedule.cancelledAt);
    const canCancel = !isCancelled && now < end;

    const doCancel = async () => {
        try {
            setIsCancelling(true);
            await cancelConsultationAsync(c.id);
            toast.success("Consultation cancelled");
            await reload();
        } catch (err) {
            toast.error((err as Error).message ?? "Failed to cancel");
        } finally {
            setIsCancelling(false);
        }
    };

    return (
        <TooltipProvider delayDuration={200}>
            <div className="flex items-center gap-2">
                <Tooltip>
                    <TooltipTrigger asChild>
                        <Button
                            asChild
                            variant="default"
                            size="icon"
                            className="h-9 w-9"
                            aria-label={`View consultation ${c.id}`}
                        >
                            <Link href={`/consultations/${c.id}`}>
                                <ChevronRight className="text-secondary h-4 w-4" />
                            </Link>
                        </Button>
                    </TooltipTrigger>
                    <TooltipContent>View consultation</TooltipContent>
                </Tooltip>

                {canCancel && (
                    <AlertDialog>
                        <Tooltip>
                            <TooltipTrigger asChild>
                                <AlertDialogTrigger asChild>
                                    <Button
                                        variant="destructive"
                                        size="icon"
                                        className="h-9 w-9"
                                        disabled={isCancelling}
                                        aria-label={`Cancel consultation ${c.id}`}
                                    >
                                        <CalendarX className="h-4 w-4" />
                                    </Button>
                                </AlertDialogTrigger>
                            </TooltipTrigger>
                            <TooltipContent>Cancel consultation</TooltipContent>
                        </Tooltip>

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
                                    onClick={doCancel}
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
        </TooltipProvider>
    );
};

export { ConsultationActionsCell };
