import { useLoaderData } from "react-router-dom";

import { GET } from "../../utilities/httpRequest";

import Page from "../../components/Page";
import { parseHeaders } from "../../utilities/pagination";
import CreateButton from "../../components/CreateButton";

import ArtistTable from "./ArtistTable";
import { baseUri, domain } from ".";

export const loader = async () => {
  return await GET({ endpoint: baseUri });
};

export const action = async ({ request }: any) => {
  let formData = await request.formData();
  return new Promise(() => formData); // TODO: decide action
};

const ListArtists = () => {
  const { data, headers }: any = useLoaderData();
  const pagination = parseHeaders(headers);

  return (
    <Page
      pageTitle="Viewing Artists"
      children={
        <p>
          <CreateButton domain={domain} />
          <ArtistTable apiData={data} paginationHeaders={pagination} />
        </p>
      }
    />
  );
};

export default ListArtists;
