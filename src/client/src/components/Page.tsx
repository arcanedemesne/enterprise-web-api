import Sheet from "@mui/joy/Sheet";
import Typography from "@mui/joy/Typography";

import { getMetadata } from "../auth/user";
import * as Layout from "../layouts";

import NavigationBar from "./NavigationBar";

const Page = ({ pageTitle, children }: any) => {
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
          <Typography level="body2" alignSelf="right">
            Signed in as: <b>{getMetadata()?.full_name}</b>
          </Typography>
          <Typography level="h4" component="h1">
            {pageTitle}
          </Typography>

          {children}
        </Sheet>
      </Layout.Main>
    </>
  );
};

export default Page;
