import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import type { RootState } from "../../../store";
import { IBook } from "..";
import { IPagination, parseHeaders } from "../../../utilities/pagination";
import { GET } from "../../../utilities/httpRequest";

// Define a type for the slice state
export interface BookState {
  status: 'idle' | 'loading' | 'failed';
  books: IBook[];
  pagination: IPagination;
}

// Define the initial state using that type
const initialState: BookState = {
  status: 'idle',
  books: [],
  pagination: { TotalItems: 0, CurrentPage: 1, PageSize: 10, OrderBy: "" },
};

export const fetchBooks = createAsyncThunk(
  'bookState/fetchBooks',
  async (endpoint: string) => {
    const response: any = await GET({ endpoint });
    // The value we return becomes the `fulfilled` action payload
    const pagination = parseHeaders(response.headers);
    return { data: response.data, pagination };
  }
);

export const bookSlice = createSlice({
  name: "book",
  // `createSlice` will infer the state type from the `initialState` argument
  initialState,
  reducers: {
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchBooks.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(fetchBooks.fulfilled, (state, action) => {
        state.status = 'idle';
        state.books = action.payload.data;
        state.pagination = action.payload.pagination;
      })
      .addCase(fetchBooks.rejected, (state) => {
        state.status = 'failed';
      });
  },
});

// Other code such as selectors can use the imported `RootState` type
export const getBookState = (state: RootState) => state.bookState;

export default bookSlice.reducer;
