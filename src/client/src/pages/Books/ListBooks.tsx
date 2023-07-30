import { useLoaderData } from "react-router-dom";

import { GET } from "../../utilities/httpRequest";

import Page from "../../components/Page";
import CreateButton from "../../components/CreateButton";
import { parseHeaders } from "../../utilities/pagination";

import BookTable from "./BookTable";
import { baseUri, domain } from ".";

export const loader = async () => {
  return await GET({ endpoint: baseUri });
};

export const action = async ({ request }: any) => {
  let formData = await request.formData();
  return new Promise(() => formData); // TODO: decide action
};

const ListBooks = () => {
  const { data, headers }: any = useLoaderData();
  const pagination = parseHeaders(headers);

  return (
    <Page
      pageTitle="Viewing Books"
      children={
        <>
          <CreateButton domain={domain} />
          <BookTable apiData={data} paginationHeaders={pagination} />
        </>
      }
    />
  );
};

export default ListBooks;
