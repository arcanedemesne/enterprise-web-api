import { useState } from "react";

import Box from "@mui/joy/Box";
import Button from "@mui/joy/Button";
import Divider from "@mui/joy/Divider";

// Icons import
import EditOutlinedIcon from "@mui/icons-material/EditOutlined";
import Notification from "./Notification";
import Stack from "@mui/joy/Stack";

import { IUser } from "../../../pages/Users";
import { UserState } from "../../../pages/Users/state";
import userHelper from "../../../utilities/userHelper";
import { useAppSelector } from "../../../store/hooks";
import { NotificationState } from "./state";
import DialogModal from "../../DialogModal";
import Typography from "@mui/joy/Typography";
import { Form } from "react-router-dom";
import FormTextArea from "../../Inputs/FormTextArea";
import errorMessages from "../../../utilities/errorMessages";
import AsynchronousSearch, { IOption } from "../../AsynchronousSearch";

const Notifications = () => {
  const notificationState: NotificationState = useAppSelector(
    (state) => state.notificationState
  );
  const userState: UserState = useAppSelector((state) => state.userState);

  const [openCreateNotificationModal, setOpenCreateNotificationModal] = useState(false);
  const [searchValue, setSearchValue] = useState({label: "", id: ""});
  const [searchInputValue, setSearchInputValue] = useState("");
  

  const searchOptions: IOption[] = userState.allUsers.map((user: IUser) => {
    return {
      label: user.fullName,
      id: user.keycloakUniqueIdentifier,
    };
  });
  const handleClickOption = () => {};
  const handleSearchAuthors = () => {};

  const hasErrors = (formValues: any): any => {
    const errors: any = {};

    // First Name
    if (!formValues?.message || formValues?.message.length === 0) {
      errors.message = `Message ${errorMessages.isRequired}.`;
    }

    return Object.keys(errors).length > 0 ? errors : false;
  };

  const [formValues, setFormValues] = useState<any>({
    message: "",
    assignedTo: "",
  });
  const [errors, setErrors] = useState<any>({});

  return (
    <>
      <DialogModal
        open={openCreateNotificationModal}
        title={
          <Typography
            id="alert-dialog-modal-title"
            component="h2"
            startDecorator={<EditOutlinedIcon />}
          >
            Create a Notification
          </Typography>
        }
        message={
          <>
            <Form method="post" id="artist-form">
              <div>
                <span>Message</span>
                <FormTextArea
                  placeholder="Message"
                  aria-label="Message"
                  minRows={4}
                  name="message"
                  value={formValues.message}
                  onChange={(event) =>
                    setFormValues({
                      ...formValues,
                      message: event.currentTarget.value,
                    })
                  }
                  error={errors?.message}
                />
                <AsynchronousSearch
                  label="User"
                  options={searchOptions}
                  value={searchValue}
                  handleChange={handleClickOption}
                  inputValue={searchInputValue}
                  handleInputChange={handleSearchAuthors}
                />
              </div>
            </Form>
          </>
        }
        actionButtonLabel="Notify"
        handleActionButtonClick={() => { console.info('not implemented yet')}}
        handleClose={() => setOpenCreateNotificationModal(false)}
      />
      <Box sx={{ p: 1 }}>
        <Button variant="plain" size="sm" endDecorator={<EditOutlinedIcon />} onClick={() => setOpenCreateNotificationModal(true)}>
          Create a notification
        </Button>
      </Box>
      <Divider />
      <Stack>
        {notificationState.notifications.length > 0 &&
          notificationState.notifications.map((n) => (
            <Notification
              key={n.id}
              id={n.id}
              message={n.message}
              createdBy={userHelper.mapUserToKeycloakId(n.createdBy)?.fullName}
              createdDateTime={new Date(n.createdTs).toLocaleString()}
            />
          ))}
      </Stack>
    </>
  );
};

export default Notifications;
