import { useState } from "react";
import { useNavigate } from "react-router-dom";

import DataTable from "../../components/DataTable";

import { domain, IArtist } from "./";

interface ArtistTableProps {
  loading: boolean;
  artists: IArtist[];
  pagination: any;
  setNewPaginationValues: (pageNumber: number, pageSize: number, orderBy: string) => void;
}

const ArtistTable = ({ loading, artists, pagination, setNewPaginationValues }: ArtistTableProps) => {
  const navigate = useNavigate();
  
  const [selectedRows, setSelectedRows] = useState<any[]>([]);

  const handleDeleteItems = async () => {
    await navigate(`/admin/${domain}/delete?ids=${selectedRows}`);
  };

  const handleEditItem = async (id: number) => {
    await navigate(`/admin/${domain}/${id}`);
  };

  const rows = artists && artists.map((x: any) => {
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
      loading={loading}
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
