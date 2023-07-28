import { Outlet } from "react-router-dom";

import { CssVarsProvider } from "@mui/joy/styles";
import CssBaseline from "@mui/joy/CssBaseline";

import filesTheme from "./layouts/theme";
import "@fontsource/public-sans";
import "./App.css";

const App = () => {
  return (
    <CssVarsProvider disableTransitionOnChange theme={filesTheme}>
      <CssBaseline />
      <Outlet />
    </CssVarsProvider>
  );
};

export default App;
