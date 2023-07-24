import Table from "@mui/joy/Table";

const artists = [
  {
    "firstName": "Dee",
    "lastName": "Bell",
    "covers": [
      {
        "designIdeas": "How about a left hand in the dark?",
        "digitalOnly": false,
        "bookId": 3,
        "book": {
          "title": "The Left Hand of Darkness",
          "publishDate": "1969-03-01T00:00:00Z",
          "basePrice": 0,
          "authorId": 3,
          "author": {
            "firstName": "Sofia",
            "lastName": "Segovia",
            "fullName": "Sofia Segovia",
            "books": [],
            "id": 3
          },
          "coverId": 1,
          "id": 3
        },
        "artists": [],
        "id": 1
      },
      {
        "designIdeas": "Should we put a clock?",
        "digitalOnly": true,
        "bookId": 2,
        "book": {
          "title": "A Tale For the Time Being",
          "publishDate": "2013-12-31T00:00:00Z",
          "basePrice": 0,
          "authorId": 2,
          "author": {
            "firstName": "Ruth",
            "lastName": "Ozeki",
            "fullName": "Ruth Ozeki",
            "books": [],
            "id": 2
          },
          "coverId": 2,
          "id": 2
        },
        "artists": [],
        "id": 2
      }
    ],
    "id": 2
  },
  {
    "firstName": "Katharine",
    "lastName": "Kuharic",
    "covers": [
      {
        "designIdeas": "Should we put a clock?",
        "digitalOnly": true,
        "bookId": 2,
        "book": {
          "title": "A Tale For the Time Being",
          "publishDate": "2013-12-31T00:00:00Z",
          "basePrice": 0,
          "authorId": 2,
          "author": {
            "firstName": "Ruth",
            "lastName": "Ozeki",
            "fullName": "Ruth Ozeki",
            "books": [],
            "id": 2
          },
          "coverId": 2,
          "id": 2
        },
        "artists": [],
        "id": 2
      },
      {
        "designIdeas": "A big ear in the clouds?",
        "digitalOnly": false,
        "bookId": 1,
        "book": {
          "title": "In God's Ear",
          "publishDate": "1989-03-01T00:00:00Z",
          "basePrice": 0,
          "authorId": 1,
          "author": {
            "firstName": "Rhoda",
            "lastName": "Lerman",
            "fullName": "Rhoda Lerman",
            "books": [],
            "id": 1
          },
          "coverId": 3,
          "id": 1
        },
        "artists": [],
        "id": 3
      }
    ],
    "id": 3
  },
  {
    "firstName": "Pablo",
    "lastName": "Picasso",
    "covers": [
      {
        "designIdeas": "How about a left hand in the dark?",
        "digitalOnly": false,
        "bookId": 3,
        "book": {
          "title": "The Left Hand of Darkness",
          "publishDate": "1969-03-01T00:00:00Z",
          "basePrice": 0,
          "authorId": 3,
          "author": {
            "firstName": "Sofia",
            "lastName": "Segovia",
            "fullName": "Sofia Segovia",
            "books": [],
            "id": 3
          },
          "coverId": 1,
          "id": 3
        },
        "artists": [],
        "id": 1
      }
    ],
    "id": 1
  }
];

const ArtistTable = () => {
  return (
    <Table aria-label="basic table">
      <thead>
        <tr>
          <th style={{ width: 50 }}>Id</th>
          <th>Artist Name</th>
          <th>Cover Art Count</th>
        </tr>
      </thead>
      <tbody>
        {artists.map(artist => (
          <tr>
          <td>{artist.id}</td>
          <td>{artist.firstName + " " + artist.lastName}</td>
          <td>{artist.covers.length}</td>
        </tr>
        ))}
      </tbody>
    </Table>
  );
};

export default ArtistTable;
