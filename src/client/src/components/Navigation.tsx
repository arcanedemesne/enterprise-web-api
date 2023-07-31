import List from "@mui/joy/List";
import ListSubheader from "@mui/joy/ListSubheader";
import ListItem from "@mui/joy/ListItem";
import ListItemButton from "@mui/joy/ListItemButton";
import ListItemContent from "@mui/joy/ListItemContent";
import Typography from "@mui/joy/Typography";

import { getMetadata } from "../auth/user";
import { useNavigate } from "react-router-dom";
import PAGE_ROUTES from "../utilities/pageRoutes";

export default function Navigation() {
  const navigate = useNavigate();

  const isActivePage = (path: string) => {
    return window.location.href.includes(path);
  };

  const listItemTemplate = (title: string, pages: string[]) => {
    return (
      
      <ListItem nested sx={{mt: 3}}>
        <ListSubheader>
          {title}
          {/* <IconButton
            size="sm"
            variant="plain"
            color="primary"
            sx={{ "--IconButton-size": "24px", ml: "auto" }}
          >
            <KeyboardArrowDownRoundedIcon fontSize="small" color="primary" />
          </IconButton> */}
        </ListSubheader>
        <List
          aria-labelledby="nav-list-browse"
          sx={{
            "& .JoyListItemButton-root": { p: "8px" },
          }}
        >
          {pages.map((route: any) => {
            const { label, path } = PAGE_ROUTES.ADMIN[route];
            if (label) {
              const isActive = isActivePage(path);
              const navLinkTemplate = ({ navLabel, navPath, isNavLinkActive, ml = 0}: any) => (
              <ListItem key={navPath} sx={{ ml }}>
                <ListItemButton
                  variant={isNavLinkActive ? "soft" : "plain"}
                  color={isNavLinkActive ? "primary" : "neutral"}
                  onClick={() => navigate(`${navPath}`)}
                >
                  <ListItemContent>{navLabel}</ListItemContent>
                </ListItemButton>
              </ListItem>);
              const navLinks = [navLinkTemplate({
                navLabel: label,
                navPath: path,
                isNavLinkActive: isActive,
              })];

              const isCreate = window.location.href.includes("create");
              const createRoute = PAGE_ROUTES.ADMIN[route].CREATE;
              const isEdit = window.location.href.includes("edit");
              const editRoute = PAGE_ROUTES.ADMIN[route].EDIT;
              if (isActive && isCreate && createRoute) {
                navLinks.push(navLinkTemplate({ 
                  navLabel: createRoute.label,
                  navPath: createRoute.path,
                  isNavLinkActive: isCreate,
                  ml: 2,
                }));
              } else if (isActive && isEdit && editRoute) {
                navLinks.push(navLinkTemplate({
                  navLabel: editRoute.label,
                  navPath: editRoute.path,
                  isNavLinkActive: isEdit,
                  ml: 2,
                }));
              }

              return navLinks.map((navLink) => navLink);
            } else return null;
          })}
        </List>
      </ListItem>
    );
  }

  return (
    <List size="sm" sx={{ "--ListItem-radius": "8px", "--List-gap": "4px" }}>
      <ListSubheader>
        <Typography level="body2" alignSelf="right">
          Signed in: <b>{getMetadata()?.full_name}</b>
        </Typography>
      </ListSubheader>
      {listItemTemplate("Application Stuff", ['AUTHORS', 'BOOKS', 'ARTISTS'])}
      {listItemTemplate("User Stuff", ['PROFILE', 'USERS', 'EMAIL_SUBSCRIPTIONS'])}
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
