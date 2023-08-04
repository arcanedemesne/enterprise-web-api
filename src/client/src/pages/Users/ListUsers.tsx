import { useEffectOnce } from "usehooks-ts";
import { useAppSelector, useAppDispatch } from "../../store/hooks";

import Page from "../../components/Page";

import UserTable from "./UserTable";
import { baseUri } from ".";
import { UserState, fetchUsers } from "./state";
import { paginate } from "../../utilities/pagination";
import { IAlert, addAlert } from "../../store/AlertState";
import createUniqueKey from "../../utilities/uniqueKey";

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
        const response: any = await dispatch(await fetchUsers(endpoint));
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
      const response: any = await dispatch(await fetchUsers(baseUri));
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
