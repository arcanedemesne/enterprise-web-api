export interface IAuthor {
  id: number;
  firstName: string;
  lastName: string;
  fullName: string;
  books: any[];
}

export const domain = "authors";
export const baseUri = `${domain}?includeBooks=true`;