import { useEffect, useState } from "react";

import { GET } from "../../utilities/httpRequest";
import SimpleDataTable from "../../components/SimpleDataTable";

let initialized = false;
const BookTable = () => {
  const [books, setBooks] = useState([]);
  const [pagination, setPagination] = useState({});

  useEffect(() => {
    async function getBooks() {
      const response: any = (
        await GET({
          endpoint: "books?includeAuthor=true&includeCoverAndArtists=true",
        })
      );
      const { headers, data } = response;
      const paginationHeaders = JSON.parse(headers.get("x-pagination"));
      setPagination(paginationHeaders);
      setBooks(data);
    }

    if (!initialized) {
      getBooks();
      initialized = true;
    }
  }, []);

  const rows = books.map((x: any) => {
    return {
      id: x.id,
      values: [
        x.id,
        x.title,
        x.author.fullName,
        x.basePrice,
        x.coverId ? "true" : "false",
        x.cover?.artists.length ?? "None",
        new Date(x.publishDate).toDateString(),
      ],
    };
  });
  const tableData = {
    headers: [
      {
        width: 50,
        label: "Id",
      },
      {
        label: "Book Title",
      },
      {
        label: "Author",
      },
      {
        label: "Price",
      },
      {
        label: "Has Cover Art",
      },
      {
        label: "Cover Artist Count",
      },
      {
        label: "Publish Date",
      },
    ],
    rows,
  };

  return (
    <SimpleDataTable
      title="Books"
      caption="This table contains Books"
      data={tableData}
      pagination={pagination}
      borderAxis="xBetween" hoverRow
    />
  );
};

export default BookTable;
