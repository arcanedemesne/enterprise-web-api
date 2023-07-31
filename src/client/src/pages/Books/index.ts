import { IBaseEntity } from "../../store";
import PAGE_ROUTES from "../../utilities/pageRoutes";

export interface IBook extends IBaseEntity {
  authorId: number;
  coverId: number;
  title: string;
  basePrice: number;
  publishDate: Date;
}

export const domain = PAGE_ROUTES.ADMIN.BOOKS.endpoint;
export const baseUri = `${domain}?includeAuthor=true&includeCoverAndArtists=true`;