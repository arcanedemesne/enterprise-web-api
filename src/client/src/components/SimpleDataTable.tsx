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
  ...props
}: any) => {
  const setCurrentPage = (pageNumber: number) => {
    console.info("changing to page # " + pageNumber);
  };

  const setPageSize = (pageSize: number) => {
    console.info("changing page size to " + pageSize);
  };

  const handleChangeCurrentPage = (pageNumber: number) => {
    setCurrentPage(pageNumber);
  };

  const handleChangePageSize = (event: any, newValue: number | null) => {
    setPageSize(parseInt(newValue!.toString(), 10));
    setCurrentPage(0);
  };

  const getLabelDisplayedRowsTo = () => {
    if (data.rows.length === -1) {
      return (pagination.CurrentPage + 1) * pagination.PageSize;
    }
    return pagination.PageSize === -1
      ? data.rows.length
      : Math.min(
          data.rows.length,
          (pagination.CurrentPage + 1) * pagination.PageSize
        );
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
      {data.rows.length === 0 ? (
        <CircularProgress />
      ) : (
        <Table aria-label="basic table" {...props}>
          <caption>{caption}</caption>
          <thead>
            <tr>
              {data.headers.length > 0 &&
                data.headers.map((h: any) => (
                  <th
                    style={{ width: h.width ?? "auto" }}
                    key={createUniqueKey(10)}
                  >
                    {h.label}
                  </th>
                ))}
            </tr>
          </thead>
          <tbody>
            {data.rows.length > 0 &&
              data.rows
                .slice(
                  (pagination.CurrentPage - 1) * pagination.PageSize,
                  (pagination.CurrentPage - 1) * pagination.PageSize +
                    pagination.PageSize
                )
                .map((row: any) => (
                  <tr key={createUniqueKey(10)}>
                    {row.values.map((value: any) => (
                      <td key={createUniqueKey(10)}>{value}</td>
                    ))}
                  </tr>
                ))}
          </tbody>
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
                      onChange={handleChangePageSize}
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
                      count: data.rows.length === -1 ? -1 : data.rows.length,
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
                        data.rows.length !== -1
                          ? (pagination.CurrentPage - 1) >=
                            Math.ceil(data.rows.length / pagination.PageSize) -
                              1
                          : false
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
      )}
    </Sheet>
  );
};

export default SimpleDataTable;
