import { useNavigate } from "react-router-dom";

import Box from "@mui/joy/Box";
import IconButton from "@mui/joy/IconButton";
import Typography from "@mui/joy/Typography";

import GridViewRoundedIcon from "@mui/icons-material/GridViewRounded";
import GroupRoundedIcon from "@mui/icons-material/GroupRounded";
import MenuIcon from "@mui/icons-material/Menu";
import LogoutRoundedIcon from "@mui/icons-material/LogoutRounded";

import * as Layout from "../layouts";
import Menu from "./Menu";
import { ColorSchemeToggle } from "./ColorSchemeToggle";
import { signOut } from "../auth/user";

interface HeaderProps {
  setDrawerOpen: (drawerOpen: boolean) => void;
}

const Header = ({ setDrawerOpen }: HeaderProps) => {
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
      >
        <IconButton
          variant="outlined"
          size="sm"
          onClick={() => setDrawerOpen(true)}
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
        <Typography
          component="h1"
          fontWeight="xl"
          sx={{ cursor: "pointer" }}
          onClick={() => navigate("/")}
        >
          Application Name
        </Typography>
      </Box>
      <Box sx={{ display: "flex", flexDirection: "row", gap: 1.5 }}>
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
              label: "Profile",
              href: "/admin/profile",
              active: window.location.href
                .toLocaleLowerCase()
                .includes("profile"),
            },
            {
              label: "Users",
              href: "/admin/users",
              active: window.location.href
                .toLocaleLowerCase()
                .includes("users"),
            },
            {
              label: "Email Subscriptions",
              href: "/admin/email-subscriptions",
              active: window.location.href
                .toLocaleLowerCase()
                .includes("email-subscriptions"),
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
