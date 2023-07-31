import Sheet from "@mui/joy/Sheet";
import Typography from "@mui/joy/Typography";

import * as Layout from "../layouts";

const Page = ({ pageTitle, children }: any) => {
  return (
    <>
      <Layout.Main>
        <Sheet
          variant="outlined"
          sx={{
            height: "auto",
            width: "100hw",
            py: 3, // padding top & bottom
            px: 3, // padding left & right
            gap: 2,
            borderRadius: "md",
            boxShadow: "sm",
          }}
        >
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
