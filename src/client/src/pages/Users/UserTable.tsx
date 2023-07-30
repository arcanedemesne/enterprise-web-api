import { useState } from "react";
import { useNavigate } from "react-router-dom";

import DataTable from "../../components/DataTable";

import { domain, IUser } from "./";

interface UserTableProps {
  loading: boolean;
  users: IUser[];
  pagination: any;
  setNewPaginationValues: (pageNumber: number, pageSize: number, orderBy: string) => void;
}

const UserTable = ({ loading, users, pagination, setNewPaginationValues }: UserTableProps) => {
  const navigate = useNavigate();
  
  const [selectedRows, setSelectedRows] = useState<number[]>([]);

  const handleDeleteItems = async () => {
    await navigate(`/admin/${domain}/delete?ids=${selectedRows}`);
  };

  const handleEditItem = async (id: number) => {
    await navigate(`/admin/${domain}/${id}`);
  };

  const rows = users && users.map((x: any) => {
    return {
      id: x.id,
      values: [x.id, x.userName, x.firstName, x.lastName, x.emailAddress],
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
        id: "userName",
        label: "User Name",
        numeric: false,
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
        id: "emailAddress",
        label: "Email Address",
        numeric: false,
      },
    ],
    rows,
  };

  return (
    <DataTable
      title="Users"
      caption="This table contains Users"
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

export default UserTable;
