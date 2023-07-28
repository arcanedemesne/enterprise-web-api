import { useState } from "react";
import { useLoaderData, redirect } from "react-router-dom";

import { DELETE, GET, PUT } from "../../utilities/httpRequest";

import Page from "../../components/Page";
import { EditFormButtons } from "../../components/FormButtons";
import { IAuthor, domain } from "./";
import AuthorForm, { hasErrors } from "./AuthorForm";

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
  const updates = Object.fromEntries(formData) as IAuthor;
  await updateItem(params.id, updates);
  return redirect(`/admin/${domain}`);
};

const EditAuthor = () => {
  const { data: author }: any = useLoaderData() as { data: IAuthor };
  const [formValues, setFormValues] = useState(author);
  const [errors, setErrors] = useState({});

  return (
    <Page
      pageTitle={`Edit Author: ${author?.id}`}
      children={
        <AuthorForm
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
              handleDelete={async () => await deleteItem(formValues.id)}
            />
          }
        />
      }
    />
  );
};

export default EditAuthor;
