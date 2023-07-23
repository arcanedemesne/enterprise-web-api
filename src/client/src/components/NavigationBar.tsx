import Box from "@mui/joy/Box";
import IconButton from "@mui/joy/IconButton";
import Input from "@mui/joy/Input";
import Typography from "@mui/joy/Typography";

import BookRoundedIcon from "@mui/icons-material/BookRounded";
import GridViewRoundedIcon from "@mui/icons-material/GridViewRounded";
import GroupRoundedIcon from "@mui/icons-material/GroupRounded";
import MenuIcon from "@mui/icons-material/Menu";
import SearchRoundedIcon from "@mui/icons-material/SearchRounded";

import * as Layout from "../layouts";
import Menu from "../layouts/Menu";
import { ColorSchemeToggle } from "../components/ColorSchemeToggle";

const NavigationBar = () => {
  return (
    <Layout.Header>
      <Box
        sx={{
          display: "flex",
          flexDirection: "row",
          alignItems: "center",
          gap: 1.5,
        }}
      >
        <IconButton
          variant="outlined"
          size="sm"
          //onClick={() => setDrawerOpen(true)}
          sx={{ display: { sm: "none" } }}
        >
          <MenuIcon />
        </IconButton>
        <IconButton
          size="sm"
          variant="solid"
          sx={{ display: { xs: "none", sm: "inline-flex" } }}
        >
          <GroupRoundedIcon />
        </IconButton>
        <Typography component="h1" fontWeight="xl">
          Application Name
        </Typography>
      </Box>
      <Input
        size="sm"
        placeholder="Search anything…"
        startDecorator={<SearchRoundedIcon color="primary" />}
        sx={{
          flexBasis: "500px",
          display: {
            xs: "none",
            sm: "flex",
          },
        }}
      />
      <Box sx={{ display: "flex", flexDirection: "row", gap: 1.5 }}>
        <IconButton
          size="sm"
          variant="outlined"
          color="primary"
          sx={{ display: { xs: "inline-flex", sm: "none" } }}
        >
          <SearchRoundedIcon />
        </IconButton>
        <IconButton
          size="sm"
          variant="outlined"
          color="primary"
          component="a"
          href="/blog/first-look-at-joy/"
        >
          <BookRoundedIcon />
        </IconButton>
        <Menu
          id="app-selector"
          control={
            <IconButton
              size="sm"
              variant="outlined"
              color="primary"
              aria-label="Apps"
            >
              <GridViewRoundedIcon />
            </IconButton>
          }
          menus={[
            {
              label: "Email",
              href: "/joy-ui/getting-started/templates/email/",
            },
            {
              label: "Team",
              active: true,
              href: "/joy-ui/getting-started/templates/team/",
              "aria-current": "page",
            },
            {
              label: "Files",
              href: "/joy-ui/getting-started/templates/files/",
            },
          ]}
        />
        <ColorSchemeToggle />
      </Box>
    </Layout.Header>
  );
};

export default NavigationBar;
