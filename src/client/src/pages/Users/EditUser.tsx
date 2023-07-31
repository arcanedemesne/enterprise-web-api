import { useEffectOnce } from "usehooks-ts";
import { useParams } from "react-router-dom";

import { CircularProgress } from "@mui/joy";

import { useAppDispatch, useAppSelector } from "../../store/hooks";

import Page from "../../components/Page";

import { UserState, fetchUserById } from "./state";
import UserForm from "./UserForm";

const EditUser = () => {
  const userState: UserState = useAppSelector((state) => state.userState);
  const dispatch = useAppDispatch();
  const params = useParams();

  const user = userState.currentUser;

  useEffectOnce(() => {
    const fetchData = async () => {
      dispatch(await fetchUserById(`users/${params.id}`));
    };

    fetchData();
  });

  return (
    <Page
      pageTitle={`Edit User: ${params?.id}`}
      children={
        userState.status === "loading" || !user?.id ? (
          <CircularProgress />
        ) : (
          <UserForm user={{ ...user }} formType="edit" />
        )
      }
    />
  );
};

export default EditUser;
