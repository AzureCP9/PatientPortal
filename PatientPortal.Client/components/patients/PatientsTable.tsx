import * as React from "react";
import type { ColumnDef, SortingState } from "@tanstack/react-table";
import {
    flexRender,
    getCoreRowModel,
    getPaginationRowModel,
    getSortedRowModel,
    useReactTable,
} from "@tanstack/react-table";
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/Table/table";
import { Button } from "@/components/ui/button";
import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from "@/components/ui/select";
import { Badge } from "@/components/ui/badge";
import {
    ArrowUpDown,
    ChevronRight,
    ChevronsLeft,
    ChevronsRight,
} from "lucide-react";

import type { PatientResponseDto } from "@/lib/api/patients";
import { Link } from "../Link";
import {
    Tooltip,
    TooltipContent,
    TooltipProvider,
    TooltipTrigger,
} from "../ui/tooltip";

const fullName = (p: PatientResponseDto) =>
    `${p.name.firstName} ${p.name.lastName}`.trim();

// TODO: refactor columns and other components into smaller components (leaving it for this demo)
const columns: ColumnDef<PatientResponseDto>[] = [
    {
        accessorKey: "id",
        header: "ID",
        cell: ({ getValue }) => (
            <span className="text-muted-foreground font-mono text-xs">
                {getValue<string>().slice(0, 8)}
            </span>
        ),
    },
    {
        id: "name",
        accessorFn: (p) => fullName(p),
        header: ({ column }) => (
            <Button
                variant="ghost"
                size="sm"
                className="px-2"
                onClick={column.getToggleSortingHandler()}
            >
                Name
                <ArrowUpDown className="ml-1 h-3.5 w-3.5" />
            </Button>
        ),
    },
    {
        accessorKey: "gender",
        header: "Gender",
        cell: ({ getValue }) => (
            <Badge variant="outline" className="font-normal">
                {getValue<string>() ?? "—"}
            </Badge>
        ),
    },
    {
        accessorKey: "age",
        header: ({ column }) => (
            <Button
                variant="ghost"
                size="sm"
                className="px-2"
                onClick={column.getToggleSortingHandler()}
            >
                Age
                <ArrowUpDown className="ml-1 h-3.5 w-3.5" />
            </Button>
        ),
        cell: ({ getValue }) => (
            <span className="tabular-nums">{getValue<string>() ?? "—"}</span>
        ),
    },
    {
        id: "actions",
        header: "Actions",
        enableSorting: false,
        cell: ({ row }) => {
            const p = row.original;
            return (
                <TooltipProvider delayDuration={200}>
                    <Tooltip>
                        <TooltipTrigger asChild>
                            <Button
                                asChild
                                variant="default"
                                size="sm"
                                className="h-8 px-2"
                                aria-label={`View patient ${p.name.firstName}`}
                            >
                                <Link href={`/patients/${p.id}`}>
                                    <ChevronRight className="text-secondary h-4 w-4" />
                                </Link>
                            </Button>
                        </TooltipTrigger>
                        <TooltipContent>View Patient</TooltipContent>
                    </Tooltip>
                </TooltipProvider>
            );
        },
    },
];

type Props = { data: PatientResponseDto[] };

export const PatientsTable: React.FC<Props> = ({ data }) => {
    const [sorting, setSorting] = React.useState<SortingState>([]);

    const table = useReactTable({
        data,
        columns,
        state: { sorting },
        onSortingChange: setSorting,
        getCoreRowModel: getCoreRowModel(),
        getSortedRowModel: getSortedRowModel(),
        getPaginationRowModel: getPaginationRowModel(),
        initialState: {
            pagination: { pageIndex: 0, pageSize: 10 },
            sorting: [{ id: "name", desc: false }],
        },
    });

    return (
        <div className="flex w-full flex-col space-y-4 p-4">
            {/* Top-right page size selector */}
            <div className="flex items-center gap-2 self-end">
                <span className="text-muted-foreground text-sm">Rows</span>
                <Select
                    value={String(table.getState().pagination.pageSize)}
                    onValueChange={(v) => table.setPageSize(Number(v))}
                >
                    <SelectTrigger className="h-9 w-[84px]">
                        <SelectValue />
                    </SelectTrigger>
                    <SelectContent>
                        {[5, 10, 20, 50].map((n) => (
                            <SelectItem key={n} value={String(n)}>
                                {n}
                            </SelectItem>
                        ))}
                    </SelectContent>
                </Select>
            </div>

            {/* Table */}
            <div className="rounded-md border">
                <div className="w-full overflow-x-auto">
                    <Table>
                        <TableHeader>
                            {table.getHeaderGroups().map((hg) => (
                                <TableRow key={hg.id}>
                                    {hg.headers.map((header) => (
                                        <TableHead
                                            key={header.id}
                                            className={
                                                header.column.getCanSort()
                                                    ? "cursor-pointer select-none"
                                                    : ""
                                            }
                                        >
                                            {flexRender(
                                                header.column.columnDef.header,
                                                header.getContext()
                                            )}
                                        </TableHead>
                                    ))}
                                </TableRow>
                            ))}
                        </TableHeader>

                        <TableBody>
                            {table.getRowModel().rows.length ? (
                                table.getRowModel().rows.map((row) => (
                                    <TableRow
                                        key={row.id}
                                        className="hover:bg-muted/40"
                                    >
                                        {row.getVisibleCells().map((cell) => (
                                            <TableCell
                                                key={cell.id}
                                                className="whitespace-nowrap"
                                            >
                                                {flexRender(
                                                    cell.column.columnDef.cell,
                                                    cell.getContext()
                                                )}
                                            </TableCell>
                                        ))}
                                    </TableRow>
                                ))
                            ) : (
                                <TableRow>
                                    <TableCell
                                        colSpan={columns.length}
                                        className="text-muted-foreground h-24 text-center"
                                    >
                                        No results.
                                    </TableCell>
                                </TableRow>
                            )}
                        </TableBody>
                    </Table>
                </div>
            </div>

            {/* Pager */}
            <div className="flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
                <div className="text-muted-foreground text-sm">
                    Page {table.getState().pagination.pageIndex + 1} of{" "}
                    {table.getPageCount() || 1}
                </div>
                <div className="flex gap-2">
                    <Button
                        variant="outline"
                        size="sm"
                        className="h-9"
                        onClick={() => table.setPageIndex(0)}
                        disabled={!table.getCanPreviousPage()}
                    >
                        <ChevronsLeft className="mr-1 h-4 w-4" />
                        First
                    </Button>
                    <Button
                        variant="outline"
                        size="sm"
                        className="h-9"
                        onClick={() => table.previousPage()}
                        disabled={!table.getCanPreviousPage()}
                    >
                        Prev
                    </Button>
                    <Button
                        variant="outline"
                        size="sm"
                        className="h-9"
                        onClick={() => table.nextPage()}
                        disabled={!table.getCanNextPage()}
                    >
                        Next
                    </Button>
                    <Button
                        variant="outline"
                        size="sm"
                        className="h-9"
                        onClick={() =>
                            table.setPageIndex(table.getPageCount() - 1)
                        }
                        disabled={!table.getCanNextPage()}
                    >
                        Last
                        <ChevronsRight className="ml-1 h-4 w-4" />
                    </Button>
                </div>
            </div>
        </div>
    );
};
