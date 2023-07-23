import axios from "axios";
import { getItem, setItem } from "../utilities/localStorage";

export interface UserMetadata {
  //roles: string[];
  username: string;
  full_name: string;
  given_name: string;
  family_name: string;
  email_address: string;
  email_verified: string;
  expiry_date: string;
}

const userMetadataKey = "userMetadataKey";
export const getMetadata = (): UserMetadata | undefined=> {
  return getItem(userMetadataKey) as UserMetadata | undefined;
};

export const setMetadata = (metadata: any): void => {
  setItem(userMetadataKey, metadata);
};

const idTokenKey = "idTokenKey";
export const getIdToken = (): UserMetadata | undefined=> {
  return getItem(idTokenKey) as UserMetadata | undefined;
};

export const setIdToken = (id_token: string): void => {
  setItem(idTokenKey, id_token);
};

export const login = async ({ username, password }: any) => {
  // TODO: encode and place username and password in headers instead of body
  let userMetadata = undefined;
  try {
    userMetadata = await axios({
      url: `https://localhost:32768/api/authentication/authenticate?Username=${username}&Password=${password}`,
      method: "POST",
      headers: {
          //authorization: "your token comes here",
      },
    });
  } catch (error) {
    throw new Response("", {
      status: 400,
      statusText: "Invalid username or password",
    });
  } finally {
    if (userMetadata && userMetadata.data) {
      const metadata = userMetadata.data.payload;
      const id_token = userMetadata.data.encodedPayload;
      setMetadata(metadata);
      setIdToken(id_token);
      return id_token && metadata;
    }
  }
};

export const logout = (): void => {
  // TODO: logout user
};