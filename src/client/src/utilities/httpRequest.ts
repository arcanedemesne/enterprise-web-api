import axios from "axios";
import {
  getAccessToken,
  getRefreshToken,
  getMetadata,
  saveUserMetadata,
} from "../auth/user";
import PAGE_ROUTES from "./pageRoutes";

export const apiServer = "localhost:32768"; // "localhost";
export const apiVersion = "v1";
export const apiRoute = `https://${apiServer}/api`;

export const authenticate = async ({ userName, password }: any) => {
  return await axios({
    url: `${apiRoute}/authentication/authenticate`,
    method: "POST",
    headers: {
      "X-UserName": userName,
      "X-Password": password,
    },
  });
};

export const refreshTokens = async () => {
  const metadata = getMetadata();
  if (Date.now() > metadata?.access_expiry_date!) {
    try {
      const newMatadata = await axios({
        url: `${apiRoute}/authentication/refresh-token`,
        method: "POST",
        headers: {
          "X-Refresh-Token": `${getRefreshToken()}`,
        },
      });
      saveUserMetadata(newMatadata);
    } catch (error: any) {
      window.location.href = PAGE_ROUTES.SIGN_IN.path;
    }
  }
};

export const GET = async ({ endpoint, headers }: any) => {
  await refreshTokens();
  return await axios({
    url: `${apiRoute}/${apiVersion}/${endpoint}`,
    method: "GET",
    headers: headers ?? {
      Authorization: `Bearer ${getAccessToken()}`,
    },
  });
};

export const POST = async ({ endpoint, data, headers }: any) => {
  await refreshTokens();

  data.createdBy = getMetadata()?.keycloakUniqueIdentifier;

  return await axios({
    url: `${apiRoute}/${apiVersion}/${endpoint}`,
    method: "POST",
    headers: headers ?? {
      Authorization: `Bearer ${getAccessToken()}`,
    },
    data,
  });
};

export const PUT = async ({ endpoint, data, headers }: any) => {
  await refreshTokens();
  
  data.modifiedBy = getMetadata()?.keycloakUniqueIdentifier;

  return await axios({
    url: `${apiRoute}/${apiVersion}/${endpoint}`,
    method: "PUT",
    headers: headers ?? {
      Authorization: `Bearer ${getAccessToken()}`,
    },
    data,
  });
};

export const PATCH = async ({ endpoint, data, headers }: any) => {
  await refreshTokens();
  
  data.modifiedBy = getMetadata()?.keycloakUniqueIdentifier;

  return await axios({
    url: `${apiRoute}/${apiVersion}/${endpoint}`,
    method: "PATCH",
    headers: headers ?? {
      Authorization: `Bearer ${getAccessToken()}`,
    },
    data,
  });
};

export const DELETE = async ({ endpoint, headers }: any) => {
  await refreshTokens();
  return await axios({
    url: `${apiRoute}/${apiVersion}/${endpoint}`,
    method: "DELETE",
    headers: headers ?? {
      Authorization: `Bearer ${getAccessToken()}`,
    },
  });
};
