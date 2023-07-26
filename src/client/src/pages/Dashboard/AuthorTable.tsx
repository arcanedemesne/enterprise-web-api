import { useEffect, useState } from "react";

import { GET } from "../../utilities/httpRequest";
import DataTable from "../../components/DataTable";

let initialized = false;
const AuthorTable = () => {
  const [apiData, setData] = useState([]);
  const [pagination, setPagination] = useState({});
  const [selectedRows, setSelectedRows] = useState([]);

  const baseUri = "authors?includeBooks=true";
  const setNewPaginationValues = async (
    pageNumber: number,
    pageSize: number,
    orderBy: string
  ) => {
    const response: any = await GET({
      endpoint: `${baseUri}&PageNumber=${pageNumber}&PageSize=${pageSize}&OrderBy=${orderBy}`,
    });
    const { headers, data } = response;
    const paginationHeaders = JSON.parse(headers.get("x-pagination"));
    setPagination(paginationHeaders);
    setData(data);
  };

  useEffect(() => {
    async function getData() {
      const response: any = await GET({
        endpoint: baseUri,
      });
      const { headers, data } = response;
      const paginationHeaders = JSON.parse(headers.get("x-pagination"));
      setPagination(paginationHeaders);
      setData(data);
    }

    if (!initialized) {
      getData();
      initialized = true;
    }
  }, []);

  const rows = apiData.map((x: any) => {
    return {
      id: x.id,
      values: [x.id, x.firstName, x.lastName, x.books.length],
    };
  });
  const tableData = {
    headers: [
      {
        id: "id",
        width: 100,
        label: "Id",
        numeric: true,
      },
      {
        id: "firstName",
        label: "First Name",
        numeric: false,
      },
      {
        id: "lastName",
        label: "last Name",
        numeric: false,
      },
      {
        label: "Book Count",
        numeric: true,
      },
    ],
    rows,
  };

  return (
    <DataTable
      title="Authors"
      caption="This table contains Authors"
      data={tableData}
      pagination={pagination}
      setNewPaginationValues={setNewPaginationValues}
      canDeleteItems
      canEditItems
      selectedRows={selectedRows}
      setSelectedRows={setSelectedRows}
      borderAxis="xBetween"
      hoverRow
    />
  );
};

export default AuthorTable;
