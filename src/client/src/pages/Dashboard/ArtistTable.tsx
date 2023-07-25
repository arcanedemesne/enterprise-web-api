import { useEffect, useState } from "react";

import { GET } from "../../utilities/httpRequest";
import SimpleDataTable from "../../components/SimpleDataTable";

let initialized = false;
const ArtistTable = () => {
  const [artists, setArtists] = useState([]);
  const [pagination, setPagination] = useState({});

  useEffect(() => {
    async function getArtists() {
      const response: any = (
        await GET({ endpoint: "artists?IncludeCoversWithBookAndAuthor=true" })
      );
      const { headers, data } = response;
      const paginationHeaders = JSON.parse(headers.get("x-pagination"));
      setPagination(paginationHeaders);
      setArtists(data);
    }

    if (!initialized) {
      getArtists();
      initialized = true;
    }
  }, []);

  const rows = artists.map((x: any) => {
    return {
      id: x.id,
      values: [x.id, `${x.firstName} ${x.lastName}`, x.covers.length],
    };
  });
  const tableData = {
    headers: [
      {
        width: 50,
        label: "Id",
      },
      {
        label: "Artist",
      },
      {
        label: "Cover Art Count",
      },
    ],
    rows,
  };
  
  return (
    <SimpleDataTable
      title="Artists"
      caption="This table contains Artists"
      data={tableData}
      pagination={pagination}
      borderAxis="xBetween" hoverRow
    />
  );
};

export default ArtistTable;
