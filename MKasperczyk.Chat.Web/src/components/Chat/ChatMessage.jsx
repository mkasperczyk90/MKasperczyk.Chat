import React, { useRef, useEffect } from "react";
import "./ChatMessage.css";
import { formatDistanceToNow } from "date-fns";

export default function ChatMessage({ messages }) {
  const historyRef = useRef(null);

  const historyElement = messages.map((message) => {
    return (
      <li className="chat-messages--element clearfix" key={message.sendAt}>
        <div
          className={
            message.type === "recieved"
              ? "chat-messages--element--details text-right"
              : "chat-messages--element--details"
          }
        >
          <span className="chat-messages--element--time">
            {formatDistanceToNow(
              Date.parse(message.sendAt),
              "MMMM do, yyyy H:mma"
            )}
          </span>
        </div>
        <div
          className={
            message.type === "recieved"
              ? "chat-messages--element--message other-message float-right"
              : "chat-messages--element--message my-message"
          }
        >
          {message.message}
        </div>
      </li>
    );
  });

  const scrollToBottom = () => {
    historyRef.current.scrollTop = historyRef.current.scrollHeight;
  };

  useEffect(() => {
    scrollToBottom();
  }, [messages]);

  return (
    <div className="chat-messages" ref={historyRef}>
      <ul className="m-b-0">
        {messages.length === 0 && <div></div>}
        {messages.length > 0 && historyElement}
      </ul>
    </div>
  );
}
