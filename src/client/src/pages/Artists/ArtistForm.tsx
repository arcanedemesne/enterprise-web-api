import { useState } from "react";
import { Form, useNavigate } from "react-router-dom";

import { DELETE, POST, PUT } from "../../utilities/httpRequest";
import { useAppDispatch } from "../../store/hooks";

import FormInput from "../../components/FormInput";
import errorMessages from "../../utilities/errorMessages";
import { CreateFormButtons, EditFormButtons } from "../../components/FormButtons";

import { baseUri, domain } from ".";
import { fetchArtists } from "./state";

export const hasErrors = (formValues: any): any => {
  const errors: any = {};

  // First Name
  if (!formValues?.firstName || formValues?.firstName.length === 0) {
    errors.firstName = `First Name ${errorMessages.isRequired}.`;
  } else if (formValues?.firstName.length > 50) {
    errors.firstName = `First Name ${errorMessages.mustBeFiftyCharsOrLess}.`;
  }
  // Last Name
  if (!formValues?.lastName || formValues?.lastName.length === 0) {
    errors.lastName = `Last Name ${errorMessages.isRequired}.`;
  } else if (formValues?.lastName.length > 50) {
    errors.lastName = `Last Name ${errorMessages.mustBeFiftyCharsOrLess}.`;
  }

  return Object.keys(errors).length > 0 ? errors : false;
};

interface ArtistFormProps {
  artist: any;
  formType: "create" | "edit";
}

const ArtistForm = ({
  artist,
  formType,
}: ArtistFormProps) => {
  const navigate = useNavigate();
  
  const dispatch = useAppDispatch();

  const [formValues, setFormValues] = useState<any>(artist);
  const [errors, setErrors] = useState<any>({});

  const addItem = async (data: any) => {
    await POST({ endpoint: `${domain}`, data });
    dispatch(await fetchArtists(baseUri));
    navigate(`/admin/${domain}`);
  };

  const updateItem = async (data: any) => {
    await PUT({ endpoint: `${domain}/${artist.id}`, data });
    dispatch(await fetchArtists(baseUri));
    navigate(`/admin/${domain}`);
  };

  const deleteItem = async () => {
    await DELETE({ endpoint: `${domain}/${artist.id}` });
    dispatch(await fetchArtists(baseUri));
    navigate(`/admin/${domain}`);
  };

  let buttons = null;
  if (formType === "create") {
    buttons = (<CreateFormButtons
      domain={domain}
      handleSave={async (event) => {
          event.preventDefault();
        const errors = hasErrors(formValues);
        if (errors) {
          setErrors(errors);
        } else {
          await addItem(formValues);
        }
      }}
    />);
  } else if (formType === "edit") {
    buttons = (<EditFormButtons
      domain={domain}
      handleSave={async (event) => {
        event.preventDefault();
        const errors = hasErrors(formValues);
        if (errors) {
          setErrors(errors);
        } else {
          await updateItem(formValues);
        }
      }}
      handleDelete={async () => 
        artist?.isDeleted
          ? await deleteItem()
          : await updateItem({
              ...formValues,
              isDeleted: true,
            })}
    />);
  }

  return (
    <Form method="post" id="artist-form">
      <div>
        <span>First Name</span>
        <FormInput
          placeholder="First"
          aria-label="First name"
          type="text"
          name="firstName"
          value={formValues.firstName}
          onChange={(event) =>
            setFormValues({
              ...formValues,
              firstName: event.currentTarget.value,
            })
          }
          error={errors?.firstName}
        />
        <span>Last Name</span>
        <FormInput
          placeholder="Last"
          aria-label="Last name"
          type="text"
          name="lastName"
          value={formValues.lastName}
          onChange={(event) =>
            setFormValues({
              ...formValues,
              lastName: event.currentTarget.value,
            })
          }
          error={errors?.lastName}
        />
      </div>
      <p>{buttons}</p>
    </Form>
  );
};

export default ArtistForm;
