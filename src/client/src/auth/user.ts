import { signInRoute } from "..";
import { authenticate } from "../utilities/httpRequest";
import { getItem, setItem } from "../utilities/localStorage";

export interface UserMetadata {
  //roles: string[];
  username: string;
  full_name: string;
  first_name: string;
  last_name: string;
  email_address: string;
  email_verified: boolean;
  expiry_date: number;
  access_expiry_date: number;
}

const userMetadataKey = "userMetadataKey";
export const getMetadata = (): UserMetadata | null => {
  return getItem(userMetadataKey) as UserMetadata | null;
};

export const setMetadata = (metadata: any | null): void => {
  setItem(userMetadataKey, metadata);
};

const idTokenKey = "idTokenKey";
export const getIdToken = (): string | null => {
  return getItem(idTokenKey) as string | null;
};

export const setIdToken = (id_token: string | null): void => {
  setItem(idTokenKey, id_token);
};

const accessTokenKey = "accessTokenKey";
export const getAccessToken = (): string | null => {
  return getItem(accessTokenKey) as string | null;
};

export const setAccessToken = (access_token: string | null): void => {
  setItem(accessTokenKey, access_token);
};

const refreshTokenKey = "refreshTokenKey";
export const getRefreshToken = (): string | null => {
  return getItem(refreshTokenKey) as string | null;
};

export const setRefreshToken = (refresh_token: string | null): void => {
  setItem(refreshTokenKey, refresh_token);
};

export const isSignedIn = (): boolean => {
  const metadata = getMetadata();
  const id_token = getIdToken();
  const refresh_token = getRefreshToken();
  return (
    !!metadata?.email_verified &&
    !!id_token &&
    !!refresh_token &&
    Date.now() < metadata?.expiry_date!
  );
};

export const saveUserMetadata = (userMetadata: any) => {
  if (userMetadata && userMetadata.data) {
    const { id_token, expires_in, access_token, refresh_token, refresh_expires_in } = userMetadata.data;
    setIdToken(id_token);
    setAccessToken(access_token);
    setRefreshToken(refresh_token);
    const metadata = convertToUserMetadata(parseJwt(id_token), expires_in, refresh_expires_in);
    setMetadata(metadata);
    return metadata;
  }
}

const checkSessionTime = 10 * 60 * 1000; // check every 10 mins
const TimeCheckAuth = () => {
  if (!isSignedIn()) {
    window.location.href = signInRoute;
  }
};

export interface ISignInProps {
  userName: string;
  password: string;
}

export const signIn = async ({ userName, password }: ISignInProps) => {
  let userMetadata = undefined;
  try {
    userMetadata = await authenticate({ userName, password });
  } catch (error) {
    throw new Response("", {
      status: 400,
      statusText: "Invalid Email/User Name and/or Password",
    });
  } finally {
    setInterval(() => { TimeCheckAuth() }, checkSessionTime);
    return saveUserMetadata(userMetadata);
  }
};

const parseJwt = (id_token: string) => {
  var base64Url = id_token.split(".")[1];
  var base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
  var jsonPayload = decodeURIComponent(
    window
      .atob(base64)
      .split("")
      .map(function (c) {
        return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
      })
      .join("")
  );

  return JSON.parse(jsonPayload);
};

const convertToUserMetadata = (metadata: any, access_exp: number, refresh_exp: number): UserMetadata => {
  return {
    username: metadata.preferred_username,
    full_name: `${metadata.given_name} ${metadata.family_name}`,
    first_name: metadata.given_name,
    last_name: metadata.family_name,
    email_address: metadata.email,
    email_verified: metadata.email_verified,
    expiry_date:  Date.now() + (refresh_exp * 1000),
    access_expiry_date: Date.now() + (access_exp * 1000),
  } as UserMetadata;
};

export const signOut = (): void => {
  setMetadata(null);
  setIdToken(null);
  setAccessToken(null);
  setRefreshToken(null);
  window.location.href = signInRoute;
};

export interface ISignUpProps {
  firstName: string;
  lastName: string;
  userName: string;
  emailAddress: string;
  password: string;
}
export const create = ({
  firstName,
  lastName,
  userName,
  emailAddress,
  password,
}: ISignUpProps): void => {
  // TODO: create API endpoint to sign up
};
