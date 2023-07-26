import { useLoaderData } from "react-router-dom";

import { GET } from "../../utilities/httpRequest";

import Page from "../../components/Page";
import { parseHeaders } from "../../utilities/pagination";

import AuthorTable from "./AuthorTable";
import { baseUri } from ".";

export const loader = async () => {
  return await GET({ endpoint: baseUri });
};

export const action = async ({ request }: any) => {
  let formData = await request.formData();
  return new Promise(() => formData); // TODO: decide action
};

const ListAuthors = () => {
  const { data, headers }: any = useLoaderData();
  const pagination = parseHeaders(headers);

  return (
    <Page
      pageTitle="Viewing Authors"
      children={<AuthorTable apiData={data} paginationHeaders={pagination} />}
    />
  );
};

export default ListAuthors;
