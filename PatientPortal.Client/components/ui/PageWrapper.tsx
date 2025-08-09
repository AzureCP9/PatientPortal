import React, { ReactNode } from "react";
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";

type PageWrapperProps = {
    title: string;
    children: ReactNode;
};

export const PageWrapper: React.FC<PageWrapperProps> = ({
    title,
    children,
}) => {
    return (
        <Card className="p-10">
            <CardHeader className="p-0">
                <CardTitle className="h1 text-xl">{title}</CardTitle>
            </CardHeader>
            <CardContent className="p-0">{children}</CardContent>
        </Card>
    );
};
