import { useEffectOnce } from "usehooks-ts";
import { useAppSelector, useAppDispatch } from "../../store/hooks";

import Page from "../../components/Page";
import CreateButton from "../../components/CreateButton";

import AuthorTable from "./AuthorTable";
import { baseUri, domain } from ".";
import { AuthorState, fetchAuthors } from "./state";
import { paginate } from "../../utilities/pagination";
import { UserState } from "../Users/state";
import { IAlert, addAlert } from "../../store/AlertState";
import createUniqueKey from "../../utilities/uniqueKey";

const ListAuthors = () => {
  const authorState: AuthorState = useAppSelector((state) => state.authorState);
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
        const response: any = await dispatch(await fetchAuthors(endpoint));
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
      const response: any = await dispatch(await fetchAuthors(baseUri));
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
      pageTitle="Viewing Authors"
      children={
        <>
          <CreateButton domain={domain} />
          <AuthorTable
            loading={authorState.status === "loading" && userState.status === "loading"}
            authors={authorState.authors}
            users={userState.users}
            pagination={authorState.pagination}
            setNewPaginationValues={setNewPaginationValues}
          />
        </>
      }
    />
  );
};

export default ListAuthors;
