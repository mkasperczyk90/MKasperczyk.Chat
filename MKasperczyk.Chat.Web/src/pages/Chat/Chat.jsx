import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import "./Chat.css";
import ChatSendingBox from "../../components/Chat/ChatSendingBox";
import ChatMessage from "../../components/Chat/ChatMessage";
import ChatMessageInfo from "../../components/Chat/ChatMessageInfo";
import ChatContact from "../../components/Chat/ChatContact";
import ChatHeader from "../../components/Chat/ChatHeader";
import { useAuthContext } from "../../providers/AuthProvider";
import { useSignalRContext } from "../../providers/SignalRProvider";
import { useChatMessage } from "../../hooks/UseChatMessage"

export default function Chat() {
  const auth = useAuthContext();
  const connection = useSignalRContext();
  const chatMessage = useChatMessage(connection);
  const navigate = useNavigate();

  useEffect(() => {
    const setData = async () => {
      if (!auth.user) {
        navigate("/login");
      } else {
        chatMessage.setUserId(auth.user.id)
      }
    };
    setData();
  }, [chatMessage, auth, navigate]);

  const changeChat = async (chat) => {
    chatMessage.setCurrentChat(chat)
  };

  const handleSendMessage = async (msg) => {
    chatMessage.sendMessage(msg)
  };

  return (
    <div className="card chat-app">
      <ChatHeader />
      <ChatContact changeChat={changeChat} connection={connection} />
      {chatMessage.currentChat && Object.keys(chatMessage.currentChat).length > 0 ? (
        <div className="chat">
          <ChatMessageInfo currentChat={chatMessage.currentChat} />
          <ChatMessage
            currentChat={chatMessage.currentChat}
            user={auth.user}
            messages={chatMessage.messages}
          />
          <ChatSendingBox
            handleSendMessage={handleSendMessage}
            currentChat={chatMessage.currentChat}
            user={auth.user}
          />
        </div>
      ) : (
        <div className="chat chat--empty">
          <span>Select user to chat with</span>
        </div>
      )}
    </div>
  );
}
