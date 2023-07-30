import { useState } from "react";
import { useNavigate } from "react-router-dom";

import Button from "@mui/joy/Button";

import AlertDialogModal from "./AlertDialogModal";

interface CreateFormButtonsProps {
  domain: string;
  handleSave?: (event: any) => void;
}

export const CreateFormButtons = ({
  domain,
  handleSave,
}: CreateFormButtonsProps) => {
  const navigate = useNavigate();
  return (
    <>
      <Button type="submit" sx={{ mr: 2 }} onClick={handleSave}>
        Save
      </Button>
      <Button
        sx={{ mr: 2 }}
        color="neutral"
        type="button"
        onClick={() => {
          navigate(`/admin/${domain}`);
        }}
      >
        Cancel
      </Button>
    </>
  );
};

interface EditFormButtonsProps {
  domain: string;
  handleSave?: (event: any) => void;
  handleDelete: () => void;
}

export const EditFormButtons = ({
  domain,
  handleSave,
  handleDelete,
}: EditFormButtonsProps) => {
  const navigate = useNavigate();
  
  const [openDeleteModal, setOpenDeleteModal] = useState<boolean>(false);
  
  return (
    <>
      <AlertDialogModal
        open={openDeleteModal}
        message={(<>This will delete the {domain.substring(0, domain.length - 1)}.</>)}
        actionButtonLabel="Delete"
        handleActionButtonClick={async () => {
          await handleDelete();
          navigate(`/admin/${domain}`);
        }}
        handleClose={() => setOpenDeleteModal(false)}
      />
      <CreateFormButtons domain={domain} handleSave={handleSave} />
      <Button
        color="danger"
        type="button"
        onClick={() => setOpenDeleteModal(true)}
      >
        Delete
      </Button>
    </>
  );
};
