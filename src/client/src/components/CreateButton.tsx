import Button from "@mui/joy/Button";
import { useNavigate } from "react-router-dom";

interface CreateButtonProps {
  domain: string;
}

const CreateButton = ({ domain }: CreateButtonProps) => {
  const navigate = useNavigate();
  return (
    <Button
      sx={{ position: "relative", my: 2 }}
      type="button"
      onClick={() => {
        navigate(`/${domain}/create`);
      }}
    >
      Create New
    </Button>
  );
};

export default CreateButton;