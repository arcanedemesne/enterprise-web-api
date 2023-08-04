import Alert from "@mui/joy/Alert";
import Button from "@mui/joy/Button";
import { Check, Warning } from "@mui/icons-material";
import { useAppDispatch } from "../store/hooks";
import { removeAlert } from "../store/AlertState";

interface AlertModalProps {
  id: string;
  type: "success" | "warning" | "danger";
  message: string;
}

const AlertModal = ({ id, type, message }: AlertModalProps) => {
  const dispatch = useAppDispatch();
  
  return (
    <Alert
      variant="soft"
      color={type}
      startDecorator={type === "success" ? <Check /> : <Warning />}
      endDecorator={
        <Button size="sm" variant="solid" color={type} onClick={() => dispatch(removeAlert(id))}>
          Close
        </Button>
      }
    >
      {message}
    </Alert>
  );
};

export default AlertModal;
