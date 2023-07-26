import { GET } from "./httpRequest";

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