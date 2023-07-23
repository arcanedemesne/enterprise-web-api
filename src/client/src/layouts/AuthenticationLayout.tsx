import * as React from "react";

import Container from "@mui/joy/Container";

export default function AuthenticationLayout({
  children,
  reversed,
}: React.PropsWithChildren<{ reversed?: boolean }>) {
  return (
    <Container
      sx={{
        position: "relative",
        minHeight: "100vh",
        py: 15,
        display: "flex",
        flexDirection: reversed ? "column-reverse" : "column",
        alignItems: "center",
      }}
    >
      {children}
    </Container>
  );
}
