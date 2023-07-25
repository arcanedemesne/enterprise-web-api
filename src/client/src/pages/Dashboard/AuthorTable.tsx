import { useEffect, useState } from "react";

import { GET } from "../../utilities/httpRequest";
import SimpleDataTable from "../../components/SimpleDataTable";

let initialized = false;
const AuthorTable = () => {
  const [authors, setAuthors] = useState([]);
  const [pagination, setPagination] = useState({});

  const setCurrentPageNumberAndPageSize = async (pageNumber: number, pageSize: number) => {
    const response: any = await GET({
      endpoint: `authors?includeBooks=true&PageNumber=${pageNumber}&PageSize=${pageSize}`,
    });
    const { headers, data } = response;
    const paginationHeaders = JSON.parse(headers.get("x-pagination"));
    setPagination(paginationHeaders);
    setAuthors(data);
  }

  useEffect(() => {
    async function getAuthors() {
      const response: any = await GET({
        endpoint: "authors?includeBooks=true",
      });
      const { headers, data } = response;
      const paginationHeaders = JSON.parse(headers.get("x-pagination"));
      setPagination(paginationHeaders);
      setAuthors(data);
    }

    if (!initialized) {
      getAuthors();
      initialized = true;
    }
  }, []);

  const rows = authors.map((x: any) => {
    return {
      id: x.id,
      values: [x.id, x.fullName, x.books.length],
    };
  });
  const tableData = {
    headers: [
      {
        width: 50,
        label: "Id",
      },
      {
        label: "Author",
      },
      {
        label: "Book Count",
      },
    ],
    rows,
  };

  return (
    <SimpleDataTable
      title="Authors"
      caption="This table contains Authors"
      data={tableData}
      pagination={pagination}
      borderAxis="xBetween"
      hoverRow
      setCurrentPageNumberAndPageSize={setCurrentPageNumberAndPageSize}
    />
  );
};

export default AuthorTable;
