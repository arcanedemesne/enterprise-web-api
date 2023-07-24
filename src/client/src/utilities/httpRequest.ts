import axios from "axios";
import { getAccessToken } from "../auth/user";

const apiServer = "localhost:32768";
const apiRoute = `https://${apiServer}/api`;

export const GET = async ({ controller, endpoint, headers }: any) => {
  return await axios({
    url: `${apiRoute}/${controller}/${endpoint}`,
    method: "GET",
    headers: headers ?? {
      Authorization: `Bearer ${getAccessToken()}`,
    }
  });
};

export const POST = async ({ controller, endpoint, data, headers }: any) => {
  return await axios({
    url: `${apiRoute}/${controller}/${endpoint}`,
    method: "POST",
    headers: headers ?? {
      Authorization: `Bearer ${getAccessToken()}`,
    },
    data
  });
};

export const PUT = async ({ controller, endpoint, data, headers }: any) => {
  return await axios({
    url: `${apiRoute}/${controller}/${endpoint}`,
    method: "PUT",
    headers: headers ?? {
      Authorization: `Bearer ${getAccessToken()}`,
    },
    data
  });
};

export const PATCH = async ({ controller, endpoint, data, headers }: any) => {
  return await axios({
    url: `${apiRoute}/${controller}/${endpoint}`,
    method: "PATCH",
    headers: headers ?? {
      Authorization: `Bearer ${getAccessToken()}`,
    },
    data
  });
};

export const DELETE = async ({ controller, endpoint, headers }: any) => {
  return await axios({
    url: `${apiRoute}/${controller}/${endpoint}`,
    method: "DELETE",
    headers: headers ?? {
      Authorization: `Bearer ${getAccessToken()}`,
    },
  });
};