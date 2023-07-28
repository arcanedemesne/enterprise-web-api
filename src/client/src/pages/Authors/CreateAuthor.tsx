import { useState } from "react";
import { redirect } from "react-router-dom";

import { POST } from "../../utilities/httpRequest";

import Page from "../../components/Page";
import { CreateFormButtons } from "../../components/FormButtons";
import { IAuthor, domain } from ".";
import AuthorForm, { hasErrors } from "./AuthorForm";

const addItem = async (data: any) => {
  await POST({ endpoint: `${domain}`, data });
};

export const action = async ({ request }: any) => {
  let formData = await request.formData();
  const item = Object.fromEntries(formData) as IAuthor;
  delete item.id;
  await addItem(item);
  return redirect(`/admin/${domain}`);
};

const CreateAuthor = () => {
  const [formValues, setFormValues] = useState({});
  const [errors, setErrors] = useState({});

  return (
    <Page
      pageTitle={`Create Author`}
      children={
        <AuthorForm
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

export default CreateAuthor;
