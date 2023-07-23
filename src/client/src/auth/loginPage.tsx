 import { useState } from "react";

import {
  login,
  UserMetadata,
}
from "./user";

const dashboardUrl: string = "/dashboard";
let redirectUrl: string = "";

const LoginPage = () => {
  
  const onLogin = async ({ username, password }: any) => {
    try {
      const metadata = await login({ username, password }) as UserMetadata | undefined;
      if (metadata?.email_verified === "true") {
        window.location.href = redirectUrl.length > 0 ? redirectUrl : dashboardUrl;
      }
    } catch (error: any) {
      setErrorMessage(error.statusText);
      console.info(error);
    }
  };

  const [errorMessage, setErrorMessage] = useState<string>("jennifer.allen");
  const [username, setUserName] = useState<string>("jennifer.allen");
  const [password, setPassword] = useState<string>("H@$43m15oN3");

  return (
    <div>
      <h1>Login</h1>
      {errorMessage && (<div className="error">{errorMessage}</div>)}
      <input
        id="username"
        aria-label="User Name"
        placeholder="username"
        type="text"
        name="username"
        value={username || ""}
        onChange={(event) => {
          setUserName(event.currentTarget.value);
        }}
        onKeyUp={async (event) => {
          if (event.code === "Enter") {
            await onLogin({ username, password })
          }
        }}
      /><br />
      <input
        id="password"
        aria-label="Password"
        placeholder="password"
        type="password"
        name="password"
        value={password || ""}
        onChange={(event) => {
          setPassword(event.currentTarget.value);
        }}
        onKeyUp={async (event) => {
          if (event.code === "Enter") {
            await onLogin({ username, password })
          }
        }}
      /><br />
      <button type="submit" onClick={async () => await onLogin({ username, password })}>Login</button>
    </div>
  );
};

export default LoginPage;