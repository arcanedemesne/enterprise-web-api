import Box from "@mui/joy/Box";
import { Button, Tooltip } from "@mui/joy";
import CircularProgress from "@mui/joy/CircularProgress";
import FormControl from "@mui/joy/FormControl";
import FormLabel from "@mui/joy/FormLabel";
import IconButton from "@mui/joy/IconButton";
import Link from "@mui/joy/Link";
import Option from "@mui/joy/Option";
import Select from "@mui/joy/Select";
import Sheet from "@mui/joy/Sheet";
import Table from "@mui/joy/Table";
import Typography from "@mui/joy/Typography";

import ArrowDownwardIcon from "@mui/icons-material/ArrowDownward";
import KeyboardArrowLeftIcon from "@mui/icons-material/KeyboardArrowLeft";
import KeyboardArrowRightIcon from "@mui/icons-material/KeyboardArrowRight";

import { visuallyHidden } from "@mui/utils";

import createUniqueKey from "../utilities/uniqueKey";

const labelDisplayedRows = ({
  from,
  to,
  count,
}: {
  from: number;
  to: number;
  count: number;
}) => {
  return `${from}–${to} of ${count !== -1 ? count : `more than ${to}`}`;
};

const DataTable = ({
  title,
  caption,
  loading,
  data,
  pagination,
  setNewPaginationValues,
  canDeleteItems,
  handleDeleteItem,
  canEditItems,
  handleEditItem,
  canRestoreItems,
  handleRestoreItem,
  ...props
}: any) => {
  const handleChangeCurrentPage = async (currentPage: number) => {
    await setNewPaginationValues(
      currentPage,
      pagination.PageSize,
      pagination.OrderBy
    );
  };

  const handleChangePageSize = async (pageSize: number | null) => {
    await setNewPaginationValues(1, pageSize, pagination.OrderBy);
  };

  const handleChangeOrderBy = async (orderBy: string | null, order: string) => {
    const newOrderBy = orderBy + " " + order;
    await setNewPaginationValues(
      pagination.CurrentPage,
      pagination.PageSize,
      newOrderBy
    );
  };

  const handleRestoreSelectedRow = async (id: number) => {
    await handleRestoreItem(id);
  }

  const handleDeleteSelectedRow = async (id: number) => {
    await handleDeleteItem(id);
  };

  const handleEditSelectedRow = async (id: number) => {
    await handleEditItem(id);
  };

  const getLabelDisplayedRowsTo = () => {
    const to = pagination.CurrentPage * pagination.PageSize;
    return to > pagination.TotalItemCount ? pagination.TotalItemCount : to;
  };

  const EnhancedTableToolbar = () => {
    return (
      <Box
        sx={{
          display: "flex",
          alignItems: "center",
          py: 1,
          pl: { sm: 2 },
          pr: { xs: 1, sm: 1 },
          borderTopLeftRadius: "var(--unstable_actionRadius)",
          borderTopRightRadius: "var(--unstable_actionRadius)",
        }}
      >
        <Typography
          level="h6"
          sx={{ flex: "1 1 100%" }}
          id="tableTitle"
          component="span"
        >
          {title}
        </Typography>
      </Box>
    );
  };

  return (
    <Sheet
      variant="outlined"
      sx={{
        width: "100%",
        boxShadow: "sm",
        borderRadius: "sm",
        my: 3,
        "--Table-firstColumnWidth": "56px",
        "--Table-lastColumnWidth": "144px",
        overflow: "auto",
        backgroundRepeat: "no-repeat",
        backgroundAttachment: "local, local, scroll, scroll",
        backgroundPosition:
          "var(--Table-firstColumnWidth) var(--TableCell-height), calc(100% - var(--Table-lastColumnWidth)) var(--TableCell-height), var(--Table-firstColumnWidth) var(--TableCell-height), calc(100% - var(--Table-lastColumnWidth)) var(--TableCell-height)",
      }}
    >
      <EnhancedTableToolbar />
      <Table
        aria-label="data table"
        {...props}
        sx={{
          "& tr > *:first-of-type": {
            position: "sticky",
            left: 0,
            bgcolor: "background.surface",
          },
          "& tr > *:last-child": {
            position: "sticky",
            right: 0,
            bgcolor: "var(--TableCell-headBackground)",
          },
        }}
      >
        <caption>{caption}</caption>
        <thead>
          <tr>
            {data.headers.length > 0 &&
              data.headers.map((headCell: any, columnIndex: number) => {
                const firstOrderBy = pagination.OrderBy.includes(",")
                  ? pagination.OrderBy.split(",")[0]
                  : pagination.OrderBy;
                const orderBy: string = firstOrderBy.split(" ")[0];
                const order: string = firstOrderBy.split(" ")[1];
                const active = orderBy === headCell.id;
                return (
                  <th
                    style={{
                      width:
                        columnIndex === 0
                          ? "var(--Table-firstColumnWidth)"
                          : headCell.width ?? 150,
                    }}
                    key={createUniqueKey(10)}
                    aria-sort={
                      active
                        ? ({ asc: "ascending", desc: "descending" } as any)[
                            order
                          ]
                        : undefined
                    }
                  >
                    {/* eslint-disable-next-line jsx-a11y/anchor-is-valid */}
                    <Tooltip title={headCell.label} arrow placement="top">
                      {headCell.id ? (
                        <Link
                          underline="none"
                          color="neutral"
                          textColor={active ? "primary.plainColor" : undefined}
                          component="button"
                          onClick={async () => {
                            await handleChangeOrderBy(
                              headCell.id,
                              order === "asc" && orderBy === headCell.id
                                ? "desc"
                                : "asc"
                            );
                          }}
                          fontWeight="lg"
                          startDecorator={
                            headCell.numeric ? (
                              <ArrowDownwardIcon
                                sx={{ opacity: active ? 1 : 0 }}
                              />
                            ) : null
                          }
                          endDecorator={
                            !headCell.numeric ? (
                              <ArrowDownwardIcon
                                sx={{ opacity: active ? 1 : 0 }}
                              />
                            ) : null
                          }
                          sx={{
                            "& svg": {
                              transition: "0.2s",
                              transform:
                                active && order === "desc"
                                  ? "rotate(0deg)"
                                  : "rotate(180deg)",
                            },
                            "&:hover": { "& svg": { opacity: 1 } },
                          }}
                        >
                          {headCell.label}
                        </Link>
                      ) : (
                        <span>{columnIndex > 1 && headCell.label}</span>
                      )}
                    </Tooltip>
                    {active ? (
                      <Box component="span" sx={visuallyHidden}>
                        {order === "desc"
                          ? "sorted descending"
                          : "sorted ascending"}
                      </Box>
                    ) : null}
                  </th>
                );
              })}
            {(canEditItems || canDeleteItems || canRestoreItems) && (
              <th
                aria-label="last"
                style={{ width: "var(--Table-lastColumnWidth)" }}
              ></th>
            )}
          </tr>
        </thead>

        {loading ? (
          <tbody>
            <tr>
              <td
                colSpan={
                  data.headers.length + (canEditItems || canDeleteItems || canRestoreItems ? 1 : 0)
                }
                align="center"
                style={{ padding: 10 }}
              >
                <CircularProgress />
              </td>
            </tr>
          </tbody>
        ) : (
          <tbody>
            {data.rows.length > 0 &&
              data.rows.map((row: any, rowIndex: number) => {
                return (
                  <tr key={createUniqueKey(10)}>
                    {row.values.map((value: any, columnIndex: number) => {
                      const alignRight =
                        columnIndex === 0 || data.headers[columnIndex].numeric;
                      return (
                        <td
                          key={createUniqueKey(10)}
                          align={alignRight ? "right" : "left"}
                        >
                          {value}
                          {alignRight && <>&nbsp;&nbsp;</>}
                        </td>
                      );
                    })}
                    {(canEditItems || canDeleteItems || canRestoreItems) && (
                      <td align="right">
                        {canEditItems ? (
                          <Button
                            size="sm"
                            variant="plain"
                            color="neutral"
                            onClick={async () => handleEditSelectedRow(row.id)}
                          >
                            Edit
                          </Button>
                        ) : (
                          ""
                        )}
                        {canDeleteItems ? (
                          <Button
                            size="sm"
                            variant="soft"
                            color="danger"
                            onClick={async () =>
                              handleDeleteSelectedRow(row.id)
                            }
                          >
                            Delete
                          </Button>
                        ) : (
                          ""
                        )}
                        {canRestoreItems ? (
                          <Button
                            size="sm"
                            variant="soft"
                            color="info"
                            onClick={async () =>
                              handleRestoreSelectedRow(row.id)
                            }
                          >
                            Restore
                          </Button>
                        ) : (
                          ""
                        )}
                      </td>
                    )}
                  </tr>
                );
              })}
          </tbody>
        )}
        <tfoot>
          <tr>
            <td
              colSpan={
                canEditItems || canDeleteItems
                  ? data.headers.length - 2
                  : data.headers.length - 1
              }
            ></td>
            <td
              colSpan={3}
              style={{
                position: "sticky",
              }}
            >
              <Box
                component="span"
                sx={{
                  display: "flex",
                  alignItems: "center",
                  gap: 2,
                  justifyContent: "flex-end",
                }}
              >
                <FormControl
                  orientation="horizontal"
                  size="sm"
                  component="span"
                >
                  <FormLabel>rows per page:</FormLabel>
                  <Select
                    onChange={async (event: any, pageSize: number | null) => {
                      await handleChangePageSize(pageSize);
                    }}
                    value={pagination.PageSize}
                  >
                    <Option value={5}>5</Option>
                    <Option value={10}>10</Option>
                    <Option value={25}>25</Option>
                  </Select>
                </FormControl>
                <Typography
                  textAlign="center"
                  sx={{ minWidth: 80 }}
                  component="span"
                >
                  {labelDisplayedRows({
                    from:
                      data.rows.length === 0
                        ? 0
                        : (pagination.CurrentPage - 1) * pagination.PageSize +
                          1,
                    to: getLabelDisplayedRowsTo(),
                    count: pagination.TotalItemCount,
                  })}
                </Typography>
                <Box sx={{ display: "flex", gap: 1 }} component="span">
                  <IconButton
                    size="sm"
                    color="neutral"
                    variant="outlined"
                    disabled={pagination.CurrentPage === 1}
                    onClick={() =>
                      handleChangeCurrentPage(pagination.CurrentPage - 1)
                    }
                    sx={{ bgcolor: "background.surface" }}
                  >
                    <KeyboardArrowLeftIcon />
                  </IconButton>
                  <IconButton
                    size="sm"
                    color="neutral"
                    variant="outlined"
                    disabled={
                      pagination.CurrentPage * pagination.PageSize >=
                      pagination.TotalItemCount
                    }
                    onClick={() =>
                      handleChangeCurrentPage(pagination.CurrentPage + 1)
                    }
                    sx={{ bgcolor: "background.surface" }}
                  >
                    <KeyboardArrowRightIcon />
                  </IconButton>
                </Box>
              </Box>
            </td>
          </tr>
        </tfoot>
      </Table>
    </Sheet>
  );
};

export default DataTable;
