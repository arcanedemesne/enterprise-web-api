import React from "react";
import ReactDOM from "react-dom/client";

import {
  createBrowserRouter,
  Navigate,
  RouterProvider,
} from "react-router-dom";

import { StyledEngineProvider } from "@mui/joy/styles";

import { store } from './store';

import Root, {
  loader as rootLoader,
  action as rootAction,
} from "./routes/root";
import reportWebVitals from "./reportWebVitals";

import App from "./App";
import SplashPage from "./pages/SplashPage";
import SignIn from "./pages/SignIn";
import SignUp from "./pages/SignUp";

import Admin from "./pages/Admin";
import Dashboard from "./pages/Dashboard";

import ListAuthors from "./pages/Authors/ListAuthors";
import CreateAuthor from "./pages/Authors/CreateAuthor";
import EditAuthor from "./pages/Authors/EditAuthor";

import ListBooks from "./pages/Books/ListBooks";
import CreateBook from "./pages/Books/CreateBook";
import EditBook from "./pages/Books/EditBook";

import ListArtists from "./pages/Artists/ListArtists";
import CreateArtist from "./pages/Artists/CreateArtist";
import EditArtist from "./pages/Artists/EditArtist";

import ListEmailSubscriptions from "./pages/EmailSubscriptions/ListEmailSubscriptions";
import CreateEmailSubscription from "./pages/EmailSubscriptions/CreateEmailSubscription";
import EditEmailSubscription from "./pages/EmailSubscriptions/EditEmailSubscription";

import ListUsers from "./pages/Users/ListUsers";
import EditUser from "./pages/Users/EditUser";

/* TEST */
import Index from "./routes";
import ErrorPage from "./ErrorPage";

import Contact, {
  loader as contactLoader,
  action as contactAction,
} from "./routes/contact";

import EditContact, { action as editAction } from "./routes/edit";

import { action as destroyAction } from "./routes/destroy";
/* END TEST */

import { isSignedIn } from "./auth/user";
import PAGE_ROUTES from "./utilities/pageRoutes";
import { Provider } from "react-redux";

const RequireAuth = ({ children, redirectTo }: any) => {
  let isAuthenticated = isSignedIn();
  return isAuthenticated ? children : <Navigate to={redirectTo} />;
};

const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    errorElement: <ErrorPage />,
    children: [
      { index: true, element: <SplashPage /> },
      {
        path: PAGE_ROUTES.SIGN_IN.path,
        element: <SignIn />,
      },
      {
        path: PAGE_ROUTES.SIGN_UP.path,
        element: <SignUp />,
      },
      {
        path: PAGE_ROUTES.ADMIN.path,
        element: (
          <RequireAuth redirectTo={PAGE_ROUTES.SIGN_IN.path}>
            <Admin />
          </RequireAuth>
        ),
        children: [
          { path: PAGE_ROUTES.ADMIN.DASHBOARD.path, element: <Dashboard /> },
          {
            path: PAGE_ROUTES.ADMIN.AUTHORS.path,
            element: <ListAuthors />,
          },
          {
            path: PAGE_ROUTES.ADMIN.AUTHORS.CREATE.path,
            element: <CreateAuthor />,
          },
          {
            path: PAGE_ROUTES.ADMIN.AUTHORS.EDIT.path,
            element: <EditAuthor />,
          },
          {
            path: PAGE_ROUTES.ADMIN.BOOKS.path,
            element: <ListBooks />,
          },
          {
            path: PAGE_ROUTES.ADMIN.BOOKS.CREATE.path,
            element: <CreateBook />,
          },
          {
            path: PAGE_ROUTES.ADMIN.BOOKS.EDIT.path,
            element: <EditBook />,
          },
          {
            path: PAGE_ROUTES.ADMIN.ARTISTS.path,
            element: <ListArtists />,
          },
          {
            path: PAGE_ROUTES.ADMIN.ARTISTS.CREATE.path,
            element: <CreateArtist />,
          },
          {
            path: PAGE_ROUTES.ADMIN.ARTISTS.EDIT.path,
            element: <EditArtist />,
          },
          {
            path: PAGE_ROUTES.ADMIN.RECYCLE_BIN.path,
            element: <>Deleted items to go here</>,
          },
          {
            path: PAGE_ROUTES.ADMIN.PROFILE.path,
            element: <>Profile to go here</>,
          },
          {
            path: PAGE_ROUTES.ADMIN.USERS.path,
            element: <ListUsers />,
          },
          {
            path: PAGE_ROUTES.ADMIN.USERS.EDIT.path,
            element: <EditUser />,
          },
          {
            path: PAGE_ROUTES.ADMIN.EMAIL_SUBSCRIPTIONS.path,
            element: <ListEmailSubscriptions />,
          },
          {
            path: PAGE_ROUTES.ADMIN.EMAIL_SUBSCRIPTIONS.CREATE.path,
            element: <CreateEmailSubscription />,
          },
          {
            path: PAGE_ROUTES.ADMIN.EMAIL_SUBSCRIPTIONS.EDIT.path,
            element: <EditEmailSubscription />,
          },
        ],
      },
    ],
  },
  {
    path: "/test",
    element: <Root />,
    errorElement: <ErrorPage />,
    loader: rootLoader,
    action: rootAction,
    children: [
      {
        errorElement: <ErrorPage />,
        children: [
          { index: true, element: <Index /> },
          {
            path: "contacts/:contactId",
            element: <Contact />,
            loader: contactLoader,
            action: contactAction,
          },
          {
            path: "contacts/:contactId/edit",
            element: <EditContact />,
            loader: contactLoader,
            action: editAction,
          },
          {
            path: "contacts/:contactId/destroy",
            action: destroyAction,
            errorElement: <div>Oops! There was an error.</div>,
          },
        ],
      },
    ],
  },
]);

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement
);
root.render(
    <Provider store={store}>
      <StyledEngineProvider injectFirst>
        <RouterProvider router={router} />
      </StyledEngineProvider>
    </Provider>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
