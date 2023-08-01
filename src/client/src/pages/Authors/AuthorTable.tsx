import { useState } from "react";
import { useNavigate } from "react-router-dom";

import DataTable from "../../components/DataTable";

import { domain, IAuthor } from "./";
import { IUser } from "../Users";

interface AuthorTableProps {
  loading: boolean;
  authors: IAuthor[];
  users: IUser[];
  pagination: any;
  setNewPaginationValues: (
    pageNumber: number,
    pageSize: number,
    orderBy: string
  ) => void;
}

const AuthorTable = ({
  loading,
  authors,
  users,
  pagination,
  setNewPaginationValues,
}: AuthorTableProps) => {
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
    authors &&
    authors.map((x: any) => {
      return {
        id: x.id,
        values: [
          x.id,
          x.firstName,
          x.lastName,
          x.books?.length,
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
        label: "Book Count",
        numeric: true,
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
      title="Authors"
      caption="This table contains Authors"
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

export default AuthorTable;
