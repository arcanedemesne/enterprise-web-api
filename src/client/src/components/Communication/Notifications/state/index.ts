import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import type { RootState } from "../../../../store";

export interface INotification {
  id: number;
  message: string;
  createdBy: string;
  createdTs: number;
}

// Define a type for the slice state
export interface NotificationState {
  notifications: INotification[];
}

// Define the initial state using that type
const initialState: NotificationState = {
  notifications: [
    {
      id: 1,
      message: "This is a notification. Need help with Authors and Books.",
      createdBy: "7b6dfb69-49fb-4825-9a11-148172a8359b",
      createdTs: new Date().getDate(),
    },
    {
      id: 2,
      message:
        "This is another notification. Need help with Artists an Covers.",
      createdBy: "7b6dfb69-49fb-4825-9a11-148172a8359b",
      createdTs: new Date().getDate(),
    },
    {
      id: 3,
      message: "This is a notification. Need help with Authors and Books.",
      createdBy: "7b6dfb69-49fb-4825-9a11-148172a8359b",
      createdTs: new Date().getDate(),
    },
    {
      id: 4,
      message:
        "This is another notification. Need help with Artists an Covers.",
      createdBy: "7b6dfb69-49fb-4825-9a11-148172a8359b",
      createdTs: new Date().getDate(),
    },
    {
      id: 5,
      message: "This is a notification. Need help with Authors and Books.",
      createdBy: "7b6dfb69-49fb-4825-9a11-148172a8359b",
      createdTs: new Date().getDate(),
    },
  ],
};

export const notificationSlice = createSlice({
  name: "alert",
  // `createSlice` will infer the state type from the `initialState` argument
  initialState,
  reducers: {
    addNotification: (state, action: PayloadAction<INotification>) => {
      state.notifications = [...state.notifications, action.payload];
    },
    removeNotification: (state, action: PayloadAction<number>) => {
      const index = state.notifications.findIndex((a) => a.id === action.payload);
      state.notifications.splice(index, 1);
    },
  },
});

export const { addNotification, removeNotification } = notificationSlice.actions;

// Other code such as selectors can use the imported `RootState` type
export const getUserState = (state: RootState) => state.notificationState;

export default notificationSlice.reducer;
