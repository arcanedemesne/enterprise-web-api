import * as React from "react";
import { useEffectOnce } from "usehooks-ts";
import { Outlet } from "react-router-dom";

// custom;
import { fetchAllUsers } from "../Users/state";
import { useAppDispatch, useAppSelector } from "../../store/hooks";
import { AlertState, IAlert, addAlert } from "../../store/AlertState";

import * as Layout from "../../layouts";
import Navigation from "../../components/Navigation";
import Header from "../../components/Header";
import AlertModal from "../../components/AlertModal";
import Box from "@mui/joy/Box";
import Communication from "../../components/Communication";
import createUniqueKey from "../../utilities/uniqueKey";

export default function Admin() {
  const dispatch = useAppDispatch();
  const alertState: AlertState = useAppSelector((state) => state.alertState);

  const [ drawerOpen, setDrawerOpen] = React.useState(false);

  useEffectOnce(() => {
    const fetchData = async () => {
      const response: any = await dispatch(await fetchAllUsers("users/all"));
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
    <>
      {drawerOpen && (
        <Layout.SideDrawer onClose={() => setDrawerOpen(false)}>
          <Navigation />
        </Layout.SideDrawer>
      )}
      <Layout.Root
        sx={{
          gridTemplateColumns: {
            xs: "1fr",
            sm: "minmax(64px, 200px) minmax(450px, 1fr)",
            md: "minmax(160px, 300px) minmax(600px, 1fr) minmax(300px, 420px)",
          },
          ...(drawerOpen && {
            height: "100vh",
            overflow: "hidden",
          }),
        }}
      >
        <Header setDrawerOpen={setDrawerOpen} />
        <Layout.SideNav>
          <Navigation />
        </Layout.SideNav>
        <Layout.Main>
          {alertState.alerts.length > 0 && (
            <Box
              sx={{
                display: "flex",
                flexDirection: "column",
                gap: 2,
                width: "100%",
                pb: 2
              }}
            >
              {alertState.alerts.map((alert) => (
                <AlertModal
                  key={alert.id}
                  id={alert.id}
                  type={alert.type}
                  message={alert.message}
                />
              ))}
            </Box>
          )}
          <Outlet />
        </Layout.Main>
        <Communication />
      </Layout.Root>
    </>
  );
}
