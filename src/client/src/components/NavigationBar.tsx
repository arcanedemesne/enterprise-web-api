import Box from "@mui/joy/Box";
import IconButton from "@mui/joy/IconButton";
import Input from "@mui/joy/Input";
import Typography from "@mui/joy/Typography";

import GridViewRoundedIcon from "@mui/icons-material/GridViewRounded";
import GroupRoundedIcon from "@mui/icons-material/GroupRounded";
import MenuIcon from "@mui/icons-material/Menu";
import SearchRoundedIcon from "@mui/icons-material/SearchRounded";
import LogoutRoundedIcon from "@mui/icons-material/LogoutRounded";

import * as Layout from "../layouts";
import Menu from "../layouts/Menu";
import { ColorSchemeToggle } from "../components/ColorSchemeToggle";
import { signOut } from "../auth/user";

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
        onClick={() => window.location.href = "/"}
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
        <Typography component="h1" fontWeight="xl" sx={{ cursor: "pointer" }}>
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
              label: "Dashboard",
              active: true,
              href: "/dashboard",
              "aria-current": "page",
            },
            {
              label: "Subscriptions",
              href: "/subscriptions",
            },
            {
              label: "Authors",
              href: "/authors",
            },
            {
              label: "Books",
              href: "/books",
            },
            {
              label: "Artists",
              href: "/artists",
            },
          ]}
        />
        <ColorSchemeToggle />
        <IconButton
          size="sm"
          variant="outlined"
          color="primary"
          component="a"
          onClick={signOut}
        >
          
          <LogoutRoundedIcon />
        </IconButton>
      </Box>
    </Layout.Header>
  );
};

export default NavigationBar;
