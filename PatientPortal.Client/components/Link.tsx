import { usePageContext } from "vike-react/usePageContext";
import { cn } from "@/lib/utils";

interface LinkProps extends React.AnchorHTMLAttributes<HTMLAnchorElement> {
    href: string;
    children: React.ReactNode;
}

export function Link({ href, children, className, ...props }: LinkProps) {
    const { urlPathname } = usePageContext();
    const isActive =
        href === "/" ? urlPathname === href : urlPathname.startsWith(href);

    return (
        <a
            href={href}
            className={cn(
                "text-muted-foreground hover:text-primary transition-colors",
                isActive && "text-primary font-medium",
                className
            )}
            {...props}
        >
            {children}
        </a>
    );
}
