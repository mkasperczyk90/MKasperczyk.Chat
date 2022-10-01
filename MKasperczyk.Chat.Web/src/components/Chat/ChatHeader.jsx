import React, { useRef, useEffect } from "react";
import "./ChatHeader.css";
import { useNavigate } from "react-router-dom";
import { useAuthContext } from "../../providers/AuthProvider";

export default function ChatHeader() {
  const auth = useAuthContext();
  const avatarElement = useRef(null);
  const navigate = useNavigate();

  useEffect(() => {
    if (auth.user && auth.user.avatar) {
      // TODO: should use link, not byte data, too many data in requests
      avatarElement.current.src = `data:image/jpeg;base64,${auth.user.avatar}`;
    } else {
      avatarElement.current.src = process.env.PUBLIC_URL + "/images/avatar.png";
    }
  }, [auth.user]);

  return (
    <div className="chat--header">
      <span className="chat--header--title">Welcom in Chat!</span>
      <div className="chat--header--info">
        <img className="chat--header--image" alt="avatar" ref={avatarElement} />
        <span>{auth.user === null ? "" : auth.user.userName}</span>
        <span
          className="chat-header--action"
          onClick={() => navigate("/avatar")}
        >
          Set Avatar
        </span>
        <span
          className="chat-header--action"
          onClick={() => navigate("/logout")}
        >
          Logout
        </span>
      </div>
    </div>
  );
}
