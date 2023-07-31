import { useEffectOnce } from "usehooks-ts";
import { useParams } from "react-router-dom";

import { CircularProgress } from "@mui/joy";

import { useAppDispatch, useAppSelector } from "../../store/hooks";

import Page from "../../components/Page";

import { EmailSubscriptionState, fetchEmailSubscriptionById } from "./state";
import EmailSubscriptionForm from "./EmailSubscriptionForm";

const EditEmailSubscription = () => {
  const emailSubscriptionState: EmailSubscriptionState = useAppSelector((state) => state.emailSubscriptionState);
  const dispatch = useAppDispatch();
  const params = useParams();

  const emailSubscription = emailSubscriptionState.currentEmailSubscription;

  useEffectOnce(() => {
    const fetchData = async () => {
      dispatch(await fetchEmailSubscriptionById(`email-subscriptions/${params.id}`));
    };

    fetchData();
  });

  return (
    <Page
      pageTitle={`Edit EmailSubscription: ${params?.id}`}
      children={
        emailSubscriptionState.status === "loading" || !emailSubscription?.id ? (
          <CircularProgress />
        ) : (
          <EmailSubscriptionForm emailSubscription={{ ...emailSubscription }} formType="edit" />
        )
      }
    />
  );
};

export default EditEmailSubscription;
