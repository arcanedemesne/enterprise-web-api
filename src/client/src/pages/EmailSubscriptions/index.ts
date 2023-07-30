import PAGE_ROUTES from "../../utilities/pageRoutes";

export interface IEmailSubscription {
  id?: number;
  firstName: string;
  lastName: string;
  fullName: string;
  emailAddress: string;
}

export const domain = PAGE_ROUTES.ADMIN.EMAIL_SUBSCRIPTIONS.endpoint;
export const baseUri = `${domain}`;