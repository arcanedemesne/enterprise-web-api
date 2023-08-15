import { useState } from "react";
import { Form, useNavigate } from "react-router-dom";

import FormInput from "../../components/Inputs/FormInput";
import errorMessages from "../../utilities/errorMessages";

import { domain } from ".";
import formHelper from "../../utilities/formHelper";
import formButtonHelper from "../../utilities/formButtonHelper";

import AsynchronousSearch, {
  IOption,
} from "../../components/AsynchronousSearch";
import { GET } from "../../utilities/httpRequest";
import regex from "../../utilities/regex";

interface FormProps {
  book: any;
  formType: "create" | "edit";
}

// TODO: create dropdown for Authors and Covers so Create works
const BookForm = ({ book, formType }: FormProps) => {
  const navigate = useNavigate();

  const [formValues, setFormValues] = useState<any>(book);
  const [errors, setErrors] = useState<any>({});

  const hasErrors = (formValues: any): any => {
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
    
  const formActions = formHelper({
    domain,
    id: book.id,
    navigate,
  });

  const buttons = formButtonHelper({
    domain,
    formType,
    hasErrors,
    setErrors,
    formValues,
    formActions,
    isDeleted: book.isDeleted,
  });

  const defalutSearchValue = {
    label: formValues.author.fullName,
    id: formValues.authorId,
  } as IOption;
  const [searchOptions, setSearchOptions] = useState<IOption[]>([
    defalutSearchValue as IOption,
  ]);
  const [searchValue, setSearchValue] = useState<IOption | undefined>(
    defalutSearchValue
  );
  const [searchInputValue, setSearchInputValue] = useState<string | undefined>(
    defalutSearchValue.label
  );

  const handleSearchAuthors = async (event: any, searchQuery: string) => {
    setSearchInputValue(searchQuery);
    const response = await await GET({
      endpoint: `authors?searchQuery=${searchQuery}`,
    });
    setSearchOptions(
      response.data.map((x: any) => {
        return { label: x.fullName, id: x.id };
      })
    );
  };

  const handleClickOption = (event: any, option: IOption) => {
    setFormValues({ ...formValues, author: option.label, authorId: option.id });
    setSearchValue(option);
  };

  return (
    <Form method="post" id="book-form">
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
          error={errors?.title}
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
          error={errors?.basePrice}
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
          error={errors?.publishDate}
        />
        <AsynchronousSearch
          label="Author"
          options={searchOptions}
          value={searchValue}
          handleChange={handleClickOption}
          inputValue={searchInputValue}
          handleInputChange={handleSearchAuthors}
        />
      </div>
      <p>{buttons}</p>
    </Form>
  );
};

export default BookForm;
