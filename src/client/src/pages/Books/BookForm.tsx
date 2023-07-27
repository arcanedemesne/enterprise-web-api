import { Form } from "react-router-dom";
import FormInput from "../../components/FormInput";
import errorMessages from "../../utilities/errorMessages";
import regex from "../../utilities/regex";
import { IBook } from ".";

interface FormProps {
  formValues: any;
  setFormValues: (formValues: IBook) => void;
  buttons: any;
  errors?: any;
}

export const hasErrors = (formValues: any): any => {
  const errors: any = {};

  // First Name
  if (!formValues?.title || formValues?.title.length === 0) {
    errors.title = `Title ${errorMessages.isRequired}.`;
  } else if (formValues?.title.length > 50) {
    errors.title = `Title ${errorMessages.mustBeFiftyCharsOrLess}.`;
  }
  // Base Price
  if (!formValues?.basePrice || formValues?.basePrice.length === 0) {
    errors.basePrice = `Base Price ${errorMessages.isRequired}.`;
  } else if (formValues?.basePrice.search(regex.monetaryFormat) < 0) {
    errors.basePrice = `Base Price ${errorMessages.mustBeMonetaryFormat}.`;
  }
  // Publish Date
  if (!formValues?.publishDate || formValues?.publishDate.length === 0) {
    errors.publishDate = `Publish Date ${errorMessages.isRequired}.`;
  } else if (formValues?.publishDate.search(regex.dateFormat) < 0) {
    errors.publishDate = `Publish Date ${errorMessages.mustBeDateFormat}.`;
  }

  return Object.keys(errors).length > 0 ? errors : false;
};

// TODO: create dropdown for Authors and Covers so Create works
const BookForm = ({
  formValues,
  setFormValues,
  buttons,
  errors = {},
}: FormProps) => {
  return (
    <Form method="post" id="book-form">
      {Object.keys(errors)?.length > 0 &&
        Object.keys(errors)?.map((error: any) => (
          <div style={{ color: "red" }}>{errors[error]}</div>
        ))}
      <div>
        <FormInput type="text" name="id" value={formValues.id} hidden={true} />
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
          hasError={errors?.title}
        />
        <span>Base Price</span>
        <FormInput
          placeholder="Base Price"
          aria-label="Base price"
          type="text"
          name="basePrice"
          value={formValues.basePrice}
          onChange={(event) =>
            setFormValues({
              ...formValues,
              basePrice: event.currentTarget.value,
            })
          }
          hasError={errors?.basePrice}
        />
        <span>Publish Date</span>
        <FormInput
          placeholder="Publish Date"
          aria-label="Publish date"
          type="text"
          name="publishDate"
          value={new Date(formValues.publishDate).toLocaleDateString()}
          onChange={(event) =>
            setFormValues({
              ...formValues,
              publishDate: event.currentTarget.value,
            })
          }
          hasError={errors?.publishDate}
        />
      </div>
      <p>{buttons}</p>
    </Form>
  );
};

export default BookForm;
