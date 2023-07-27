
export interface IBook {
  id?: number;
  authorId: number;
  coverId: number;
  title: string;
  basePrice: number;
  publishDate: Date;
}

export const domain = "books";
export const baseUri = `${domain}?includeAuthor=true&includeCoverAndArtists=true`;