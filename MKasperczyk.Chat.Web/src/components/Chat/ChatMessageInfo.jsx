import React, { useRef, useEffect } from "react";
import "./ChatMessageInfo.css";
import { formatDistanceToNow } from "date-fns";

export default function ChatMessageInfo({ currentChat }) {
  const avatarElement = useRef(null);

  useEffect(() => {
    if (currentChat.avatar) {
      // TODO: should use link, not byte data, too many data in requests
      avatarElement.current.src = `data:image/jpeg;base64,${currentChat.avatar}`;
    } else {
      avatarElement.current.src = process.env.PUBLIC_URL + "/images/avatar.png";
    }
  }, [currentChat]);

  const getLeftOnlineText = (contact) => {
    if (contact.currentlyLogin) return "Online";
    else if (contact.lastConnection === null) return "Never Login";
    else {
      return "left " + formatDistanceToNow(Date.parse(contact.lastConnection));
    }
  };

  return (
    <div className="chat-info clearfix">
      <div className="row">
        <div className="col-lg-6">
          <span>
            <img alt="avatar" ref={avatarElement} />
          </span>
          <div className="chat-info--about">
            <h6 className="m-b-0">{currentChat && currentChat.userName}</h6>
            <small>Last seen: {getLeftOnlineText(currentChat)}</small>
          </div>
        </div>
      </div>
    </div>
  );
}
