import { useEffectOnce } from "usehooks-ts";
import { useParams } from "react-router-dom";

import { CircularProgress } from "@mui/joy";

import { useAppDispatch, useAppSelector } from "../../store/hooks";

import Page from "../../components/Page";

import { BookState, fetchBookById } from "./state";
import BookForm from "./BookForm";

const EditBook = () => {
  const bookState: BookState = useAppSelector((state) => state.bookState);
  const dispatch = useAppDispatch();
  const params = useParams();

  const book = bookState.currentBook;

  useEffectOnce(() => {
    const fetchData = async () => {
      dispatch(await fetchBookById(`books/${params.id}?includeAuthor=true`));
    };

    fetchData();
  });

  return (
    <Page
      pageTitle={`Edit Book: ${params?.id}`}
      children={
        bookState.status === "loading" || !book?.id ? (
          <CircularProgress />
        ) : (
          <BookForm book={{ ...book,
            basePrice: book.basePrice.toString(),
            publishDate: new Date(book.publishDate).toLocaleDateString() }} formType="edit" />
        )
      }
    />
  );
};

export default EditBook;
