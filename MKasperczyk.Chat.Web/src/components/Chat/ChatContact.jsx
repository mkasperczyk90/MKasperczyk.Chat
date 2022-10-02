import React, { useEffect } from "react";
import "./ChatContact.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { formatDistanceToNow } from "date-fns";
import { useAuthContext } from "../../providers/AuthProvider";
import { useSearchableContacts } from "../../hooks/UseSearchableContacts"

export default function ChatContact({ changeChat, connection }) {
  const auth = useAuthContext();
  const searchableResult = useSearchableContacts(connection);

  useEffect(() => {
    searchableResult.setUserId(auth.user.id)
  }, [searchableResult, auth.user.id]);

  const handleChange = (event) => {
    searchableResult.setSearchQuery(event.target.value);
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

  const contactsElement = searchableResult.dataResult.map((contact) => {
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
          value={searchableResult.searchQuery}
          onChange={handleChange}
        />
      </div>
      <ul className="chat-contact--list list-unstyled mt-2">
        {contactsElement}
      </ul>
    </div>
  );
}
