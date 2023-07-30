import { useEffect } from "react";
import { useAppSelector, useAppDispatch } from "../../store/hooks";

import Page from "../../components/Page";
import CreateButton from "../../components/CreateButton";

import EmailSubscriptionTable from "./EmailSubscriptionTable";
import { baseUri, domain } from ".";
import { EmailSubscriptionState, fetchEmailSubscriptions } from "./state";
import { paginate } from "../../utilities/pagination";

let initialized = false;
const ListArtists = () => {
  const emailSubscriptionState: EmailSubscriptionState = useAppSelector((state) => state.emailSubscriptionState);
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
        dispatch(await fetchEmailSubscriptions(endpoint));
      },
    });
  };

  useEffect(() => {
    const fetchData = async () => {
      dispatch(await fetchEmailSubscriptions(baseUri));
    };

    if (!initialized) {
      fetchData();
      initialized = true;
    }
  });
  return (
    <Page
      pageTitle="Viewing Artists"
      children={
        <>
          <CreateButton domain={domain} />
          <EmailSubscriptionTable
            loading={emailSubscriptionState.status === "loading"}
            emailSubscriptions={emailSubscriptionState.emailSubscriptions}
            pagination={emailSubscriptionState.pagination}
            setNewPaginationValues={setNewPaginationValues}
          />
        </>
      }
    />
  );
};

export default ListArtists;
