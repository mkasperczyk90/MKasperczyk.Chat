import React, { useEffect } from "react";
import axios from "axios";
import { usersUrl } from "../helpers/ApiRequests";

export const useSearchableContacts = (connection) => {
  const [userId, setUserId] = React.useState(null);
  const [searchQuery, setSearchQuery] = React.useState("");
  const [data, setData] = React.useState([]);
  const [dataResult, setDataResult] = React.useState([]);

  useEffect(() => {
    const fetchDataAsync = async () => {
      const data = await axios.get(`${usersUrl}/${userId}`);
      setData(data.data);
    };
    if(userId !== null) fetchDataAsync();
  }, [userId]);
  
  useEffect(() => {
    var dataToDisplay = [];
    if (searchQuery === "") dataToDisplay = data;
    else {
      dataToDisplay = data.filter((contact) =>
        contact.userName.toLowerCase().includes(searchQuery.toLowerCase())
      );
    }

    setDataResult(dataToDisplay);
  }, [data, searchQuery]);

  //https://www.npmjs.com/package/react-signalr
  useEffect(() => {
    if (connection !== null) {
      connection.on("statusChanged", (data) => {
        setDataResult((previousContacts) =>
          previousContacts.map((contact) =>
            contact.id === data.user
              ? {
                  ...contact,
                  currentlyLogin: data.online,
                  lastConnection: data.when,
                }
              : contact
          )
        );
      });
    }
  }, [connection]);

  return { dataResult, data, searchQuery, setUserId, setSearchQuery };
};
