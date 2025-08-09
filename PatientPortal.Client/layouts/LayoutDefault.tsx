import "./style.css";
import "./tailwind.css";
import { Link } from "../components/Link.js";
import { Toaster } from "@/components/ui/toaster";
import { Calendar, Users } from "lucide-react";

const LayoutDefault = ({ children }: { children: React.ReactNode }) => {
    return (
        <div className="m-auto flex">
            <Sidebar>
                <h1 className="text-2xl font-bold">Patient Portal</h1>
                <div className="mt-6 flex flex-col gap-2">
                    <Link
                        className="flex items-center gap-2 rounded p-4 text-lg"
                        href="/consultations"
                    >
                        <Calendar className="h-5 w-5" />
                        Consultations
                    </Link>
                    <Link
                        className="flex items-center gap-2 rounded p-4 text-lg"
                        href="/patients"
                    >
                        <Users className="h-5 w-5" />
                        Patients
                    </Link>
                </div>
            </Sidebar>
            <Content>{children}</Content>
            <Toaster position="bottom-right" />
        </div>
    );
};

const Sidebar = ({ children }: { children: React.ReactNode }) => {
    return (
        <aside
            id="sidebar"
            className="flex shrink-0 flex-col border-r border-r-1 border-gray-200 px-8 py-5"
        >
            {children}
        </aside>
    );
};

const Content = ({ children }: { children: React.ReactNode }) => {
    return (
        <div id="page-container" className="w-full">
            <div id="page-content" className="min-h-screen w-full p-5 pb-12">
                {children}
            </div>
        </div>
    );
};

export default LayoutDefault;
