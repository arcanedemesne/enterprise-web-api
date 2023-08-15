import { useState } from "react";
import { Form, useNavigate } from "react-router-dom";

import FormInput from "../../components/Inputs/FormInput";
import errorMessages from "../../utilities/errorMessages";

import { domain } from ".";
import formHelper from "../../utilities/formHelper";
import formButtonHelper from "../../utilities/formButtonHelper";

interface AuthorFormProps {
  author: any;
  formType: "create" | "edit";
}

const AuthorForm = ({
  author,
  formType,
}: AuthorFormProps) => {
  const navigate = useNavigate();

  const [formValues, setFormValues] = useState<any>(author);
  const [errors, setErrors] = useState<any>({});
  
  const hasErrors = (formValues: any): any => {
    const errors: any = {};

    // First Name
    if (!formValues?.firstName || formValues?.firstName.length === 0) {
      errors.firstName = `First Name ${errorMessages.isRequired}.`;
    } else if (formValues?.firstName.length > 50) {
      errors.firstName = `First Name ${errorMessages.mustBeFiftyCharsOrLess}.`;
    }
    // Last Name
    if (!formValues?.lastName || formValues?.lastName.length === 0) {
      errors.lastName = `Last Name ${errorMessages.isRequired}.`;
    } else if (formValues?.lastName.length > 50) {
      errors.lastName = `Last Name ${errorMessages.mustBeFiftyCharsOrLess}.`;
    }

    return Object.keys(errors).length > 0 ? errors : false;
  };

  const formActions = formHelper({
    domain,
    id: author.id,
    navigate,
  });

  const buttons = formButtonHelper({
    domain,
    formType,
    hasErrors,
    setErrors,
    formValues,
    formActions,
    isDeleted: author.isDeleted,
  });
  
  return (
    <Form method="post" id="author-form">
      <div>
        <FormInput type="text" name="id" value={formValues.id} hidden={true} />
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
          error={errors?.firstName}
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
          error={errors?.lastName}
        />
      </div>
      <p>{buttons}</p>
    </Form>
  );
};

export default AuthorForm;
