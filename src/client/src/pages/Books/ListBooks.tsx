import { useEffectOnce } from "usehooks-ts";
import { useAppSelector, useAppDispatch } from "../../store/hooks";

import Page from "../../components/Page";
import CreateButton from "../../components/CreateButton";

import BookTable from "./BookTable";
import { baseUri, domain } from ".";
import { BookState, fetchBooks } from "./state";
import { paginate } from "../../utilities/pagination";
import { UserState } from "../Users/state";
import { IAlert, addAlert } from "../../store/AlertState";
import createUniqueKey from "../../utilities/uniqueKey";

const ListBooks = () => {
  const bookState: BookState = useAppSelector((state) => state.bookState);
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
        const response: any = await dispatch(await fetchBooks(endpoint));
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
      const response: any = await dispatch(await fetchBooks(baseUri));
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
      pageTitle="Viewing Books"
      children={
        <>
          <CreateButton domain={domain} />
          <BookTable
            loading={
              bookState.status === "loading" && userState.status === "loading"
            }
            books={bookState.books}
            users={userState.users}
            pagination={bookState.pagination}
            setNewPaginationValues={setNewPaginationValues}
          />
        </>
      }
    />
  );
};

export default ListBooks;
