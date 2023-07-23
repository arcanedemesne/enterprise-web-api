import { useState } from "react";

import Button from "@mui/joy/Button";
import Link from "@mui/joy/Link";
import Sheet from "@mui/joy/Sheet";
import Typography from "@mui/joy/Typography";

import { create, ISignUpProps, UserMetadata } from "../../auth/user";
import AuthenticationLayout from "../../layouts/AuthenticationLayout";
import FormInput from "../../components/FormInput";

const dashboardUrl: string = "/dashboard";
let redirectUrl: string = "";

const SignUp = () => {

  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [firstName, setFirstName] = useState<string | null>(null);
  const [lastName, setLastName] = useState<string | null>(null);
  const [emailAddress, setEmailAddress] = useState<string | null>(null);
  const [userName, setUserName] = useState<string | null>(null);
  const [password, setPassword] = useState<string | null>(null);
  const [confirmPassword, setConfirmPassword] = useState<string | null>(null);

  const onSignUp = async () => {
    try {
      const metadata = (await create({ firstName, lastName, userName, emailAddress, password } as ISignUpProps)) as
        | UserMetadata
        | undefined;
      if (metadata?.email_verified === "true") {
        window.location.href =
          redirectUrl.length > 0 ? redirectUrl : dashboardUrl;
      }
    } catch (error: any) {
      setErrorMessage(error.statusText);
      console.info(error);
    }
  };

  const keyupEvent = async (event: React.KeyboardEvent<HTMLInputElement>) => {
    if (event.code === "Enter") {
      await onSignUp();
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
        <div>
          <Typography level="h4" component="h1">
            Join us!
          </Typography>
          <Typography level="body2">Sign up to continue.</Typography>
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
          label="First Name"
          name="firstName"
          placeholder="Given Name"
          value={firstName}
          onChange={(event) => {
            setFirstName(event.currentTarget.value);
          }}
          onKeyUp={keyupEvent}
        />
        <FormInput
          type="text"
          label="Last Name"
          name="lastName"
          placeholder="Family Name"
          value={lastName}
          onChange={(event) => {
            setLastName(event.currentTarget.value);
          }}
          onKeyUp={keyupEvent}
        />
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
          type="text"
          label="Email Address"
          name="emailAddress"
          placeholder="Email Address"
          value={emailAddress}
          onChange={(event) => {
            setEmailAddress(event.currentTarget.value);
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
        <FormInput
          type="password"
          label="Confirm Password"
          name="confirmpassword"
          placeholder="Confirm Password"
          value={confirmPassword}
          onChange={(event) => {
            setConfirmPassword(event.currentTarget.value);
          }}
          onKeyUp={keyupEvent}
        />
        <Button
          sx={{ mt: 1 /* margin top */ }}
          onClick={async () => await onSignUp()}
        >
          Sign up
        </Button>
        <Typography
          endDecorator={<Link href="/sign-in">Sign in</Link>}
          fontSize="sm"
          sx={{ alignSelf: "center" }}
        >
          Already have an account?
        </Typography>
      </Sheet>
    </AuthenticationLayout>
  );
};

export default SignUp;
