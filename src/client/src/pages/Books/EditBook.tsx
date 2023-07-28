import { useState } from "react";
import { useLoaderData, redirect } from "react-router-dom";

import { DELETE, GET, PUT } from "../../utilities/httpRequest";

import Page from "../../components/Page";
import { EditFormButtons } from "../../components/FormButtons";
import { IBook, domain } from "./";
import BookForm, { hasErrors } from "./BookForm";

const updateItem = async (id: number, data: any) => {
  await PUT({ endpoint: `${domain}/${id}`, data });
};

const deleteItem = async (id: number) => {
  await DELETE({ endpoint: `${domain}/${id}` });
};

export const loader = async ({ params }: any) => {
  return await GET({ endpoint: `${domain}/${params.id}?includeAuthor=true&includeCoverAndArtists=true` });
};

export const action = async ({ request, params }: any) => {
  let formData = await request.formData();
  const updates = Object.fromEntries(formData) as IBook;
  updates.publishDate = new Date(updates.publishDate);
  await updateItem(params.id, updates);
  return redirect(`/admin/${domain}`);
};

const EditBook = () => {
  const { data: book }: any = useLoaderData() as { data: IBook };
  const [formValues, setFormValues] = useState<any>({
    author: !!book.author ? book.author : null,
    authorId: !!book.authorId ? book.authorId : 0,
    ...book,
    basePrice: !!book.basePrice ? book.basePrice : "0.00",
    publishDate:
      Date.parse(book.publishDate) > 0
        ? new Date(book.publishDate).toLocaleDateString()
        : new Date().toLocaleDateString(),
  });
  const [errors, setErrors] = useState({});

  return (
    <Page
      pageTitle={`Edit Book: ${book?.id}`}
      children={
        <BookForm
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

export default EditBook;
