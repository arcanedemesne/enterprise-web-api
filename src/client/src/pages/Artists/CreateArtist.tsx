import { useState } from "react";
import { redirect } from "react-router-dom";

import { POST } from "../../utilities/httpRequest";

import Page from "../../components/Page";
import { CreateFormButtons } from "../../components/FormButtons";
import { IArtist, domain } from ".";
import ArtistForm, { hasErrors } from "./ArtistForm";

const addItem = async (data: any) => {
  await POST({ endpoint: `${domain}`, data });
};

export const action = async ({ request }: any) => {
  let formData = await request.formData();
  const item = Object.fromEntries(formData) as IArtist;
  delete item.id;
  await addItem(item);
  return redirect(`/admin/${domain}`);
};

const CreateArtist = () => {
  const [formValues, setFormValues] = useState({});
  const [errors, setErrors] = useState({});

  return (
    <Page
      pageTitle={`Create Artist`}
      children={
        <ArtistForm
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

export default CreateArtist;
