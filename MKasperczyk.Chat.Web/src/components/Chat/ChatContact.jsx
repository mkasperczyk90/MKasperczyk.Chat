import React, { useEffect } from "react";
import "./ChatContact.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { formatDistanceToNow } from "date-fns";
import { useAuthContext } from "../../providers/AuthProvider";
import axios from "axios";
import { usersUrl } from "../../helpers/ApiRequests";

export default function ChatContact({ changeChat, connection }) {
  const auth = useAuthContext();
  const [searchQuery, setSearchQuery] = React.useState("");
  const [contacts, setContacts] = React.useState([]);
  const [contactResult, setContactResult] = React.useState([]);

  useEffect(() => {
    const setData = async () => {
      if (auth.user) {
        const data = await axios.get(`${usersUrl}/${auth.user.id}`);
        setContacts(data.data);
      }
    };
    setData();
  }, [auth.user]);

  //https://www.npmjs.com/package/react-signalr
  useEffect(() => {
    if (connection !== null) {
      connection.on("statusChanged", (data) => {
        setContactResult((previousContacts) =>
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

  useEffect(() => {
    var contactToDisplay = [];
    if (searchQuery === "") contactToDisplay = contacts;
    else {
      contactToDisplay = contacts.filter((contact) =>
        contact.userName.includes(searchQuery)
      );
    }

    setContactResult(contactToDisplay);
  }, [contacts, searchQuery]);

  const handleChange = (event) => {
    setSearchQuery(event.target.value);
  };

  const changeCurrentChat = (contact) => {
    changeChat(contact);
  };

  const getLeftOnlineText = (contact) => {
    if (contact.currentlyLogin) return "Online";
    else if (contact.lastConnection === null) return "Never Login";
    else {
      return "left " + formatDistanceToNow(Date.parse(contact.lastConnection));
    }
  };

  const contactsElement = contactResult.map((contact) => {
    return (
      <li
        key={contact.id}
        className="clearfix chat-contact--element"
        onClick={() => changeCurrentChat(contact)}
      >
        <img
          className="chat-contact--image"
          // TODO: should use link, not byte data, too many data in requests
          src={
            contact.avatar
              ? `data:image/jpeg;base64,${contact.avatar}`
              : process.env.PUBLIC_URL + "/images/avatar.png"
          }
          alt="avatar"
        />
        <div className="chat-contact--about">
          <div className="chat-contact--name">{contact.userName}</div>
          <div
            className={
              "chat-contact--status" + (contact.currentlyLogin ? " online" : "")
            }
          >
            <FontAwesomeIcon icon="fa-solid fa-circle" />{" "}
            {getLeftOnlineText(contact)}
          </div>
        </div>
      </li>
    );
  });

  return (
    <div id="plist" className="chat-contact">
      <div className="input-group">
        <div className="input-group-prepend">
          <span className="chat-contact--icon input-group-text">
            <FontAwesomeIcon icon="fa-solid fa-search" size="lg" />
          </span>
        </div>
        <input
          type="text"
          className="chat-contact--input form-control"
          placeholder="Search..."
          value={searchQuery}
          onChange={handleChange}
        />
      </div>
      <ul className="chat-contact--list list-unstyled mt-2">
        {contactsElement}
      </ul>
    </div>
  );
}
