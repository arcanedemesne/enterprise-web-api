import React from "react";
import ReactDOM from "react-dom/client";

import {
  createBrowserRouter,
  Navigate,
  RouterProvider,
} from "react-router-dom";

import Root, {
  loader as rootLoader,
  action as rootAction,
} from "./routes/root";
import reportWebVitals from "./reportWebVitals";

import App from "./App";
import SplashPage from "./pages/SplashPage";
import SignIn from "./pages/SignIn";
import SignUp from "./pages/SignUp";

import Dashboard from "./pages/Dashboard";

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

export const signInRoute = "/sign-in";
export const signUpRoute = "/sign-up";

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
        path: "dashboard",
        element: (
          <RequireAuth redirectTo={signInRoute}>
            <Dashboard />
          </RequireAuth>
        ),
      },
      {
        path: "sign-in",
        element: <SignIn />,
      },
      {
        path: "sign-up",
        element: <SignUp />,
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
  <React.StrictMode>
    {/* <Navbar keycloak={keycloak} /> */}
    <RouterProvider router={router} />
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
