import { useEffectOnce } from "usehooks-ts";
import { useAppSelector, useAppDispatch } from "../../store/hooks";

import Page from "../../components/Page";
import CreateButton from "../../components/CreateButton";

import ArtistTable from "./ArtistTable";
import { baseUri, domain } from ".";
import { ArtistState, fetchArtists } from "./state";
import { paginate } from "../../utilities/pagination";
import { UserState } from "../Users/state";
import { IAlert, addAlert } from "../../store/AlertState";
import createUniqueKey from "../../utilities/uniqueKey";

const ListArtists = () => {
  const artistState: ArtistState = useAppSelector((state) => state.artistState);
  const userState: UserState = useAppSelector((state) => state.userState);
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
        const response: any = await dispatch(await fetchArtists(endpoint));
        if (response.error) {
          dispatch(
            addAlert({
              id: createUniqueKey(10),
              type: "danger",
              message: response.error.message,
            } as IAlert)
          );
        }
      },
    });
  };

  useEffectOnce(() => {
    const fetchData = async () => {
      const response: any = await dispatch(await fetchArtists(baseUri));
      if (response.error) {
        dispatch(
          addAlert({
            id: createUniqueKey(10),
            type: "danger",
            message: response.error.message,
          } as IAlert)
        );
      }
    };

    fetchData();
  });

  return (
    <Page
      pageTitle="Viewing Artists"
      children={
        <>
          <CreateButton domain={domain} />
          <ArtistTable
            loading={
              artistState.status === "loading" && userState.status === "loading"
            }
            artists={artistState.artists}
            users={userState.users}
            pagination={artistState.pagination}
            setNewPaginationValues={setNewPaginationValues}
          />
        </>
      }
    />
  );
};

export default ListArtists;
