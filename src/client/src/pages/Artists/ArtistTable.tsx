import { useState } from "react";
import { useNavigate } from "react-router-dom"

import { setPaginationValues } from "../../utilities/pagination";
import DataTable from "../../components/DataTable";

import { IArtist, baseUri, domain } from ".";;

const ArtistTable = ({ apiData, paginationHeaders }: any) => {
  const navigate = useNavigate();
  
  const [apiResponseData, setApiResponseData] = useState<IArtist[]>(apiData);
  const [pagination, setPagination] = useState<any>(paginationHeaders);
  const [selectedRows, setSelectedRows] = useState<any[]>([]);

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
      values: [x.id, `${x.firstName}`, `${x.lastName}`, x.covers.length],
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
        label: "Last Name",
        numeric: false,
      },
      {
        label: "Cover Art Count",
        numeric: true,
      },
    ],
    rows,
  };

  return (
    <DataTable
      title="Artists"
      caption="This table contains Artists"
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

export default ArtistTable;
