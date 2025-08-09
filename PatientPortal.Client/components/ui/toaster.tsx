import { CircleCheckBig, CircleX } from "lucide-react";
import { Toaster as Sonner } from "sonner";

const Toaster = ({ ...props }) => {
    return (
        <Sonner
            theme={"light"}
            className="toaster group"
            toastOptions={{
                classNames: {
                    toast: "group toast group-[.toaster]:bg-background group-[.toaster]:text-foreground group-[.toaster]:border-border group-[.toaster]:shadow-lg",
                    title: "ml-2",
                    description: "group-[.toast]:text-muted-foreground ml-2",
                    actionButton:
                        "group-[.toast]:bg-primary group-[.toast]:text-primary-foreground",
                    cancelButton:
                        "group-[.toast]:bg-muted group-[.toast]:text-muted-foreground",
                },
            }}
            icons={{
                success: <CircleCheckBig className="stroke-primary h-6 w-6" />,
                error: <CircleX className="stroke-destructive h-6 w-6" />,
            }}
            {...props}
        />
    );
};

export { Toaster };
