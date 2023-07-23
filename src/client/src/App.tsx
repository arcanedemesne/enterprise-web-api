import * as React from "react";
import { Outlet } from "react-router-dom";

import { CssVarsProvider } from "@mui/joy/styles";
import Sheet from "@mui/joy/Sheet";
import '@fontsource/public-sans';

const App = () => {
  return (
    <CssVarsProvider>
      <div>Navigation bar</div>
      <Sheet variant="outlined">
        <Outlet />
      </Sheet>
    </CssVarsProvider>
  );
};

export default App;
