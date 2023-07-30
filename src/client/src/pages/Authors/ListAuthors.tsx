import { useEffect } from "react";
import { useAppSelector, useAppDispatch } from "../../store/hooks";

import Page from "../../components/Page";
import CreateButton from "../../components/CreateButton";

import AuthorTable from "./AuthorTable";
import { baseUri, domain } from ".";
import { AuthorState, fetchAuthors } from "./state";
import { paginate } from "../../utilities/pagination";

let initialized = false;
const ListAuthors = () => {
  const authorState: AuthorState = useAppSelector((state) => state.authorState);
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
        dispatch(await fetchAuthors(endpoint));
      },
    });
  };

  useEffect(() => {
    const fetchData = async () => {
      dispatch(await fetchAuthors(baseUri));
    };

    if (!initialized) {
      fetchData();
      initialized = true;
    }
  });

  return (
    <Page
      pageTitle="Viewing Authors"
      children={
        <>
          <CreateButton domain={domain} />
          <AuthorTable
            loading={authorState.status === "loading"}
            authors={authorState.authors}
            pagination={authorState.pagination}
            setNewPaginationValues={setNewPaginationValues}
          />
        </>
      }
    />
  );
};

export default ListAuthors;
