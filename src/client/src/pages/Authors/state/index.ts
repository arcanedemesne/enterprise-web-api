import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import type { RootState } from "../../../store";
import { IAuthor } from "..";
import { IPagination, parseHeaders } from "../../../utilities/pagination";
import { GET } from "../../../utilities/httpRequest";

// Define a type for the slice state
export interface AuthorState {
  status: 'idle' | 'loading' | 'failed';
  authors: IAuthor[];
  pagination: IPagination;
}

// Define the initial state using that type
const initialState: AuthorState = {
  status: 'idle',
  authors: [],
  pagination: { TotalItems: 0, CurrentPage: 1, PageSize: 10, OrderBy: "" },
};

export const fetchAuthors = createAsyncThunk(
  'authorState/fetchAuthors',
  async (endpoint: string) => {
    const response: any = await GET({ endpoint });
    // The value we return becomes the `fulfilled` action payload
    const pagination = parseHeaders(response.headers);
    return { data: response.data, pagination };
  }
);

export const authorSlice = createSlice({
  name: "author",
  // `createSlice` will infer the state type from the `initialState` argument
  initialState,
  reducers: {
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchAuthors.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(fetchAuthors.fulfilled, (state, action) => {
        state.status = 'idle';
        state.authors = action.payload.data;
        state.pagination = action.payload.pagination;
      })
      .addCase(fetchAuthors.rejected, (state) => {
        state.status = 'failed';
      });
  },
});

//export const { setAuthors } = authorSlice.actions;

// Other code such as selectors can use the imported `RootState` type
export const getAuthorState = (state: RootState) => state.authorState;

export default authorSlice.reducer;
