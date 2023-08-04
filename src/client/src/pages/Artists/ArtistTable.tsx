import { useState } from "react";
import { useNavigate } from "react-router-dom";

import { DELETE } from "../../utilities/httpRequest";
import DataTable from "../../components/DataTable";
import DeleteDialogModal from "../../components/DeleteDialogModal";

import { domain, IArtist } from "./";
import { IUser } from "../Users";

interface ArtistTableProps {
  loading: boolean;
  artists: IArtist[];
  users: IUser[];
  pagination: any;
  setNewPaginationValues: (
    pageNumber: number,
    pageSize: number,
    orderBy: string
  ) => void;
}

const ArtistTable = ({
  loading,
  artists,
  users,
  pagination,
  setNewPaginationValues,
}: ArtistTableProps) => {
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

  const mapUserToKeycloakId = (kcId: string) =>
    users.find((u) => u.keycloakUniqueIdentifier === kcId);

  const rows =
    artists &&
    artists.map((x: any) => {
      return {
        id: x.id,
        values: [
          x.id,
          x.firstName,
          x.lastName,
          x.covers.length,
          x.createdBy ? mapUserToKeycloakId(x.createdBy)?.fullName : "N/A",
          x.createdTs ? new Date(x.createdTs).toDateString() : "N/A",
          x.modifiedBy ? mapUserToKeycloakId(x.modifiedBy)?.fullName : "",
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
        label: "# Covers",
        numeric: true,
        width: 50
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
        title="Artists"
        caption="This table contains Artists"
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

export default ArtistTable;
