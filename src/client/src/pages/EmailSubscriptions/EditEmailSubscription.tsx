import { useState } from "react";
import { useLoaderData, redirect } from "react-router-dom";

import { DELETE, GET, PUT } from "../../utilities/httpRequest";

import Page from "../../components/Page";
import { EditFormButtons } from "../../components/FormButtons";
import { IEmailSubscription, domain } from ".";
import EmailSubscriptionForm, { hasErrors } from "./EmailSubscriptionForm";

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
  const updates = Object.fromEntries(formData) as IEmailSubscription;
  await updateItem(params.id, updates);
  return redirect(`/admin/${domain}`);
};

const EditEmailSubscription = () => {
  const { data: emailSubscription }: any = useLoaderData() as { data: IEmailSubscription };
  const [formValues, setFormValues] = useState(emailSubscription);
  const [errors, setErrors] = useState({});

  return (
    <Page
      pageTitle={`Edit Email Subscription: ${emailSubscription?.id}`}
      children={
        <EmailSubscriptionForm
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

export default EditEmailSubscription;
