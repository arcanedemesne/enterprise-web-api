import { useEffect } from "react";
import { useAppSelector, useAppDispatch } from "../../store/hooks";

import Page from "../../components/Page";
import CreateButton from "../../components/CreateButton";

import BookTable from "./BookTable";
import { baseUri, domain } from ".";
import { BookState, fetchBooks } from "./state";
import { paginate } from "../../utilities/pagination";

let initialized = false;
const ListBooks = () => {
  const bookState: BookState = useAppSelector((state) => state.bookState);
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
        dispatch(await fetchBooks(endpoint));
      },
    });
  };

  useEffect(() => {
    const fetchData = async () => {
      dispatch(await fetchBooks(baseUri));
    };

    if (!initialized) {
      fetchData();
      initialized = true;
    }
  });

  return (
    <Page
      pageTitle="Viewing Books"
      children={
        <>
          <CreateButton domain={domain} />
          <BookTable 
            loading={bookState.status === "loading"}
            books={bookState.books}
            pagination={bookState.pagination}
            setNewPaginationValues={setNewPaginationValues}
          />
        </>
      }
    />
  );
};

export default ListBooks;
