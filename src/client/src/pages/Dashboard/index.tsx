import Sheet from "@mui/joy/Sheet";
import Typography from "@mui/joy/Typography";

import { getMetadata } from "../../auth/user";
import * as Layout from "../../layouts";
import NavigationBar from "../../components/NavigationBar";
import AuthorTable from "./AuthorTable";
import BookTable from "./BookTable";
import ArtistTable from "./ArtistTable";

const Dashboard = () => {
  return (
    <>
      <NavigationBar />
      <Layout.Main>
        <Sheet
          variant="outlined"
          sx={{
            height: "auto",
            width: "auto",
            mx: "auto", // margin left & right
            my: 1, // margin top & bottom
            py: 2, // padding top & bottom
            px: 2, // padding left & right
            gap: 2,
            borderRadius: "sm",
            boxShadow: "md",
          }}
        >
          <Typography level="h4" component="h1">
            Dashboard
          </Typography>
          <Typography level="body2">
            Welcome, <b>{getMetadata()?.full_name}</b>
          </Typography>
        </Sheet>

        <AuthorTable />
        <BookTable />
        <ArtistTable />
      </Layout.Main>
    </>
  );
};

export default Dashboard;
