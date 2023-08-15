import { useState } from "react";
import { useNavigate } from "react-router-dom";

import DataTable from "../../components/DataTable";
import DeleteDialogModal from "../../components/DeleteDialogModal";

import { domain, IUser } from "./";
import { DELETE } from "../../utilities/httpRequest";
import userHelper from "../../utilities/userHelper";

interface UserTableProps {
  loading: boolean;
  users: IUser[];
  pagination: any;
  setNewPaginationValues: (
    pageNumber: number,
    pageSize: number,
    orderBy: string
  ) => void;
}

const UserTable = ({
  loading,
  users,
  pagination,
  setNewPaginationValues,
}: UserTableProps) => {
  const navigate = useNavigate();

  const [openDeleteModal, setOpenDeleteModal] = useState<boolean>(false);
  const [idToDelete, setIdToDelete] = useState<number>(-1);

  const handleEditItem = async (id: number) => {
    await navigate(`/admin/${domain}/${id}/edit`);
  };

  const handleDeleteItem = async (id: number) => {
    setOpenDeleteModal(true);
    setIdToDelete(id);
  };

  const handleDelete = async () => {
    await DELETE({ endpoint: `${domain}/${idToDelete}` });
    navigate(`/admin/${domain}`);
  };
  
  const rows =
    users &&
    users.map((x: any) => {
      return {
        id: x.id,
        values: [
          x.id,
          x.userName,
          x.firstName,
          x.lastName,
          x.emailAddress,
          x.createdBy ? userHelper.mapUserToKeycloakId(x.createdBy)?.fullName : "N/A",
          x.createdTs ? new Date(x.createdTs).toDateString() : "N/A",
          x.modifiedBy ? userHelper.mapUserToKeycloakId(x.modifiedBy)?.fullName : "",
          x.modifiedTs ? new Date(x.modifiedTs).toDateString() : "",
        ],
      };
    });
  const tableData = {
    headers: [
      {
        id: "id",
        label: "Id",
        numeric: true,
      },
      {
        id: "userName",
        label: "User Name",
        numeric: false,
        width: 150
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
        width: 250,
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
    <>
      <DeleteDialogModal
        open={openDeleteModal}
        domain={domain}
        handleDelete={handleDelete}
        handleClose={() => setOpenDeleteModal(false)}
      />
      <DataTable
        title="Users"
        caption="Users are people who belong to this application"
        loading={loading}
        data={tableData}
        pagination={pagination}
        setNewPaginationValues={setNewPaginationValues}
        canDeleteItems
        handleDeleteItem={handleDeleteItem}
        canEditItems
        handleEditItem={handleEditItem}
        borderAxis="xBetween"
        hoverRow
      />
    </>
  );
};

export default UserTable;
