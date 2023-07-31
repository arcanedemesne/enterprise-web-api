import Page from "../../components/Page";
import EmailSubscriptionForm from "./EmailSubscriptionForm";

const CreateEmailSubscription = () => {
  return (
    <Page
      pageTitle={`Create Email Subscription`}
      children={
        <EmailSubscriptionForm emailSubscription={{
          firstNme: "",
          lastName: "",
        }} formType="create" />
      }
    />
  );
};

export default CreateEmailSubscription;
