import { useState } from "react";

import Box from "@mui/joy/Box";
import Button from "@mui/joy/Button";
import ListSubheader from "@mui/joy/ListSubheader";
import Typography from "@mui/joy/Typography";
import Divider from "@mui/joy/Divider";
import Sheet from "@mui/joy/Sheet";
import Notifications from "./Notifications";
import Messenger from "./Messenger";

const TAB_TYPES = {
  NOTIFICATIONS: "notifications",
  MESSENGER: "messenger",
};

const Communication = () => {
  const [currentTab, setCurrentTab] = useState<string>(TAB_TYPES.NOTIFICATIONS);

  return (
    <Sheet
      sx={{
        display: { xs: "none", sm: "initial" },
        borderLeft: "1px solid",
        borderColor: "neutral.outlinedBorder",
      }}
    >
      <Box sx={{ p: 2, display: "flex", alignItems: "center" }}>
        <ListSubheader>
          <Typography level="body2" alignSelf="right" sx={{ flex: 1 }}>
            Communication
          </Typography>
        </ListSubheader>
      </Box>
      <Divider />
      <Box sx={{ display: "flex" }}>
        <Button
          variant={currentTab === TAB_TYPES.NOTIFICATIONS ? "soft" : "plain"}
          sx={{
            borderRadius: 0,
            flex: 1,
            py: "1rem",
            borderColor:
              currentTab === TAB_TYPES.NOTIFICATIONS ? "primary.solidBg" : "",
          }}
          onClick={() => setCurrentTab(TAB_TYPES.NOTIFICATIONS)}
        >
          Notifications
        </Button>
        <Button
          variant={currentTab === TAB_TYPES.MESSENGER ? "soft" : "plain"}
          sx={{
            borderRadius: 0,
            flex: 1,
            py: "1rem",
            borderColor:
              currentTab === TAB_TYPES.MESSENGER ? "primary.solidBg" : "",
          }}
          onClick={() => setCurrentTab(TAB_TYPES.MESSENGER)}
        >
          Messenger
        </Button>
      </Box>
      {/* <Box sx={{ p: 2, display: "flex", gap: 1, alignItems: "center" }}>
        <Typography level="body2" mr={1}>
          Notifications from
        </Typography>
        <AvatarGroup size="sm" sx={{ "--Avatar-size": "24px" }}>
          <Avatar
            src="https://i.pravatar.cc/24?img=6"
            srcSet="https://i.pravatar.cc/48?img=6 2x"
          />
          <Avatar
            src="https://i.pravatar.cc/24?img=7"
            srcSet="https://i.pravatar.cc/48?img=7 2x"
          />
          <Avatar
            src="https://i.pravatar.cc/24?img=8"
            srcSet="https://i.pravatar.cc/48?img=8 2x"
          />
          <Avatar
            src="https://i.pravatar.cc/24?img=9"
            srcSet="https://i.pravatar.cc/48?img=9 2x"
          />
        </AvatarGroup>
      </Box> */}
      <Divider />
      {currentTab === TAB_TYPES.NOTIFICATIONS && (
        <Notifications />
      )}
      {currentTab === TAB_TYPES.MESSENGER && (
        <Messenger />
      )}
    </Sheet>
  );
};

export default Communication;
