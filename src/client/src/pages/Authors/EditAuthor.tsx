import { useEffectOnce } from "usehooks-ts";
import { useParams } from "react-router-dom";

import { CircularProgress } from "@mui/joy";

import { useAppDispatch, useAppSelector } from "../../store/hooks";

import Page from "../../components/Page";

import { AuthorState, fetchAuthorById } from "./state";
import AuthorForm from "./AuthorForm";

const EditAuthor = () => {
  const authorState: AuthorState = useAppSelector((state) => state.authorState);
  const dispatch = useAppDispatch();
  const params = useParams();

  const author = authorState.currentAuthor;

  useEffectOnce(() => {
    const fetchData = async () => {
      dispatch(await fetchAuthorById(`authors/${params.id}`));
    };

    fetchData();
  });

  return (
    <Page
      pageTitle={`Edit Author: ${params?.id}`}
      children={
        authorState.status === "loading" || !author?.id ? (
          <CircularProgress />
        ) : (
          <AuthorForm author={{ ...author }} formType="edit" />
        )
      }
    />
  );
};

export default EditAuthor;
