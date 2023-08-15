import { useState } from "react";
import { useNavigate } from "react-router-dom";

import DataTable from "../../components/DataTable";
import DeleteDialogModal from "../../components/DeleteDialogModal";

import { domain, IEmailSubscription } from "./";
import { DELETE } from "../../utilities/httpRequest";

interface EmailSubscriptionTableProps {
  loading: boolean;
  emailSubscriptions: IEmailSubscription[];
  pagination: any;
  setNewPaginationValues: (
    pageNumber: number,
    pageSize: number,
    orderBy: string
  ) => void;
}

const EmailSubscriptionTable = ({
  loading,
  emailSubscriptions,
  pagination,
  setNewPaginationValues,
}: EmailSubscriptionTableProps) => {
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
    emailSubscriptions &&
    emailSubscriptions.map((x: any) => {
      return {
        id: x.id,
        values: [x.id, x.firstName, x.lastName, x.emailAddress],
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
        id: "emailAddress",
        label: "Email Address",
        numeric: false,
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
        title="Email Subscriptions"
        caption="Email Subscriptions are created by those who wish to here new things"
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

export default EmailSubscriptionTable;
