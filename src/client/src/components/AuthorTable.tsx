import Table from "@mui/joy/Table";

const authors = [
  {
    firstName: "Isabelle",
    lastName: "Allende",
    fullName: "Isabelle Allende",
    books: [],
    id: 6,
  },
  {
    firstName: "Hugh",
    lastName: "Howey",
    fullName: "Hugh Howey",
    books: [],
    id: 5,
  },
  {
    firstName: "Ursula K.",
    lastName: "LeGuin",
    fullName: "Ursula K. LeGuin",
    books: [],
    id: 4,
  },
  {
    firstName: "Rhoda",
    lastName: "Lerman",
    fullName: "Rhoda Lerman",
    books: [
      {
        title: "In God's Ear",
        publishDate: "1989-03-01T00:00:00Z",
        basePrice: 0,
        authorId: 1,
        coverId: 3,
        cover: {
          designIdeas: "A big ear in the clouds?",
          digitalOnly: false,
          bookId: 1,
          artists: [
            {
              firstName: "Katharine",
              lastName: "Kuharic",
              covers: [],
              id: 3,
            },
          ],
          id: 3,
        },
        id: 1,
      },
    ],
    id: 1,
  },
  {
    firstName: "Ruth",
    lastName: "Ozeki",
    fullName: "Ruth Ozeki",
    books: [
      {
        title: "A Tale For the Time Being",
        publishDate: "2013-12-31T00:00:00Z",
        basePrice: 0,
        authorId: 2,
        coverId: 2,
        cover: {
          designIdeas: "Should we put a clock?",
          digitalOnly: true,
          bookId: 2,
          artists: [
            {
              firstName: "Dee",
              lastName: "Bell",
              covers: [],
              id: 2,
            },
            {
              firstName: "Katharine",
              lastName: "Kuharic",
              covers: [],
              id: 3,
            },
          ],
          id: 2,
        },
        id: 2,
      },
    ],
    id: 2,
  },
  {
    firstName: "Sofia",
    lastName: "Segovia",
    fullName: "Sofia Segovia",
    books: [
      {
        title: "The Left Hand of Darkness",
        publishDate: "1969-03-01T00:00:00Z",
        basePrice: 0,
        authorId: 3,
        coverId: 1,
        cover: {
          designIdeas: "How about a left hand in the dark?",
          digitalOnly: false,
          bookId: 3,
          artists: [
            {
              firstName: "Pablo",
              lastName: "Picasso",
              covers: [],
              id: 1,
            },
            {
              firstName: "Dee",
              lastName: "Bell",
              covers: [],
              id: 2,
            },
          ],
          id: 1,
        },
        id: 3,
      },
    ],
    id: 3,
  },
];

const AuthorTable = () => {
  return (
    <Table aria-label="basic table">
      <thead>
        <tr>
          <th style={{ width: 50 }}>Id</th>
          <th>Author Name</th>
          <th>Book Count</th>
        </tr>
      </thead>
      <tbody>
        {authors.map((author) => (
          <tr key={author.id}>
            <td>{author.id}</td>
            <td>{author.fullName}</td>
            <td>{author.books.length}</td>
          </tr>
        ))}
      </tbody>
    </Table>
  );
};

export default AuthorTable;
