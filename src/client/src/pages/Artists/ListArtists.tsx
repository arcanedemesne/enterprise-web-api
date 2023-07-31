import { useEffect } from "react";
import { useAppSelector, useAppDispatch } from "../../store/hooks";

import Page from "../../components/Page";
import CreateButton from "../../components/CreateButton";

import ArtistTable from "./ArtistTable";
import { baseUri, domain } from ".";
import { ArtistState, fetchArtists } from "./state";
import { paginate } from "../../utilities/pagination";

let initialized = false;
const ListArtists = () => {
  const artistState: ArtistState = useAppSelector((state) => state.artistState);
  const dispatch = useAppDispatch();

  const setNewPaginationValues = (
    pageNumber: number,
    pageSize: number,
    orderBy: string
  ) => {
    paginate({
      baseUri,
      pageNumber,
      pageSize,
      orderBy,
      callback: async (endpoint: string) => {
        dispatch(await fetchArtists(endpoint));
      },
    });
  };

  useEffect(() => {
    const fetchData = async () => {
      dispatch(await fetchArtists(baseUri));
    };

    if (!initialized) {
      fetchData();
      initialized = true;
    }
  });
  
  return (
    <Page
      pageTitle="Viewing Artists"
      children={
        <>
          <CreateButton domain={domain} />
          <ArtistTable
            loading={artistState.status === "loading"}
            artists={artistState.artists}
            pagination={artistState.pagination}
            setNewPaginationValues={setNewPaginationValues}
          />
        </>
      }
    />
  );
};

export default ListArtists;
