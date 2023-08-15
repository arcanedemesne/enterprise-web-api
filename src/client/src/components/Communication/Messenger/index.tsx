import Box from "@mui/joy/Box";
import Button from "@mui/joy/Button";
import Divider from "@mui/joy/Divider";

const Messenger = () => {
  return (
    <>
      People to message here.
      <Divider />
      <Box sx={{ py: 2, px: 1 }}>
        <Button variant="plain" size="sm" endDecorator={<>a gear box</>}>
          Messenger Settings
        </Button>
      </Box>
    </>
  );
};

export default Messenger;
