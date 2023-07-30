import { useState } from "react";
import { useLoaderData, redirect } from "react-router-dom";

import { DELETE, GET, PUT } from "../../utilities/httpRequest";

import Page from "../../components/Page";
import { EditFormButtons } from "../../components/FormButtons";
import { IUser, domain } from ".";
import UserForm, { hasErrors } from "./UserForm";

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
  const updates = Object.fromEntries(formData) as IUser;
  await updateItem(params.id, updates);
  return redirect(`/admin/${domain}`);
};

const EditUser = () => {
  const { data: user }: any = useLoaderData() as { data: IUser };
  const [formValues, setFormValues] = useState(user);
  const [errors, setErrors] = useState({});

  return (
    <Page
      pageTitle={`Edit user: ${user?.id}`}
      children={
        <UserForm
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

export default EditUser;
