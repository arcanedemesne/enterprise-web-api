import { useRouteError } from "react-router-dom";
import Page from "./components/Page";
import { CssVarsProvider } from "@mui/joy";

export default function ErrorPage() {
  const error: any = useRouteError();
  console.error(error);

  return (
    <CssVarsProvider>
      <Page
        pageTitle="An error occured"
        children={
          <div id="error-page">
            <h1>Oops!</h1>
            <p>Sorry, an unexpected error has occurred.</p>
            <p>
              <i>{error?.statusText || error.message}</i>
            </p>
          </div>
        }
      />
    </CssVarsProvider>
  );
}
