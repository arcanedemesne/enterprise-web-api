import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import type { RootState } from "../../../store";
import { IBook } from "..";
import { IPagination, paginationInitialState, parseHeaders } from "../../../utilities/pagination";
import { GET } from "../../../utilities/httpRequest";

// Define a type for the slice state
export interface BookState {
  status: 'idle' | 'loading' | 'failed';
  books: IBook[];
  pagination: IPagination;

  currentBook: IBook | null;
}

// Define the initial state using that type
const initialState: BookState = {
  status: 'idle',
  books: [],
  pagination: paginationInitialState,

  currentBook: null,
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

export const fetchBookById = createAsyncThunk(
  'bookState/fetchBookById',
  async (endpoint: string) => {
    const response: any = await GET({ endpoint });
    return { data: response.data };
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
      })
      
      .addCase(fetchBookById.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(fetchBookById.fulfilled, (state, action) => {
        state.status = 'idle';
        state.currentBook = action.payload.data;
      })
      .addCase(fetchBookById.rejected, (state) => {
        state.status = 'failed';
      });
  },
});

// Other code such as selectors can use the imported `RootState` type
export const getBookState = (state: RootState) => state.bookState;

export default bookSlice.reducer;
