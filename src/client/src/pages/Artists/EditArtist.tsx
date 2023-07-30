import { useState } from "react";
import { useLoaderData, redirect } from "react-router-dom";

import { DELETE, GET, PUT } from "../../utilities/httpRequest";

import Page from "../../components/Page";
import { EditFormButtons } from "../../components/FormButtons";
import { IArtist, domain } from ".";
import ArtistForm, { hasErrors } from "./ArtistForm";

const updateItem = async (id: number, data: any) => {
  await PUT({ endpoint: `${domain}/${id}`, data });
};

const deleteItem = async (id: number) => {
  await DELETE({ endpoint: `${domain}/${id}` });
};

export const loader = async ({ params }: any) => {
  return await GET({ endpoint: `${domain}/${params.id}` });
};

export const action = async ({ request, params }: any) => {
  let formData = await request.formData();
  const updates = Object.fromEntries(formData) as IArtist;
  await updateItem(params.id, updates);
  return redirect(`/admin/${domain}`);
};

const EditArtist = () => {
  const { data: artist }: any = useLoaderData() as { data: IArtist };
  const [formValues, setFormValues] = useState(artist);
  const [errors, setErrors] = useState({});

  return (
    <Page
      pageTitle={`Edit Artist: ${artist?.id}`}
      children={
        <ArtistForm
          formValues={formValues}
          setFormValues={setFormValues}
          errors={errors}
          buttons={
            <EditFormButtons
              domain={domain}
              handleSave={(event) => {
                const errors = hasErrors(formValues);
                if (errors) {
                  event.preventDefault();
                  setErrors(errors);
                }
              }}
              handleDelete={async () => 
                formValues.isDeleted
                  ? await deleteItem(formValues.id)
                  : await updateItem(formValues.id, {
                      ...formValues,
                      isDeleted: true,
                    })}
            />
          }
        />
      }
    />
  );
};

export default EditArtist;
