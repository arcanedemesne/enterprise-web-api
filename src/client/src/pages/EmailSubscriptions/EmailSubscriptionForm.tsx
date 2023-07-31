import { useState } from "react";
import { Form, useNavigate } from "react-router-dom";

import FormInput from "../../components/FormInput";
import errorMessages from "../../utilities/errorMessages";

import { domain } from ".";
import formHelper from "../../utilities/formHelper";
import formButtonHelper from "../../utilities/formButtonHelper";

export const hasErrors = (formValues: any): any => {
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

interface EmailSubscriptionFormProps {
  emailSubscription: any;
  formType: "create" | "edit";
}

const EmailSubscriptionForm = ({
  emailSubscription,
  formType,
}: EmailSubscriptionFormProps) => {
  const navigate = useNavigate();

  const [formValues, setFormValues] = useState<any>(emailSubscription);
  const [errors, setErrors] = useState<any>({});

  const formActions = formHelper({
    domain,
    id: emailSubscription.id,
    navigate,
  });

  const buttons = formButtonHelper({
    domain,
    formType,
    hasErrors,
    setErrors,
    formValues,
    formActions,
    isDeleted: emailSubscription.isDeleted,
  });

  return (
    <Form method="post" id="email-subscription-form">
      <div>
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
        <span>Email Address</span>
        <FormInput
          placeholder="Email Address"
          aria-label="Emali address"
          type="text"
          name="emailAddress"
          value={formValues.emailAddress}
          onChange={(event) =>
            setFormValues({
              ...formValues,
              emailAddress: event.currentTarget.value,
            })
          }
          error={errors?.emailAddress}
        />
      </div>
      <p>{buttons}</p>
    </Form>
  );
};

export default EmailSubscriptionForm;
