import Box from "@mui/joy/Box";
import Checkbox from "@mui/joy/Checkbox";
import FormControl from "@mui/joy/FormControl";
import FormLabel from "@mui/joy/FormLabel";
import IconButton from "@mui/joy/IconButton";
import Link from "@mui/joy/Link";
import Option from "@mui/joy/Option";
import Select from "@mui/joy/Select";
import Sheet from "@mui/joy/Sheet";
import Table from "@mui/joy/Table";
import Tooltip from "@mui/joy/Tooltip";
import Typography from "@mui/joy/Typography";

import ArrowDownwardIcon from "@mui/icons-material/ArrowDownward";
import DeleteIcon from "@mui/icons-material/Delete";
import FilterListIcon from "@mui/icons-material/FilterList";
import KeyboardArrowLeftIcon from "@mui/icons-material/KeyboardArrowLeft";
import KeyboardArrowRightIcon from "@mui/icons-material/KeyboardArrowRight";

import { visuallyHidden } from "@mui/utils";

import createUniqueKey from "../utilities/uniqueKey";
import CircularProgress from "@mui/joy/CircularProgress";

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
  handleDeleteItems,
  canEditItems,
  handleEditItem,
  canFilterItems,
  selectedRows,
  setSelectedRows,
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

  const handleRowSelected = (id: number) => {
    const newSelectedRows = [...selectedRows];
    if (!selectedRows.includes(id)) {
      newSelectedRows.push(id);
    } else {
      const index = selectedRows.indexOf(id);
      newSelectedRows.splice(index, 1);
    }
    setSelectedRows(newSelectedRows);
  };

  const handleDeleteSelectedRows = async () => {
    await handleDeleteItems();
  };

  const handleEditSelectedRow = async (id: number) => {
    await handleEditItem(id);
  };

  const handleFilterItems = async () => {
    console.info(`filter items`);
  };

  const getLabelDisplayedRowsTo = () => {
    const to = pagination.CurrentPage * pagination.PageSize;
    return to > pagination.TotalItemCount ? pagination.TotalItemCount : to;
  };

  const EnhancedTableToolbar = ({ numSelected }: { numSelected: number }) => {
    return (
      <Box
        sx={{
          display: "flex",
          alignItems: "center",
          py: 1,
          pl: { sm: 2 },
          pr: { xs: 1, sm: 1 },
          ...(numSelected > 0 && {
            bgcolor: "background.level1",
          }),
          borderTopLeftRadius: "var(--unstable_actionRadius)",
          borderTopRightRadius: "var(--unstable_actionRadius)",
        }}
      >
        {numSelected > 0 ? (
          <Typography sx={{ flex: "1 1 100%" }} component="span">
            {numSelected} selected
          </Typography>
        ) : (
          <Typography
            level="h6"
            sx={{ flex: "1 1 100%" }}
            id="tableTitle"
            component="span"
          >
            {title}
          </Typography>
        )}

        {numSelected && canDeleteItems > 0 ? (
          <Tooltip title="Delete">
            <IconButton
              size="sm"
              color="danger"
              variant="solid"
              onClick={async () => await handleDeleteSelectedRows()}
            >
              <DeleteIcon />
            </IconButton>
          </Tooltip>
        ) : canFilterItems ? (
          <>
            <Tooltip title="Filter list">
              <IconButton
                size="sm"
                variant="outlined"
                color="neutral"
                onClick={async () => await handleFilterItems()}
              >
                <FilterListIcon />
              </IconButton>
            </Tooltip>
          </>
        ) : null}
      </Box>
    );
  };

  return (
    <Sheet
      variant="outlined"
      sx={{ width: "100%", boxShadow: "sm", borderRadius: "sm", my: 3 }}
    >
      <EnhancedTableToolbar numSelected={selectedRows.length} />
      <Table aria-label="basic table" {...props}>
        <caption>{caption}</caption>
        <thead>
          <tr>
            {canDeleteItems && <th style={{ width: 50 }}>&nbsp;</th>}
            {data.headers.length > 0 &&
              data.headers.map((headCell: any) => {
                const firstOrderBy = pagination.OrderBy.includes(",")
                  ? pagination.OrderBy.split(",")[0]
                  : pagination.OrderBy;
                const orderBy: string = firstOrderBy.split(" ")[0];
                const order: string = firstOrderBy.split(" ")[1];
                const active = orderBy === headCell.id;
                return (
                  <th
                    style={{ width: headCell.width ?? "auto" }}
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
                      <>{headCell.label}</>
                    )}

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
          </tr>
        </thead>

        {loading ? (
          <tbody>
            <tr>
              <td
                colSpan={data.headers.length + 1}
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
              const isItemSelected = selectedRows.indexOf(row.id) !== -1;
              const labelId = `enhanced-table-checkbox-${rowIndex}`;

              return (
                <tr key={createUniqueKey(10)}>
                  {canDeleteItems && (
                    <th scope="row" key={createUniqueKey(10)}>
                      <Checkbox
                        checked={isItemSelected}
                        slotProps={{
                          input: {
                            "aria-labelledby": labelId,
                          },
                        }}
                        sx={{ verticalAlign: "top" }}
                        onClick={() => handleRowSelected(row.id)}
                      />
                    </th>
                  )}
                  {row.values.map((value: any, columnIndex: number) => (
                    <td
                      key={createUniqueKey(10)}
                      onClick={async () => {
                        if (columnIndex === 0 && canEditItems)
                          handleEditSelectedRow(row.id);
                      }}
                    >
                      {columnIndex === 0 && canEditItems ? (
                        <>
                          <u
                            style={{
                              cursor:
                                columnIndex === 0 && canEditItems
                                  ? "pointer"
                                  : "auto",
                              color:
                                columnIndex === 0 && canEditItems
                                  ? "#64b5f6"
                                  : "auto",
                            }}
                          >
                            edit
                          </u>{" "}
                          |{" "}
                        </>
                      ) : (
                        ""
                      )}
                      {value}{" "}
                    </td>
                  ))}
                </tr>
              );
            })}
        </tbody>
        )}
        <tfoot>
          <tr>
            <td
              colSpan={
                canDeleteItems ? data.headers.length + 1 : data.headers.length
              }
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
