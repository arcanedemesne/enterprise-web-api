import Box from "@mui/joy/Box";
import IconButton from "@mui/joy/IconButton";
import Sheet from "@mui/joy/Sheet";
import Typography from "@mui/joy/Typography";

// Icons import
import CloseIcon from "@mui/icons-material/Close";

import { useAppDispatch } from "../../../store/hooks";
import { removeNotification } from "./state";

interface NotificationProps {
  id: number;
  message: string;
  createdBy: string | undefined;
  createdDateTime: string;
}

const Notification = ({
  id,
  message,
  createdBy,
  createdDateTime,
} : NotificationProps) => {
  const dispatch = useAppDispatch();

  return (
    <Sheet variant="outlined" color="neutral" sx={{ p: 1, flex: 1 }}>
        <IconButton
          color="neutral"
          size="sm"
          sx={{ position: "absolute", right: "0.5rem" }}
          onClick={() => dispatch(removeNotification(id))}
        >
          <CloseIcon />
        </IconButton>
        <Box sx={{ m: ".5rem", mr: "2.5rem" }}>
          <Typography level="body1">
            {message}
          </Typography>
          <Box sx={{ m: ".5rem"}}>
            <Typography level="body2">{createdBy ?? "N/A"}</Typography>
            <Typography level="body2">{createdDateTime}</Typography>
          </Box>
        </Box>
      </Sheet>
  );
};

export default Notification;