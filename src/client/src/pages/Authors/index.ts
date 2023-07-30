import PAGE_ROUTES from "../../utilities/pageRoutes";

export interface IAuthor {
  id?: number;
  firstName: string;
  lastName: string;
  fullName: string;
  books: any[];
}

export const domain = PAGE_ROUTES.ADMIN.AUTHORS.endpoint;
export const baseUri = `${domain}?includeBooks=true`;