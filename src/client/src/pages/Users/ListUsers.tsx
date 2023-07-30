import { useLoaderData } from "react-router-dom";

import { GET } from "../../utilities/httpRequest";

import Page from "../../components/Page";
import CreateButton from "../../components/CreateButton";
import { parseHeaders } from "../../utilities/pagination";

import UserTable from "./UserTable";
import { baseUri, domain } from ".";

export const loader = async () => {
  return await GET({ endpoint: baseUri });
};

export const action = async ({ request }: any) => {
  let formData = await request.formData();
  return new Promise(() => formData); // TODO: decide action
};

const ListUsers = () => {
  const { data, headers }: any = useLoaderData();
  const pagination = parseHeaders(headers);

  return (
    <Page
      pageTitle="Viewing Users"
      children={
        <>
          <CreateButton domain={domain} />
          <UserTable apiData={data} paginationHeaders={pagination} />
        </>
      }
    />
  );
};

export default ListUsers;
