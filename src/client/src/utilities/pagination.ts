import { GET } from "./httpRequest";

export interface IPagination {
  TotalItems: number;
  CurrentPage: number;
  PageSize: number;
  OrderBy: string;
}

interface PaginateProps {
  baseUri: string;
  pageNumber: number;
  pageSize: number;
  orderBy: string;
  callback: (endpoint: string) => {};
}

export const paginate = ({
  baseUri,
  pageNumber,
  pageSize,
  orderBy,
  callback,
}: PaginateProps) => {
  const endpoint = `${baseUri}${
    baseUri.includes("?") ? "&" : "?"
  }PageNumber=${pageNumber}&PageSize=${pageSize}&OrderBy=${orderBy}`;
  callback(endpoint);
};

interface SetPaginationValuesProps {
  baseUri: string;
  pageNumber: number;
  pageSize: number;
  orderBy: string;
  setPagination: (paginationHeaders: any) => void;
  setApiResponseData: (data: any) => void;
}

export const setPaginationValues = async ({
  baseUri,
  pageNumber,
  pageSize,
  orderBy,
  setPagination,
  setApiResponseData,
}: SetPaginationValuesProps) => {
  const response: any = await GET({
    endpoint: `${baseUri}&PageNumber=${pageNumber}&PageSize=${pageSize}&OrderBy=${orderBy}`,
  });
  const { headers, data } = response;
  const paginationHeaders = JSON.parse(headers.get("x-pagination"));
  setPagination(paginationHeaders);
  setApiResponseData(data);
};

export const parseHeaders = (headers: any) => {
  return JSON.parse(headers.get("x-pagination"));
};

export const paginationInitialState = { TotalItems: 0, CurrentPage: 1, PageSize: 10, OrderBy: "" };