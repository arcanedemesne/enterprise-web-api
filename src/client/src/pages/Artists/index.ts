export interface IArtist {
  id?: number;
  firstName: string;
  lastName: string;
  fullName: string;
  covers: any[];
}

export const domain = "artists";
export const baseUri = `${domain}?IncludeCoversWithBookAndAuthor=true`;