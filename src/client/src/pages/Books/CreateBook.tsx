import { useState } from "react";
import { redirect } from "react-router-dom";

import { POST } from "../../utilities/httpRequest";

import Page from "../../components/Page";
import { CreateFormButtons } from "../../components/FormButtons";
import { IBook, domain } from ".";
import BookForm, { hasErrors } from "./BookForm";

const addItem = async (data: any) => {
  await POST({ endpoint: `${domain}`, data });
};

export const action = async ({ request }: any) => {
  let formData = await request.formData();
  const item = Object.fromEntries(formData) as IBook;
  item.publishDate = new Date(item.publishDate);
  delete item.id;
  await addItem(item);
  return redirect(`/admin/${domain}`);
};

const CreateBook = () => {
  const [formValues, setFormValues] = useState<any>({
    author: { fullName: "" },
    authorId: 0,
    coverId: 0,
    basePrice: "0.00",
    publishDate: new Date().toLocaleDateString(),
  });
  const [errors, setErrors] = useState({});

  return (
    <Page
      pageTitle={`Create Book`}
      children={
        <BookForm
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

export default CreateBook;
