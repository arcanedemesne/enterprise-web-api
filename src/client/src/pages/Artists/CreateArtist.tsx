import Page from "../../components/Page";
import ArtistForm from "./ArtistForm";

const CreateArtist = () => {
  return (
    <Page
      pageTitle={`Create Artist`}
      children={
        <ArtistForm artist={{
          firstNme: "",
          lastName: "",
        }} formType="create" />
      }
    />
  );
};

export default CreateArtist;
