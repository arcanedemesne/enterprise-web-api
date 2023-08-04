import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import type { RootState } from ".";

export interface IAlert {
  id: string;
  type: "success" | "warning" | "danger";
  message: string;
}

// Define a type for the slice state
export interface AlertState {
  alerts: IAlert[];
}

// Define the initial state using that type
const initialState: AlertState = {
  alerts: [],
};

export const alertSlice = createSlice({
  name: "alert",
  // `createSlice` will infer the state type from the `initialState` argument
  initialState,
  reducers: {
    addAlert: (state, action: PayloadAction<IAlert>) => {
      state.alerts = [...state.alerts, action.payload];
    },
    removeAlert: (state, action: PayloadAction<string>) => {
      const index = state.alerts.findIndex((a) => a.id === action.payload);
      state.alerts.splice(index, 1);
    },
  },
});

export const { addAlert, removeAlert } = alertSlice.actions;

// Other code such as selectors can use the imported `RootState` type
export const getUserState = (state: RootState) => state.alertState;

export default alertSlice.reducer;
