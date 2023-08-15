import { useState } from "react";
import { useNavigate } from "react-router-dom";

import DataTable from "../../components/DataTable";
import DeleteDialogModal from "../../components/DeleteDialogModal";

import { domain, IAuthor } from "./";
import { IUser } from "../Users";
import { DELETE } from "../../utilities/httpRequest";
import userHelper from "../../utilities/userHelper";

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
    authors &&
    authors.map((x: any) => {
      return {
        id: x.id,
        values: [
          x.id,
          x.firstName,
          x.lastName,
          x.books?.length,
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
        label: "# Books",
        numeric: true,
        width: 60
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
        title="Authors"
        caption="Authors write books"
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

export default AuthorTable;
