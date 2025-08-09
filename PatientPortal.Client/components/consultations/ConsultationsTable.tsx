import * as React from "react";
import type { ColumnDef } from "@tanstack/react-table";
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
import { ChevronsLeft, ChevronsRight } from "lucide-react";
import type { ConsultationResponseDto } from "@/lib/api/consultations";
import { format } from "date-fns";
import { ConsultationActionsCell } from "./ConsultationActionsCell";
import { TableSortIcon } from "../ui/Table/TableSortIcon";

const columns: ColumnDef<ConsultationResponseDto>[] = [
    {
        accessorKey: "id",
        header: "Id",
        cell: ({ getValue }) => (
            <span className="text-muted-foreground font-mono text-xs">
                {getValue<string>().slice(0, 8)}
            </span>
        ),
    },
    {
        accessorKey: "patientName",
        header: "Patient Name",
        cell: ({ getValue }) => (
            <span className="text-muted-foreground font-mono text-xs">
                {getValue<string>()}
            </span>
        ),
    },
    {
        id: "schedule.start",
        accessorFn: (r) => new Date(r.schedule.start).getTime(),
        header: ({ column }) => (
            <Button
                variant="ghost"
                size="sm"
                className="px-2"
                onClick={column.getToggleSortingHandler()}
            >
                Start
                <TableSortIcon column={column} />
            </Button>
        ),
        cell: ({ row }) =>
            format(new Date(row.original.schedule.start), "yyyy-MM-dd HH:mm"),
    },
    {
        id: "schedule.end",
        accessorFn: (r) => new Date(r.schedule.end).getTime(),
        header: ({ column }) => (
            <Button
                variant="ghost"
                size="sm"
                className="px-2"
                onClick={column.getToggleSortingHandler()}
            >
                End
                <TableSortIcon column={column} />
            </Button>
        ),
        cell: ({ row }) =>
            format(new Date(row.original.schedule.end), "yyyy-MM-dd HH:mm"),
    },
    {
        accessorKey: "schedule.durationInMinutes",
        header: "Duration (min)",
        cell: ({ getValue }) => {
            const mins = Number(getValue() ?? 0);
            return <span className="tabular-nums">{mins}</span>;
        },
    },
    {
        id: "status",
        header: "Status",
        cell: ({ row }) => {
            const { schedule } = row.original;
            const now = new Date();

            if (schedule.cancelledAt)
                return <Badge variant="destructive">Cancelled</Badge>;

            const start = new Date(schedule.start);
            const end = new Date(schedule.end);

            if (now < start)
                return <Badge variant="secondary">Scheduled</Badge>;
            if (now >= start && now < end)
                return <Badge variant="default">Ongoing</Badge>;

            return <Badge variant="outline">Completed</Badge>;
        },
    },
    {
        id: "actions",
        header: "Actions",
        enableSorting: false,
        cell: ({ row }) => <ConsultationActionsCell c={row.original} />,
    },
];

type Props = { data: ConsultationResponseDto[] };

export const ConsultationsTable: React.FC<Props> = ({ data }) => {
    const table = useReactTable({
        data,
        columns,
        getCoreRowModel: getCoreRowModel(),
        getSortedRowModel: getSortedRowModel(),
        getPaginationRowModel: getPaginationRowModel(),
        initialState: {
            pagination: { pageIndex: 0, pageSize: 10 },
            sorting: [{ id: "schedule.start", desc: true }],
        },
    });

    return (
        <div className="flex w-full flex-col space-y-4 p-4">
            {/* Page size selector */}
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
                                            aria-sort={
                                                header.column.getIsSorted() ===
                                                "asc"
                                                    ? "ascending"
                                                    : header.column.getIsSorted() ===
                                                        "desc"
                                                      ? "descending"
                                                      : "none"
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

            {/* Pagination controls */}
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
