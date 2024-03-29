import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import type { RootState } from "../../../store";
import { IArtist } from "..";
import { IPagination, paginationInitialState, parseHeaders } from "../../../utilities/pagination";
import { GET } from "../../../utilities/httpRequest";

// Define a type for the slice state
export interface ArtistState {
  status: 'idle' | 'loading' | 'failed';
  artists: IArtist[];
  pagination: IPagination;

  currentArtist: IArtist | null;
}

// Define the initial state using that type
const initialState: ArtistState = {
  status: 'idle',
  artists: [],
  pagination: paginationInitialState,

  currentArtist: null,
};

export const fetchArtists = createAsyncThunk(
  'artistState/fetchArtists',
  async (endpoint: string) => {
    const response: any = await GET({ endpoint });
    // The value we return becomes the `fulfilled` action payload
    const pagination = parseHeaders(response.headers);
    return { data: response.data, pagination };
  }
);

export const fetchArtistById = createAsyncThunk(
  'artistState/fetchArtistById',
  async (endpoint: string) => {
    const response: any = await GET({ endpoint });
    return { data: response.data };
  }
);

export const artistSlice = createSlice({
  name: "artist",
  // `createSlice` will infer the state type from the `initialState` argument
  initialState,
  reducers: {
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchArtists.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(fetchArtists.fulfilled, (state, action) => {
        state.status = 'idle';
        state.artists = action.payload.data;
        state.pagination = action.payload.pagination;
      })
      .addCase(fetchArtists.rejected, (state) => {
        state.status = 'failed';
      })
      
      .addCase(fetchArtistById.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(fetchArtistById.fulfilled, (state, action) => {
        state.status = 'idle';
        state.currentArtist = action.payload.data;
      })
      .addCase(fetchArtistById.rejected, (state) => {
        state.status = 'failed';
      });
  },
});

// Other code such as selectors can use the imported `RootState` type
export const getArtistState = (state: RootState) => state.artistState;

export default artistSlice.reducer;
