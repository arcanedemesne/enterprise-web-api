import Page from "../../components/Page";
import AuthorForm from "./AuthorForm";

const CreateAuthor = () => {
  return (
    <Page
      pageTitle={`Create Author`}
      children={
        <AuthorForm author={{
          firstNme: "",
          lastName: "",
        }} formType="create" />
      }
    />
  );
};

export default CreateAuthor;
