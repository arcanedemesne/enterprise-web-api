
import AspectRatio from "@mui/joy/AspectRatio";
import Avatar from "@mui/joy/Avatar";
import AvatarGroup from "@mui/joy/AvatarGroup";
import Box from "@mui/joy/Box";
import Button from "@mui/joy/Button";
import Typography from "@mui/joy/Typography";
import IconButton from "@mui/joy/IconButton";
import Divider from "@mui/joy/Divider";
import Sheet from "@mui/joy/Sheet";

// Icons import
import EditOutlinedIcon from "@mui/icons-material/EditOutlined";
import CloseIcon from "@mui/icons-material/Close";

const Details = () => {
  return (
    <Sheet
      sx={{
        display: { xs: "none", sm: "initial" },
        borderLeft: "1px solid",
        borderColor: "neutral.outlinedBorder",
      }}
    >
      <Box sx={{ p: 2, display: "flex", alignItems: "center" }}>
        <Typography sx={{ flex: 1 }}>torres-del-paine.png</Typography>
        <IconButton variant="outlined" color="neutral" size="sm">
          <CloseIcon />
        </IconButton>
      </Box>
      <Divider />
      <Box sx={{ display: "flex" }}>
        <Button
          variant="soft"
          sx={{
            borderRadius: 0,
            borderBottom: "2px solid",
            borderColor: "primary.solidBg",
            flex: 1,
            py: "1rem",
          }}
        >
          Details
        </Button>
        <Button
          variant="plain"
          color="neutral"
          sx={{ borderRadius: 0, flex: 1, py: "1rem" }}
        >
          Activity
        </Button>
      </Box>
      <AspectRatio ratio="21/9">
        <img
          alt=""
          src="https://images.unsplash.com/photo-1534067783941-51c9c23ecefd?auto=format&fit=crop&w=774"
        />
      </AspectRatio>
      <Box sx={{ p: 2, display: "flex", gap: 1, alignItems: "center" }}>
        <Typography level="body2" mr={1}>
          Shared with
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
      </Box>
      <Divider />
      <Box
        sx={{
          gap: 2,
          p: 2,
          display: "grid",
          gridTemplateColumns: "auto 1fr",
          "& > *:nth-of-type(odd)": { color: "text.secondary" },
        }}
      >
        <Typography level="body2">Type</Typography>
        <Typography level="body2" textColor="text.primary">
          Image
        </Typography>

        <Typography level="body2">Size</Typography>
        <Typography level="body2" textColor="text.primary">
          3,6 MB (3,258,385 bytes)
        </Typography>

        <Typography level="body2">Storage used</Typography>
        <Typography level="body2" textColor="text.primary">
          3,6 MB (3,258,385 bytes)
        </Typography>

        <Typography level="body2">Location</Typography>
        <Typography level="body2" textColor="text.primary">
          Travel pictures
        </Typography>

        <Typography level="body2">Owner</Typography>
        <Typography level="body2" textColor="text.primary">
          Michael Scott
        </Typography>

        <Typography level="body2">Modified</Typography>
        <Typography level="body2" textColor="text.primary">
          26 October 2016
        </Typography>

        <Typography level="body2">Created</Typography>
        <Typography level="body2" textColor="text.primary">
          5 August 2016
        </Typography>
      </Box>
      <Divider />
      <Box sx={{ py: 2, px: 1 }}>
        <Button variant="plain" size="sm" endDecorator={<EditOutlinedIcon />}>
          Add a description
        </Button>
      </Box>
    </Sheet>
  );
};

export default Details;
