import CircularProgress from "@mui/joy/CircularProgress";
import Sheet from "@mui/joy/Sheet";
import Table from "@mui/joy/Table";
import Typography from "@mui/joy/Typography";
import Box from "@mui/joy/Box";
import FormControl from "@mui/joy/FormControl";
import FormLabel from "@mui/joy/FormLabel";
import IconButton from "@mui/joy/IconButton";
import Select from "@mui/joy/Select";
import Option from "@mui/joy/Option";
import KeyboardArrowLeftIcon from "@mui/icons-material/KeyboardArrowLeft";
import KeyboardArrowRightIcon from "@mui/icons-material/KeyboardArrowRight";

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
  setCurrentPageNumberAndPageSize,
  ...props
}: any) => {
  const handleChangeCurrentPage = async (currentPage: number) => {
    await setCurrentPageNumberAndPageSize(currentPage, pagination.PageSize);
  };

  const handleChangePageSize = async (pageSize: number | null) => {
    await setCurrentPageNumberAndPageSize(1, pageSize);
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
              data.headers.map((h: any) => {
                return (
                  <th
                    style={{ width: h.width ?? "auto" }}
                    key={createUniqueKey(10)}
                  >
                    {h.label}
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
