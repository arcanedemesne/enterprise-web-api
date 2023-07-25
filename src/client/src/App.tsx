import { Outlet } from "react-router-dom";

import { CssVarsProvider } from "@mui/joy/styles";

import "@fontsource/public-sans";
import "./App.css";

const App = () => {
  return (
    <CssVarsProvider>
      <Outlet />
    </CssVarsProvider>
  );
};

export default App;
