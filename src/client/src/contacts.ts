import { Contact } from "./routes/contact";

let data: Contact[] = [
  {
    id: 1,
    first: "Jennifer",
    last: "Allen",
    avatar: "https://placekitten.com/g/200/200",
    twitter: "nella.refinnej",
    notes: "I wish to understand the nature of reality.",
    favorite: true,
  },
  {
    id: 2,
    first: "Seanna",
    last: "Makepeace",
    avatar: "https://placekitten.com/g/200/200",
    twitter: "ecaepekam.annaes",
    notes: "I am the nature of reality.",
    favorite: true,
  },
];

const createBlankContact = (id: number): Contact => {
  return {
    id,
    first: "",
    last: "",
    avatar: "",
    twitter: "",
    notes: "",
    favorite: false,
  };
}

export const getContacts = (q: string): Contact[] => {
  return data.filter(c => c.first?.includes(q) || c.last?.includes(q));
};

export const getContact = ({ params }: any): Contact | undefined => {
  console.info(params);
  return data.find(c => c.id === Number(params.contactId));
};

export const updateContact = (id: number, updates: Contact) => {
  data.forEach(c => {
    if (c.id === Number(id)) {
      c.first = updates.first;
      c.last = updates.last;
      c.avatar = updates.avatar;
      c.twitter = updates.twitter;
      c.notes = updates.notes;
    }
  });
};

export const updateContactFavorite = (id: number, { favorite }: { favorite: boolean }) => {
  const contact = data.find(c => c.id === Number(id));
  if (contact) contact.favorite = favorite;
  return contact;
};

export const createContact = () => {
  const getId = (): number => {
    return data.reduce(function(max, obj) {
      return obj.id > max.id? obj : max;
    }).id + 1;
  };

  const contact = createBlankContact(getId()); 
  data.push(contact);
  return contact;
};

export const deleteContact = (id: number) => {
  data = data.filter(c => c.id !== Number(id));
};
