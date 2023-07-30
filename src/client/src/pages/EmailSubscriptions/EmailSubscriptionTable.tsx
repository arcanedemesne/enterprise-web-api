import { useState } from "react";
import { useNavigate } from "react-router-dom";

import DataTable from "../../components/DataTable";
import { setPaginationValues } from "../../utilities/pagination";

import { domain, baseUri, IEmailSubscription } from ".";

const EmailSubscriptionTable = ({ apiData, paginationHeaders }: any) => {
  const navigate = useNavigate();

  const [apiResponseData, setApiResponseData] = useState<IEmailSubscription[]>(apiData || []);
  const [pagination, setPagination] = useState<any>(paginationHeaders || {});
  const [selectedRows, setSelectedRows] = useState<number[]>([]);

  const setNewPaginationValues = async (
    pageNumber: number,
    pageSize: number,
    orderBy: string
  ) => {
    setPaginationValues({
      baseUri,
      pageNumber,
      pageSize,
      orderBy,
      setPagination,
      setApiResponseData,
    });
  };

  const handleDeleteItems = async () => {
    await navigate(`/admin/${domain}/delete?ids=${selectedRows}`);
  };

  const handleEditItem = async (id: number) => {
    await navigate(`/admin/${domain}/${id}`);
  };

  const rows = apiResponseData.map((x: any) => {
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
      title="Authors"
      caption="This table contains Authors"
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
