import Page from "../../components/Page";
import BookForm from "./BookForm";

const Createbook = () => {
  return (
    <Page
      pageTitle={`Create book`}
      children={
        <BookForm
          book={{
            author: { fullName: "" },
            authorId: 0,
            coverId: 0,
            basePrice: "0.00",
            publishDate: new Date().toLocaleDateString(),
          }}
          formType="create"
        />
      }
    />
  );
};

export default Createbook;
