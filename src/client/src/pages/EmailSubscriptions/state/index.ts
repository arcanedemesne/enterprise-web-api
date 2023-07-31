import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import type { RootState } from "../../../store";
import { IEmailSubscription } from "..";
import { IPagination, paginationInitialState, parseHeaders } from "../../../utilities/pagination";
import { GET } from "../../../utilities/httpRequest";

// Define a type for the slice state
export interface EmailSubscriptionState {
  status: 'idle' | 'loading' | 'failed';
  emailSubscriptions: IEmailSubscription[];
  pagination: IPagination;

  currentEmailSubscription: IEmailSubscription | null;
}

// Define the initial state using that type
const initialState: EmailSubscriptionState = {
  status: 'idle',
  emailSubscriptions: [],
  pagination: paginationInitialState,

  currentEmailSubscription: null
};

export const fetchEmailSubscriptions = createAsyncThunk(
  'emailSubscriptionState/fetchEmailSubscriptions',
  async (endpoint: string) => {
    const response: any = await GET({ endpoint });
    // The value we return becomes the `fulfilled` action payload
    const pagination = parseHeaders(response.headers);
    return { data: response.data, pagination };
  }
);

export const fetchEmailSubscriptionById = createAsyncThunk(
  'emailSubscriptionState/fetchEmailSubscriptionById',
  async (endpoint: string) => {
    const response: any = await GET({ endpoint });
    return { data: response.data };
  }
);

export const emailSubscriptionSlice = createSlice({
  name: "emailSubscription",
  // `createSlice` will infer the state type from the `initialState` argument
  initialState,
  reducers: {
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchEmailSubscriptions.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(fetchEmailSubscriptions.fulfilled, (state, action) => {
        state.status = 'idle';
        state.emailSubscriptions = action.payload.data;
        state.pagination = action.payload.pagination;
      })
      .addCase(fetchEmailSubscriptions.rejected, (state) => {
        state.status = 'failed';
      })
      
      .addCase(fetchEmailSubscriptionById.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(fetchEmailSubscriptionById.fulfilled, (state, action) => {
        state.status = 'idle';
        state.currentEmailSubscription = action.payload.data;
      })
      .addCase(fetchEmailSubscriptionById.rejected, (state) => {
        state.status = 'failed';
      });
  },
});

// Other code such as selectors can use the imported `RootState` type
export const getEmailSubscriptionState = (state: RootState) => state.emailSubscriptionState;

export default emailSubscriptionSlice.reducer;
