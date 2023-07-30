import { useEffect } from "react";
import { useAppSelector, useAppDispatch } from "../../store/hooks";

import Page from "../../components/Page";

import UserTable from "./UserTable";
import { baseUri } from ".";
import { UserState, fetchUsers } from "./state";
import { paginate } from "../../utilities/pagination";

let initialized = false;
const ListUsers = () => {
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
        dispatch(await fetchUsers(endpoint));
      },
    });
  };

  useEffect(() => {
    const fetchData = async () => {
      dispatch(await fetchUsers(baseUri));
    };

    if (!initialized) {
      fetchData();
      initialized = true;
    }
  });

  return (
    <Page
      pageTitle="Viewing Users"
      children={
        <>
          <UserTable
            loading={userState.status === "loading"}
            users={userState.users}
            pagination={userState.pagination}
            setNewPaginationValues={setNewPaginationValues}
          />
        </>
      }
    />
  );
};

export default ListUsers;
