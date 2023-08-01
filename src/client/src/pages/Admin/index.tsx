import * as React from "react";
import { useEffectOnce } from "usehooks-ts";
import { Outlet } from "react-router-dom";

// custom;
import { fetchAllUsers } from "../Users/state";
import { useAppDispatch } from "../../store/hooks";

import * as Layout from "../../layouts";
import Navigation from "../../components/Navigation";
import Header from "../../components/Header";

export default function Admin() {
  const dispatch = useAppDispatch();

  useEffectOnce(() => {
    const fetchData = async () => {
      dispatch(await fetchAllUsers("users/all"));
    };

    fetchData();
  });

  const [drawerOpen ] = React.useState(true);
  
  return (
    <>
      <Layout.Root
        sx={{
          gridTemplateColumns: {
            xs: "1fr",
            sm: "minmax(64px, 200px) minmax(450px, 1fr)",
            md: "minmax(160px, 300px) minmax(900px, 1fr)",
          },
          ...(drawerOpen && {
            height: "100vh",
            overflow: "hidden",
          }),
        }}
      >
        <Header />
        <Layout.SideNav>
          <Navigation />
        </Layout.SideNav>
        <Layout.Main>
          
          <Outlet />
        </Layout.Main>
        {/* <Details /> */}
      </Layout.Root>
    </>
  );
}
