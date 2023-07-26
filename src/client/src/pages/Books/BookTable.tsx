import { useState } from "react";
import { useNavigate } from "react-router-dom";

import DataTable from "../../components/DataTable";
import { setPaginationValues } from "../../utilities/pagination";

import { domain, baseUri, IBook } from "./";

const BookTable = ({ apiData, paginationHeaders }: any) => {
  const navigate = useNavigate();
  
  const [apiResponseData, setApiResponseData] = useState<IBook[]>(apiData);
  const [pagination, setPagination] = useState<any>(paginationHeaders);
  const [selectedRows, setSelectedRows] = useState<number[]>([]);

  const setNewPaginationValues = async (
    pageNumber: number,
    pageSize: number,
    orderBy: string
  ) => {
    setPaginationValues({
      baseUri,
      pageNumber,
      pageSize,
      orderBy,
      setPagination,
      setApiResponseData,
    });
  };

  const handleDeleteItems = async () => {
    await navigate(`/${domain}/delete?ids=${selectedRows}`);
  };

  const handleEditItem = async (id: number) => {
    await navigate(`/${domain}/${id}`);
  };

  const rows = apiResponseData.map((x: any) => {
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
        width: 100,
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
        label: "Base Price ($)",
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
    <DataTable
      title="Books"
      caption="This table contains Books"
      data={tableData}
      pagination={pagination}
      setNewPaginationValues={setNewPaginationValues}
      canDeleteItems
      handleDeleteItems={handleDeleteItems}
      canEditItems
      handleEditItem={handleEditItem}
      canFilterItems
      selectedRows={selectedRows}
      setSelectedRows={setSelectedRows}
      borderAxis="xBetween"
      hoverRow
    />
  );
};

export default BookTable;
