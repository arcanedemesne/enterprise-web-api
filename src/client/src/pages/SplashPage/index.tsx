/* eslint-disable jsx-a11y/anchor-is-valid */
import * as React from "react";

import Button from "@mui/joy/Button";
import Link from "@mui/joy/Link";
import Typography from "@mui/joy/Typography";
import ArrowForward from "@mui/icons-material/ArrowForward";
import Sheet from "@mui/joy/Sheet";

import TwoSidedLayout from "../../layouts/TwoSidedLayout";

export default function SplashPage() {
  return (
    <Sheet
      variant="outlined"
      sx={{
        overflow: "hidden",
        width: "auto",
        mx: "auto", // margin left & right
        my: "auto", // margin top & bottom
        py: 5, // padding tpo & bottom
        display: "flex",
        flexDirection: "column",
        gap: 2,
        alignContent: "center",
      }}
    >
      <TwoSidedLayout>
        <Typography color="primary" fontSize="lg" fontWeight="lg">
          The power to do more
        </Typography>
        <Typography
          level="h1"
          fontWeight="xl"
          fontSize="clamp(1.875rem, 1.3636rem + 2.1818vw, 3rem)"
        >
          A large headlinerer about our product features & services
        </Typography>
        <Typography fontSize="lg" textColor="text.secondary" lineHeight="lg">
          A descriptive secondary text placeholder. Use it to explain your
          business offer better.
        </Typography>
        <Button size="lg" endDecorator={<ArrowForward />}>
          Get Started
        </Button>
        <Typography>
          Already a member?{" "}
          <Link fontWeight="lg" href="/sign-in">
            Sign in
          </Link>
        </Typography>
      </TwoSidedLayout>
    </Sheet>
  );
}
