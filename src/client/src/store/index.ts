import { Action, ThunkAction, configureStore } from "@reduxjs/toolkit";

import authorReducer from "../pages/Authors/state";
import bookReducer from "../pages/Books/state";
import artistReducer from "../pages/Artists/state";

import emailSubscriptionReducer from "../pages/EmailSubscriptions/state";
import userReducer from "../pages/Users/state";

export const store = configureStore({
  reducer: {
    authorState: authorReducer,
    bookState: bookReducer,
    artistState: artistReducer,
    
    emailSubscriptionState: emailSubscriptionReducer,
    userState: userReducer,
  },
});

// Infer the `RootState` and `AppDispatch` types from the store itself
export type RootState = ReturnType<typeof store.getState>;
// Inferred type: {posts: PostsState, comments: CommentsState, users: UsersState}
export type AppDispatch = typeof store.dispatch;
// let toolkit create our actions using thunk
export type AppThunk<ReturnType = void> = ThunkAction<
  ReturnType,
  RootState,
  unknown,
  Action<string>
>;

export interface IBaseEntity { 
  id?: number;
  createdBy: string;
  createdTs: number;
  modifiedBy: string;
  modifiedTs: number;
  isDeleted: boolean;
}