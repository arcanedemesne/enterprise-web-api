import DialogModal from "./DialogModal";

interface DeleteDialogModalProps {
  open: boolean;
  domain: string;
  handleDelete: () => void;
  handleClose: () => void;
}

const DeleteDialogModal = ({
  open,
  domain,
  handleDelete,
  handleClose,
}: DeleteDialogModalProps) => {

  return (
    <DialogModal
      open={open}
      message={
        <>This will delete the {domain.substring(0, domain.length - 1)}.</>
      }
      actionButtonLabel="Delete"
      handleActionButtonClick={async () => {
        await handleDelete();
      }}
      handleClose={handleClose}
    />
  );
};

export default DeleteDialogModal;
