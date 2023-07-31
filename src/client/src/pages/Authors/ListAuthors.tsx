import { useEffectOnce } from "usehooks-ts";
import { useAppSelector, useAppDispatch } from "../../store/hooks";

import Page from "../../components/Page";
import CreateButton from "../../components/CreateButton";

import AuthorTable from "./AuthorTable";
import { baseUri, domain } from ".";
import { AuthorState, fetchAuthors } from "./state";
import { paginate } from "../../utilities/pagination";

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

  useEffectOnce(() => {
    const fetchData = async () => {
      dispatch(await fetchAuthors(baseUri));
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
