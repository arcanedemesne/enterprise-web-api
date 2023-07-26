import CircularProgress from "@mui/joy/CircularProgress";
import Sheet from "@mui/joy/Sheet";
import Table from "@mui/joy/Table";
import Typography from "@mui/joy/Typography";
import Box from "@mui/joy/Box";
import Link from "@mui/joy/Link";
import FormControl from "@mui/joy/FormControl";
import FormLabel from "@mui/joy/FormLabel";
import IconButton from "@mui/joy/IconButton";
import Select from "@mui/joy/Select";
import Option from "@mui/joy/Option";
import KeyboardArrowLeftIcon from "@mui/icons-material/KeyboardArrowLeft";
import KeyboardArrowRightIcon from "@mui/icons-material/KeyboardArrowRight";
import ArrowDownwardIcon from "@mui/icons-material/ArrowDownward";
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

const SimpleDataTable = ({
  title,
  caption,
  data,
  pagination,
  setNewPaginationValues,
  ...props
}: any) => {
  const handleChangeCurrentPage = async (currentPage: number) => {
    await setNewPaginationValues(currentPage, pagination.PageSize, pagination.OrderBy);
  };

  const handleChangePageSize = async (pageSize: number | null) => {
    await setNewPaginationValues(1, pageSize, pagination.OrderBy);
  };

  const handleChangeOrderBy = async (orderBy: string | null, order: string) => {
    const newOrderBy = orderBy + " " + order;
    await setNewPaginationValues(pagination.CurrentPage, pagination.PageSize, newOrderBy);
  };

  const getLabelDisplayedRowsTo = () => {
    const to = pagination.CurrentPage * pagination.PageSize;
    return to > pagination.TotalItemCount ? pagination.TotalItemCount : to;
  };

  return (
    <Sheet
      variant="outlined"
      sx={{ width: "100%", boxShadow: "sm", borderRadius: "sm", my: 3 }}
    >
      <Box
        sx={{
          display: "flex",
          alignItems: "center",
          py: 1,
          pl: { sm: 2 },
          pr: { xs: 1, sm: 1 },
          // ...(numSelected > 0 && {
          //   bgcolor: "background.level1",
          // }),
          borderTopLeftRadius: "var(--unstable_actionRadius)",
          borderTopRightRadius: "var(--unstable_actionRadius)",
        }}
      >
        <Typography level="h6" component="div">
          {title}
        </Typography>
      </Box>
      <Table aria-label="basic table" {...props}>
        <caption>{caption}</caption>
        <thead>
          <tr>
            {data.headers.length > 0 &&
              data.headers.map((headCell: any) => {
                const firstOrderBy = pagination.OrderBy
                  ? pagination.OrderBy.split(",")[0]
                  : " ";
                const orderBy: string = firstOrderBy.split(" ")[0];
                const order: string = firstOrderBy.split(" ")[1];
                const active = orderBy === headCell.id;
                return (
                  <th
                    style={{ width: headCell.width ?? "auto" }}
                    key={headCell.id ?? createUniqueKey(10)}
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
                          <ArrowDownwardIcon sx={{ opacity: active ? 1 : 0 }} />
                        ) : null
                      }
                      endDecorator={
                        !headCell.numeric ? (
                          <ArrowDownwardIcon sx={{ opacity: active ? 1 : 0 }} />
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
                    </Link>) : (<>{headCell.label}</>)}

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

        {data.rows.length === 0 ? (
          <tbody>
            <tr>
              <td
                colSpan={data.headers.length}
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
              data.rows.map((row: any) => (
                <tr key={createUniqueKey(10)}>
                  {row.values.map((value: any) => (
                    <td key={createUniqueKey(10)}>{value}</td>
                  ))}
                </tr>
              ))}
          </tbody>
        )}
        <tfoot>
          <tr>
            <td colSpan={data.headers.length}>
              <Box
                sx={{
                  display: "flex",
                  alignItems: "center",
                  gap: 2,
                  justifyContent: "flex-end",
                }}
              >
                <FormControl orientation="horizontal" size="sm">
                  <FormLabel>Rows per page:</FormLabel>
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
                <Typography textAlign="center" sx={{ minWidth: 80 }}>
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
                <Box sx={{ display: "flex", gap: 1 }}>
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

export default SimpleDataTable;
