import { useState } from "react";
import { useNavigate } from "react-router-dom";

import Button from "@mui/joy/Button";
import Link from "@mui/joy/Link";
import Sheet from "@mui/joy/Sheet";
import Typography from "@mui/joy/Typography";

import { ISignInProps, signIn, UserMetadata } from "../../auth/user";
import AuthenticationLayout from "../../layouts/AuthenticationLayout";
import FormInput from "../../components/FormInput";
import { signUpRoute } from "../..";

const dashboardUrl: string = "/dashboard";
let redirectUrl: string = "";

const SignIn = () => {
  const navigate = useNavigate();

  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [userName, setUserName] = useState<string | null>("jennifer.allen");
  const [password, setPassword] = useState<string | null>("H@$43m15oN3");

  const onSignIn = async () => {
    try {
      const metadata = (await signIn({
        userName,
        password,
      } as ISignInProps)) as UserMetadata | undefined;
      if (metadata?.email_verified) {
        navigate(redirectUrl.length > 0 ? redirectUrl : dashboardUrl);
      }
    } catch (error: any) {
      setErrorMessage(error.statusText);
    }
  };

  const keyupEvent = async (event: React.KeyboardEvent<HTMLInputElement>) => {
    if (event.code === "Enter") {
      await onSignIn();
    }
  };

  return (
    <AuthenticationLayout>
      <Sheet
        variant="outlined"
        sx={{
          width: "80%",
          maxWidth: 400,
          mx: "auto", // margin left & right
          my: "auto", // margin top & bottom
          py: 2, // padding top & bottom
          px: 2, // padding left & right
          display: "flex",
          flexDirection: "column",
          gap: 2,
          alignContent: "center",
          borderRadius: "sm",
          boxShadow: "md",
        }}
      >
        <Typography
          level="h3"
          component="h1"
          sx={{ alignSelf: "center", py: 3 }}
        >
          Application Name
        </Typography>
        <div>
          <Typography level="h4" component="h1">
            Welcome!
          </Typography>
          <Typography level="body2">Sign in to continue.</Typography>
        </div>
        <div>
          {errorMessage && (
            <Typography level="body1" sx={{ color: "red" }}>
              {errorMessage}
            </Typography>
          )}
        </div>
        <FormInput
          type="text"
          label="User Name"
          name="userName"
          placeholder="User Name"
          value={userName}
          onChange={(event) => {
            setUserName(event.currentTarget.value);
          }}
          onKeyUp={keyupEvent}
        />
        <FormInput
          type="password"
          label="Password"
          name="password"
          placeholder="Password"
          value={password}
          onChange={(event) => {
            setPassword(event.currentTarget.value);
          }}
          onKeyUp={keyupEvent}
        />
        <Button
          sx={{ mt: 1 /* margin top */ }}
          onClick={async () => await onSignIn()}
        >
          Sign in
        </Button>
        <Typography
          endDecorator={<Link href={signUpRoute}>Sign up</Link>}
          fontSize="sm"
          sx={{ alignSelf: "center" }}
        >
          Don't have an account?
        </Typography>
      </Sheet>
    </AuthenticationLayout>
  );
};

export default SignIn;
