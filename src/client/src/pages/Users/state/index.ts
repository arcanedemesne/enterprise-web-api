import { PayloadAction, createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import type { RootState } from "../../../store";
import { IUser } from "..";
import {
  IPagination,
  paginationInitialState,
  parseHeaders,
} from "../../../utilities/pagination";
import { GET } from "../../../utilities/httpRequest";

// Define a type for the slice state
export interface UserState {
  status: "idle" | "loading" | "failed";
  allUsers: IUser[];
  users: IUser[];
  pagination: IPagination;
  editUser: IUser | null;

  currentUser: IUser | null;
}

// Define the initial state using that type
const initialState: UserState = {
  status: "idle",
  allUsers: [],
  users: [],
  pagination: paginationInitialState,

  editUser: null,
  currentUser: null,
};

export const fetchAllUsers = createAsyncThunk(
  "userState/fetchAllUsers",
  async (endpoint: string) => {
    const response: any = await GET({ endpoint });
    // The value we return becomes the `fulfilled` action payload
    return { data: response.data };
  }
);

export const fetchUsers = createAsyncThunk(
  "userState/fetchUsers",
  async (endpoint: string) => {
    const response: any = await GET({ endpoint });
    // The value we return becomes the `fulfilled` action payload
    const pagination = parseHeaders(response.headers);
    return { data: response.data, pagination };
  }
);

export const fetchUserById = createAsyncThunk(
  "userState/fetchUserById",
  async (endpoint: string) => {
    const response: any = await GET({ endpoint });
    return { data: response.data };
  }
);

export const userSlice = createSlice({
  name: "user",
  // `createSlice` will infer the state type from the `initialState` argument
  initialState,
  reducers: {
    setCurrentUser: (state, action: PayloadAction<IUser>) => {
      state.currentUser = { ...action.payload };
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchAllUsers.pending, (state) => {
        state.status = "loading";
      })
      .addCase(fetchAllUsers.fulfilled, (state, action) => {
        state.status = "idle";
        state.allUsers = action.payload.data;
      })
      .addCase(fetchAllUsers.rejected, (state) => {
        state.status = "failed";
      })

      .addCase(fetchUsers.pending, (state) => {
        state.status = "loading";
      })
      .addCase(fetchUsers.fulfilled, (state, action) => {
        state.status = "idle";
        state.users = action.payload.data;
        state.pagination = action.payload.pagination;
      })
      .addCase(fetchUsers.rejected, (state) => {
        state.status = "failed";
      })

      .addCase(fetchUserById.pending, (state) => {
        state.status = "loading";
      })
      .addCase(fetchUserById.fulfilled, (state, action) => {
        state.status = "idle";
        state.editUser = action.payload.data;
      })
      .addCase(fetchUserById.rejected, (state) => {
        state.status = "failed";
      });
  },
});

export const { setCurrentUser } = userSlice.actions;

// Other code such as selectors can use the imported `RootState` type
export const getUserState = (state: RootState) => state.userState;

export default userSlice.reducer;
