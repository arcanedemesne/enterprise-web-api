import axios from "axios";
import { getIdToken } from "../auth/user";

const apiRoute = "https://localhost:32768/api";

export const GET = async ({ controller, endpoint }: any) => {
  return await axios({
    url: `${apiRoute}/${controller}/${endpoint}`,
    method: "GET",
    headers: {
      "Bearer": `Token ${getIdToken()}`,
    }
  });
};

export const POST = async ({ controller, endpoint, data, headers }: any) => {
  return await axios({
    url: `${apiRoute}/${controller}/${endpoint}`,
    method: "POST",
    headers: headers ?? {
      "Bearer": `Token ${getIdToken()}`,
    },
    data
  });
};