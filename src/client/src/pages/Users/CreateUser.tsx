import { useState } from "react";
import { redirect } from "react-router-dom";

import { POST } from "../../utilities/httpRequest";

import Page from "../../components/Page";
import { CreateFormButtons } from "../../components/FormButtons";
import { IUser, domain } from ".";
import UserForm, { hasErrors } from "./UserForm";

const addItem = async (data: any) => {
  await POST({ endpoint: `${domain}`, data });
};

export const action = async ({ request }: any) => {
  let formData = await request.formData();
  const item = Object.fromEntries(formData) as IUser;
  delete item.id;
  await addItem(item);
  return redirect(`/admin/${domain}`);
};

const CreateUser = () => {
  const [formValues, setFormValues] = useState({});
  const [errors, setErrors] = useState({});

  return (
    <Page
      pageTitle={`Create User`}
      children={
        <UserForm
          formValues={formValues}
          setFormValues={setFormValues}
          errors={errors}
          buttons={
            <CreateFormButtons
              domain={domain}
              handleSave={(event) => {
                const errors = hasErrors(formValues);
                if (errors) {
                  event.preventDefault();
                  setErrors(errors);
                }
              }}
            />
          }
        />
      }
    />
  );
};

export default CreateUser;
