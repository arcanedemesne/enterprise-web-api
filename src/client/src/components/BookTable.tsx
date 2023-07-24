import Table from "@mui/joy/Table";

const books = [
  {
    title: "A Tale For the Time Being",
    publishDate: "2013-12-31T00:00:00Z",
    basePrice: 0,
    authorId: 2,
    author: {
      firstName: "Ruth",
      lastName: "Ozeki",
      fullName: "Ruth Ozeki",
      books: [],
      id: 2,
    },
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
  {
    title: "In God's Ear",
    publishDate: "1989-03-01T00:00:00Z",
    basePrice: 0,
    authorId: 1,
    author: {
      firstName: "Rhoda",
      lastName: "Lerman",
      fullName: "Rhoda Lerman",
      books: [],
      id: 1,
    },
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
  {
    title: "The Left Hand of Darkness",
    publishDate: "1969-03-01T00:00:00Z",
    basePrice: 0,
    authorId: 3,
    author: {
      firstName: "Sofia",
      lastName: "Segovia",
      fullName: "Sofia Segovia",
      books: [],
      id: 3,
    },
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
];

const BookTable = () => {
  return (
    <Table aria-label="basic table">
      <thead>
        <tr>
          <th style={{ width: 50 }}>Id</th>
          <th>Book Title</th>
          <th>Author</th>
          <th>Price</th>
          <th>Has Cover Art</th>
          <th>Cover Artist Count</th>
          <th>Publish Date</th>
        </tr>
      </thead>
      <tbody>
        {books.map((book) => (
          <tr key={book.id}>
            <td>{book.id}</td>
            <td>{book.title}</td>
            <td>{book.author.fullName}</td>
            <td>{book.basePrice}</td>
            <td>{book.coverId ? "true" : "false"}</td>
            <td>{book.cover.artists.length ?? "None"}</td>
            <td>{new Date(book.publishDate).toDateString()}</td>
          </tr>
        ))}
      </tbody>
    </Table>
  );
};

export default BookTable;
