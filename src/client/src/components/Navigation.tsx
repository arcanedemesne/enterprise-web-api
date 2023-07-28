import IconButton from "@mui/joy/IconButton";
import List from "@mui/joy/List";
import ListSubheader from "@mui/joy/ListSubheader";
import ListItem from "@mui/joy/ListItem";
import ListItemButton from "@mui/joy/ListItemButton";
import ListItemContent from "@mui/joy/ListItemContent";
import Typography from "@mui/joy/Typography";

// Icons import
import KeyboardArrowDownRoundedIcon from "@mui/icons-material/KeyboardArrowDownRounded";

import { getMetadata } from "../auth/user";
import { useNavigate } from "react-router-dom";

export default function Navigation() {
  const navigate = useNavigate();

  const isActivePage = (page: string) =>
    window.location.href.toLocaleLowerCase().includes(page);
  const pages = [
    {
      label: "Dashboard",
      value: "dashboard",
    },
    {
      label: "Authors",
      value: "authors",
    },
    {
      label: "Books",
      value: "books",
    },
    {
      label: "Covers",
      value: "covers",
    },
    {
      label: "Artists",
      value: "artists",
    },
  ];

  return (
    <List size="sm" sx={{ "--ListItem-radius": "8px", "--List-gap": "4px" }}>
      <ListSubheader>
        <Typography level="body2" alignSelf="right">
          Signed in: <b>{getMetadata()?.full_name}</b>
        </Typography>
      </ListSubheader>
      <ListItem nested>
        <ListSubheader>
          Application Stuff
          <IconButton
            size="sm"
            variant="plain"
            color="primary"
            sx={{ "--IconButton-size": "24px", ml: "auto" }}
          >
            <KeyboardArrowDownRoundedIcon fontSize="small" color="primary" />
          </IconButton>
        </ListSubheader>
        <List
          aria-labelledby="nav-list-browse"
          sx={{
            "& .JoyListItemButton-root": { p: "8px" },
          }}
        >
          {pages.map((page: { label: string, value: string}) => (
            <ListItem>
              <ListItemButton
                variant={isActivePage(page.value) ? "soft" : "plain"}
                color={isActivePage(page.value) ? "primary" : "neutral"}
                onClick={() => navigate(`/admin/${page.value}`)}
              >
                <ListItemContent>{page.label}</ListItemContent>
              </ListItemButton>
            </ListItem>
          ))}
        </List>
      </ListItem>

      {/* <ListItem nested>
        <ListSubheader>
          Browse
          <IconButton
            size="sm"
            variant="plain"
            color="primary"
            sx={{ "--IconButton-size": "24px", ml: "auto" }}
          >
            <KeyboardArrowDownRoundedIcon fontSize="small" color="primary" />
          </IconButton>
        </ListSubheader>
        <List
          aria-labelledby="nav-list-browse"
          sx={{
            "& .JoyListItemButton-root": { p: "8px" },
          }}
        >
          <ListItem>
            <ListItemButton variant="soft" color="primary">
              <ListItemDecorator sx={{ color: "inherit" }}>
                <FolderOpenIcon fontSize="small" />
              </ListItemDecorator>
              <ListItemContent>My files</ListItemContent>
            </ListItemButton>
          </ListItem>
          <ListItem>
            <ListItemButton>
              <ListItemDecorator sx={{ color: "neutral.500" }}>
                <ShareOutlinedIcon fontSize="small" />
              </ListItemDecorator>
              <ListItemContent>Shared files</ListItemContent>
            </ListItemButton>
          </ListItem>
          <ListItem>
            <ListItemButton>
              <ListItemDecorator sx={{ color: "neutral.500" }}>
                <DeleteRoundedIcon fontSize="small" />
              </ListItemDecorator>
              <ListItemContent>Trash</ListItemContent>
            </ListItemButton>
          </ListItem>
        </List>
      </ListItem>

      <ListItem nested sx={{ mt: 2 }}>
        <ListSubheader>
          Tags
          <IconButton
            size="sm"
            variant="plain"
            color="primary"
            sx={{ "--IconButton-size": "24px", ml: "auto" }}
          >
            <KeyboardArrowDownRoundedIcon fontSize="small" color="primary" />
          </IconButton>
        </ListSubheader>
        <List
          aria-labelledby="nav-list-tags"
          size="sm"
          sx={{
            "--ListItemDecorator-size": "32px",
          }}
        >
          <ListItem>
            <ListItemButton>
              <ListItemDecorator>
                <Box
                  sx={{
                    width: "10px",
                    height: "10px",
                    borderRadius: "99px",
                    bgcolor: "primary.300",
                  }}
                />
              </ListItemDecorator>
              <ListItemContent>Personal</ListItemContent>
            </ListItemButton>
          </ListItem>
          <ListItem>
            <ListItemButton>
              <ListItemDecorator>
                <Box
                  sx={{
                    width: "10px",
                    height: "10px",
                    borderRadius: "99px",
                    bgcolor: "danger.400",
                  }}
                />
              </ListItemDecorator>
              <ListItemContent>Work</ListItemContent>
            </ListItemButton>
          </ListItem>
          <ListItem>
            <ListItemButton>
              <ListItemDecorator>
                <Box
                  sx={{
                    width: "10px",
                    height: "10px",
                    borderRadius: "99px",
                    bgcolor: "warning.500",
                  }}
                />
              </ListItemDecorator>
              <ListItemContent>Travels</ListItemContent>
            </ListItemButton>
          </ListItem>
          <ListItem>
            <ListItemButton>
              <ListItemDecorator>
                <Box
                  sx={{
                    width: "10px",
                    height: "10px",
                    borderRadius: "99px",
                    bgcolor: "success.400",
                  }}
                />
              </ListItemDecorator>
              <ListItemContent>Concert tickets</ListItemContent>
            </ListItemButton>
          </ListItem>
        </List>
      </ListItem> */}
    </List>
  );
}
