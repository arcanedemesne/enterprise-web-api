
import { useState } from "react";
import { Form, useLoaderData, useNavigate, redirect } from "react-router-dom";

import Button from "@mui/joy/Button";

import { DELETE, GET, PUT } from "../../utilities/httpRequest";

import Page from "../../components/Page";
import FormInput from "../../components/FormInput";
import { IBook, domain } from "./";

const updateBook = async (id: number, data: any) => {
  await PUT({ endpoint: `${domain}/${id}`, data });
};

const deleteBook = async (id: number) => {
  await DELETE({ endpoint: `${domain}/${id}` });
};

export const loader = async ({ params }: any) => {
  return await GET({ endpoint: `${domain}/${params.id}` });
};

export const action = async ({ request, params }: any) => {
  let formData = await request.formData();
  const updates = Object.fromEntries(formData) as IBook;
  await updateBook(params.id, updates);
  return redirect(`/${domain}`);
};

const EditBook = () => {
  const navigate = useNavigate();
  const { data: book }: any = useLoaderData() as { data: IBook };
  const [formValues, setFormValues] = useState(book);

  return (
    <Page
      pageTitle={`Editing Book: ${book?.title}`}
      children={
        <Form method="post" id="book-form">
          <div>
            <FormInput
              type="text"
              name="id"
              value={formValues.id}
              hidden={true}
            />
            <FormInput
              type="text"
              name="authorId"
              value={formValues.authorId}
              hidden={true}
            />
            <FormInput
              type="text"
              name="coverId"
              value={formValues.coverId}
              hidden={true}
            />
            <span>Title</span>
            <FormInput
              placeholder="Title"
              aria-label="Title"
              type="text"
              name="title"
              value={formValues.title}
              onChange={(event) =>
                setFormValues({
                  ...formValues,
                  title: event.currentTarget.value,
                })
              }
            />
            <span>Base Price</span>
            <FormInput
              placeholder="Base Price"
              aria-label="Base price"
              type="text"
              name="basePrice"
              value={formValues.basePrice || 0}
              onChange={(event) =>
                setFormValues({
                  ...formValues,
                  basePrice: event.currentTarget.value,
                })
              }
            />
            <span>Publish Date</span>
            <FormInput
              placeholder="Publish Date"
              aria-label="Publish date"
              type="text"
              name="publishDate"
              value={formValues.publishDate}
              onChange={(event) =>
                setFormValues({
                  ...formValues,
                  publishDate: event.currentTarget.value,
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
                await deleteBook(formValues.id);
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

export default EditBook;
