import { useEffect, useState } from "react";

import { GET } from "../../utilities/httpRequest";
import SimpleDataTable from "../../components/SimpleDataTable";

let initialized = false;
const BookTable = () => {
  const [apiData, setData] = useState([]);
  const [pagination, setPagination] = useState({});

  const baseUri = "books?includeAuthor=true&includeCoverAndArtists=true";
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
      values: [
        x.id,
        x.title,
        x.author.fullName,
        x.basePrice,
        x.coverId ? "true" : "false",
        x.cover?.artists.length ?? "None",
        new Date(x.publishDate).toDateString(),
      ],
    };
  });
  const tableData = {
    headers: [
      {
        id: "id",
        width: 50,
        label: "Id",
        numeric: true,
      },
      {
        id: "title",
        label: "Book Title",
        numeric: false,
      },
      {
        label: "Author",
        numeric: false,
      },
      {
        id: "basePrice",
        label: "Price",
        numeric: true,
      },
      {
        label: "Has Cover Art",
        numeric: false,
      },
      {
        label: "Cover Artist Count",
        numeric: true,
      },
      {
        id: "publishDate",
        label: "Publish Date",
        numeric: false,
      },
    ],
    rows,
  };

  return (
    <SimpleDataTable
      title="Books"
      caption="This table contains Books"
      data={tableData}
      pagination={pagination}
      setNewPaginationValues={setNewPaginationValues}
      borderAxis="xBetween"
      hoverRow
    />
  );
};

export default BookTable;
