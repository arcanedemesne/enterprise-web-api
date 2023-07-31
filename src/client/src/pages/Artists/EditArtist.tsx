import { useEffect } from "react";
import { useParams } from "react-router-dom";

import { CircularProgress } from "@mui/joy";

import { useAppDispatch, useAppSelector } from "../../store/hooks";

import Page from "../../components/Page";

import { ArtistState, fetchArtistById } from "./state";
import ArtistForm from "./ArtistForm";

let initialized = false;
const EditArtist = () => {
  const artistState: ArtistState = useAppSelector((state) => state.artistState);
  const dispatch = useAppDispatch();
  const params = useParams();

  const artist = artistState.currentArtist;

  useEffect(() => {
    const fetchData = async () => {
      dispatch(await fetchArtistById(`artists/${params.id}`));
    };

    if (!initialized) {
      fetchData();
      initialized = true;
    }
  });

  return (
    <Page
      pageTitle={`Edit Artist: ${params?.id}`}
      children={
        artistState.status === "loading" || !artist?.id ? (
          <CircularProgress />
        ) : (
          <ArtistForm artist={{ ...artist }} formType="edit" />
        )
      }
    />
  );
};

export default EditArtist;
