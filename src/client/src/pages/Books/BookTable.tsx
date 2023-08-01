import { useState } from "react";
import { useNavigate } from "react-router-dom";

import DataTable from "../../components/DataTable";

import { domain, IBook } from "./";
import { IUser } from "../Users";

interface BookTableProps {
  loading: boolean;
  books: IBook[];
  users: IUser[];
  pagination: any;
  setNewPaginationValues: (
    pageNumber: number,
    pageSize: number,
    orderBy: string
  ) => void;
}

const BookTable = ({
  loading,
  books,
  users,
  pagination,
  setNewPaginationValues,
}: BookTableProps) => {
  const navigate = useNavigate();

  const [selectedRows, setSelectedRows] = useState<number[]>([]);

  const handleDeleteItems = async () => {
    await navigate(`/admin/${domain}/delete?ids=${selectedRows}`);
  };

  const handleEditItem = async (id: number) => {
    await navigate(`/admin/${domain}/${id}/edit`);
  };

  const mapUserToKeycloakId = (kcId: string) =>
    users.find((u) => u.keycloakUniqueIdentifier === kcId);

  const rows =
    books &&
    books.map((x: any) => {
      return {
        id: x.id,
        values: [
          x.id,
          x.title,
          x.author.fullName,
          x.basePrice,
          x.coverId ? "true" : "false",
          x.cover?.artists.length || 0,
          new Date(x.publishDate).toDateString(),
          x.createdBy ? mapUserToKeycloakId(x.createdBy)?.fullName : "N/A",
          x.createdTs ? new Date(x.createdTs).toLocaleDateString() : "N/A",
          x.modifiedBy ? mapUserToKeycloakId(x.modifiedBy)?.fullName : "",
          x.modifiedTs ? new Date(x.modifiedTs).toLocaleDateString() : "",
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
      {
        label: "Created By",
      },
      {
        label: "Created Date",
      },
      {
        label: "Modified By",
      },
      {
        label: "Modified Date",
      },
    ],
    rows,
  };

  return (
    <DataTable
      title="Books"
      caption="This table contains Books"
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

export default BookTable;
