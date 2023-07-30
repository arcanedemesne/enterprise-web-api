import PAGE_ROUTES from "../../utilities/pageRoutes";

export interface IUser {
  id?: number;
  keycloakUniqueIdentifier: string;
  userName: string;
  firstName: string;
  lastName: string;
  fullName: string;
  emailAddress: string;
}

export const domain = PAGE_ROUTES.ADMIN.USERS.endpoint;
export const baseUri = `${domain}`;