import { useNavigate } from "react-router-dom";

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
import Menu from "./Menu";
import { ColorSchemeToggle } from "./ColorSchemeToggle";
import { signOut } from "../auth/user";

const Header = () => {
  const navigate = useNavigate();

  return (
    <Layout.Header>
      <Box
        sx={{
          display: "flex",
          flexDirection: "row",
          alignItems: "center",
          gap: 1.5,
        }}
        onClick={() => navigate("/")}
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
              href: "/",
              active: window.location.href.toLocaleLowerCase().includes('dashboard'),
            },
            {
              label: "Authors",
              href: "/admin/authors",
              active: window.location.href.toLocaleLowerCase().includes('authors'),
            },
            {
              label: "Books",
              href: "/admin/books",
              active: window.location.href.toLocaleLowerCase().includes('books'),
            },
            {
              label: "Artists",
              href: "/admin/artists",
              active: window.location.href.toLocaleLowerCase().includes('artists'),
            },
            {
              label: "Subscriptions",
              href: "/admin/subscriptions",
              active: window.location.href.toLocaleLowerCase().includes('subscriptions'),
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

export default Header;
