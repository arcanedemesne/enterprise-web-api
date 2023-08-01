import Sheet from "@mui/joy/Sheet";
import Typography from "@mui/joy/Typography";

const Page = ({ pageTitle, children }: any) => {
  return (
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
  );
};

export default Page;
