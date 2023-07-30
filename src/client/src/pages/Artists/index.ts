import PAGE_ROUTES from "../../utilities/pageRoutes";

export interface IArtist {
  id?: number;
  firstName: string;
  lastName: string;
  fullName: string;
  covers: any[];
}

export const domain = PAGE_ROUTES.ADMIN.ARTISTS.endpoint;
export const baseUri = `${domain}?IncludeCoversWithBookAndAuthor=true`;