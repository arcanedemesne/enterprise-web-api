import { useState } from "react";
import { useNavigate } from "react-router-dom";

import DataTable from "../../components/DataTable";
import DeleteDialogModal from "../../components/DeleteDialogModal";

import { domain, IBook } from "./";
import { IUser } from "../Users";
import { DELETE } from "../../utilities/httpRequest";

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
        width: 110
      },
      {
        label: "Has Art",
        numeric: false,
        width: 80
      },
      {
        label: "# Artists",
        numeric: true,
        width: 65
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
    <>
      <DeleteDialogModal
        open={openDeleteModal}
        domain={domain}
        handleDelete={handleDelete}
        handleClose={() => setOpenDeleteModal(false)}
      />
      <DataTable
        title="Books"
        caption="This table contains Books"
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

export default BookTable;
