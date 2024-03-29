import Box from "@mui/joy/Box";
import Button from "@mui/joy/Button";
import Divider from "@mui/joy/Divider";
import Modal from "@mui/joy/Modal";
import ModalDialog from "@mui/joy/ModalDialog";
import WarningRoundedIcon from "@mui/icons-material/WarningRounded";
import Typography from "@mui/joy/Typography";

interface AlertDialogModalProps {
  open: boolean;
  title?: React.JSX.Element | null;
  message: React.JSX.Element;
  actionButtonLabel: string;
  handleActionButtonClick: () => void;
  handleClose: () => void;
}

const DialogModal = ({
  open,
  title,
  message,
  actionButtonLabel,
  handleActionButtonClick,
  handleClose,
}: AlertDialogModalProps) => {
  return (
    <Modal open={open} onClose={handleClose}>
      <ModalDialog
        variant="outlined"
        role="alertdialog"
        aria-labelledby="alert-dialog-modal-title"
        aria-describedby="alert-dialog-modal-description"
        sx={{
          minWidth: 400,
          maxWidth: 800
        }}
      >
        {title ? title : (<Typography
          id="alert-dialog-modal-title"
          component="h2"
          startDecorator={<WarningRoundedIcon />}
        >
          Confirmation
        </Typography>)}
        <Divider />
        <Typography
          id="alert-dialog-modal-description"
          textColor="text.tertiary"
        >
          {message}
        </Typography>
        <Box
          sx={{ display: "flex", gap: 1, justifyContent: "flex-end", pt: 2 }}
        >
          <Button variant="plain" color="neutral" onClick={handleClose}>
            Cancel
          </Button>
          <Button
            variant="solid"
            color="danger"
            onClick={handleActionButtonClick}
          >
            {actionButtonLabel}
          </Button>
        </Box>
      </ModalDialog>
    </Modal>
  );
};

export default DialogModal;
