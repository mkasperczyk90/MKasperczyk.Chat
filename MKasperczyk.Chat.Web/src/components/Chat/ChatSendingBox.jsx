import React, { useState } from "react";
import "./ChatSendingBox.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

export default function ChatSendingBox({ handleSendMessage }) {
  const [message, setMessage] = useState("");

  const sendMessage = () => {
    if (message.length > 0) {
      handleSendMessage(message);
      setMessage("");
    }
  };

  return (
    <div className="chat-sending-box clearfix">
      <form onSubmit={(event) => {event.preventDefault(); sendMessage();}}>
        <div className="input-group mb-0">
          <input
            type="text"
            className="chat-sending-box--input form-control"
            placeholder="Enter message ..."
            value={message}
            onChange={(event) => setMessage(event.target.value)}
          />
          <span
            className="chat-contact--icon input-group-text"
            onClick={(event) => { event.preventDefault(); sendMessage(); }}
          >
            <FontAwesomeIcon icon="fa-solid fa-paper-plane" />
          </span>
        </div>
      </form>
    </div>
  );
}
