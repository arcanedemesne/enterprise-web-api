import { useState } from "react";
import { useNavigate } from "react-router-dom";

import DataTable from "../../components/DataTable";

import { domain, IEmailSubscription } from "./";

interface EmailSubscriptionTableProps {
  loading: boolean;
  emailSubscriptions: IEmailSubscription[];
  pagination: any;
  setNewPaginationValues: (pageNumber: number, pageSize: number, orderBy: string) => void;
}

const EmailSubscriptionTable = ({ loading, emailSubscriptions, pagination, setNewPaginationValues }: EmailSubscriptionTableProps) => {
  const navigate = useNavigate();

  const [selectedRows, setSelectedRows] = useState<number[]>([]);

  const handleDeleteItems = async () => {
    await navigate(`/admin/${domain}/delete?ids=${selectedRows}`);
  };

  const handleEditItem = async (id: number) => {
    await navigate(`/admin/${domain}/${id}`);
  };

  const rows = emailSubscriptions && emailSubscriptions.map((x: any) => {
    return {
      id: x.id,
      values: [x.id, x.firstName, x.lastName, x.emailAddress],
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
        id: "emailAddress",
        label: "Email Address",
        numeric: false,
      },
    ],
    rows,
  };

  return (
    <DataTable
      title="Email Subscriptions"
      caption="This table contains Email Subscriptions"
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

export default EmailSubscriptionTable;
