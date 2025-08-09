import { ArrowUp, ArrowDown, ArrowUpDown } from "lucide-react";

const SortIcon: React.FC<{ column: any }> = ({ column }) => {
    const dir = column.getIsSorted();
    if (dir === "asc") return <ArrowUp className="ml-1 h-3.5 w-3.5" />;
    if (dir === "desc") return <ArrowDown className="ml-1 h-3.5 w-3.5" />;
    return <ArrowUpDown className="text-muted-foreground ml-1 h-3.5 w-3.5" />;
};

export { SortIcon as TableSortIcon };
