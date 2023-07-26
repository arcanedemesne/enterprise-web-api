import { useState } from "react";
import { Form, useLoaderData, useNavigate, redirect } from "react-router-dom";

import Button from "@mui/joy/Button";

import { DELETE, GET, PUT } from "../../utilities/httpRequest";

import Page from "../../components/Page";
import FormInput from "../../components/FormInput";
import { IArtist, domain } from ".";

const updateArtist = async (id: number, data: any) => {
  await PUT({ endpoint: `${domain}/${id}`, data });
};

const deleteArtist = async (id: number) => {
  await DELETE({ endpoint: `${domain}/${id}` });
};

export const loader = async ({ params }: any) => {
  return await GET({ endpoint: `${domain}/${params.id}` });
};

export const action = async ({ request, params }: any) => {
  let formData = await request.formData();
  const updates = Object.fromEntries(formData) as IArtist;
  await updateArtist(params.id, updates);
  return redirect(`/${domain}`);
};

const EditArtist = () => {
  const navigate = useNavigate();
  const { data: artist }: any = useLoaderData() as { data: IArtist };
  const [formValues, setFormValues] = useState(artist);

  return (
    <Page
      pageTitle={`Editing Artist: ${artist?.fullName}`}
      children={
        <Form method="post" id="artist-form">
          <div>
            <FormInput
              type="text"
              name="id"
              value={formValues.id}
              hidden={true}
            />
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
            />
          </div>
          <p>
            <Button type="submit" sx={{ mr: 2 }}>
              Save
            </Button>
            <Button
              sx={{ mr: 2 }}
              color="neutral"
              type="button"
              onClick={() => {
                navigate(`/${domain}`);
              }}
            >
              Cancel
            </Button>
            <Button
              color="danger"
              type="button"
              onClick={async () => {
                await deleteArtist(formValues.id);
                navigate(`/${domain}`);
              }}
            >
              Delete
            </Button>
          </p>
        </Form>
      }
    />
  );
};

export default EditArtist;
