import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { HubConnectionBuilder } from "@microsoft/signalr";
import "./Chat.css";
import axios from "axios";
import {
  sendMessageUrl,
  messagesUrl,
  chatHubUrl,
} from "../../helpers/ApiRequests";
import ChatSendingBox from "../../components/Chat/ChatSendingBox";
import ChatMessage from "../../components/Chat/ChatMessage";
import ChatMessageInfo from "../../components/Chat/ChatMessageInfo";
import ChatContact from "../../components/Chat/ChatContact";
import ChatHeader from "../../components/Chat/ChatHeader";
import { useAuthContext } from "../../providers/AuthProvider";

export default function Chat() {
  const chatLocalStorageKey = "chat";
  const navigate = useNavigate();
  const [connection, setConnection] = useState(null);
  const auth = useAuthContext();
  const [messages, setMessages] = useState([]);
  const [currentChat, setCurrentChat] = React.useState(undefined);

  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
      .withUrl(chatHubUrl, { accessTokenFactory: () => auth.user.token })
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);
  }, [auth.user]);

  useEffect(() => {
    const setData = async () => {
      if (!auth.user) {
        navigate("/login");
      } else {
        var actualChatChannel = getStorageValue(chatLocalStorageKey);
        if (actualChatChannel != null) {
          changeChat(actualChatChannel);
        }
      }
    };
    setData();
  }, [auth, navigate]);

  useEffect(() => {
    if (connection) {
      connection
        .start()
        .then(() => {
          connection.on("ReceiveMessage", (messageData) => {
            var actualChatChannel = getStorageValue(chatLocalStorageKey);

            if (actualChatChannel.id === messageData.sender) {
              setMessages((prvMessages) => [
                ...prvMessages,
                {
                  type: "recieved",
                  sendAt: new Date(),
                  message: messageData.message,
                },
              ]);
            }
          });
        })
        .catch((e) => console.log("Connection failed: ", e));
    }
  }, [connection]);

  useEffect(() => {
    const getMessagesFromApi = async () => {
      var user = auth.user;
      const response = await axios.get(messagesUrl, {
        params: {
          senderId: user.id,
          receiverId: currentChat.id,
        },
      });
      setMessages(response.data);
    };

    if (currentChat !== undefined) {
      getMessagesFromApi();
    }
  }, [currentChat, auth.user]);

  const changeChat = async (chat) => {
    localStorage.setItem(chatLocalStorageKey, JSON.stringify(chat));
    setCurrentChat(chat);
  };

  const getStorageValue = (key) => {
    const saved = localStorage.getItem(key);
    const initial = JSON.parse(saved);
    return initial;
  };

  const handleSendMessage = async (msg) => {
    await axios.post(sendMessageUrl, {
      Sender: auth.user.id,
      Message: msg,
      Recipients: [currentChat.id],
    });

    await connection.send("SendMessage", currentChat.id, msg);

    setMessages((prvMessages) => [
      ...prvMessages,
      {
        type: "sended",
        sendAt: new Date(),
        message: msg,
      },
    ]);
  };

  return (
    <div className="card chat-app">
      <ChatHeader />
      <ChatContact changeChat={changeChat} connection={connection} />
      {(currentChat && Object.keys(currentChat).length > 0) ? (
        <div className="chat">
          <ChatMessageInfo currentChat={currentChat} />
          <ChatMessage
            currentChat={currentChat}
            user={auth.user}
            messages={messages}
          />
          <ChatSendingBox
            handleSendMessage={handleSendMessage}
            currentChat={currentChat}
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