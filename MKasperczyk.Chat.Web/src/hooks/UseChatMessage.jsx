import React, { useEffect, useState } from "react";
import axios from "axios";
import { sendMessageUrl, messagesUrl } from "../helpers/ApiRequests";

export const useChatMessage = (connection) => {
  const chatLocalStorageKey = "chat";
  const [messages, setMessages] = useState([]);
  const [userId, setUserId] = React.useState(null);
  const [currentChat, setCurrentChat] = React.useState(undefined);

  useEffect(() => {
    const fetchMessagesAsync = async () => {
      const response = await axios.get(messagesUrl, {
        params: {
          senderId: userId,
          receiverId: currentChat.id,
        },
      });
      setMessages(response.data);
    };

    if (currentChat && userId) {
      fetchMessagesAsync();
    }
  }, [currentChat, userId]);

  useEffect(() => {
    var actualChatChannel = getStorageValue(chatLocalStorageKey);
    if (actualChatChannel != null) {
      changeChat(actualChatChannel);
    }
  }, [])

  useEffect(() => {
    if (connection != null) {
      connection.on("ReceiveMessage", (messageData) => {
        var actualChatChannel = getStorageValue(chatLocalStorageKey);

        if (actualChatChannel.id === messageData.sender) {
          setMessages((prvMessages) => [
            ...prvMessages,
            {
              type: "received",
              sendAt: new Date(),
              message: messageData.message,
            },
          ]);
        }
      });
    }
  }, [connection]);

  const sendMessage = async (msg) => {
    await axios.post(sendMessageUrl, {
      Sender: userId,
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

  const changeChat = async (chat) => {
    localStorage.setItem(chatLocalStorageKey, JSON.stringify(chat));
    setCurrentChat(chat);
  };
  
  const getStorageValue = (key) => {
    const saved = localStorage.getItem(key);
    const initial = JSON.parse(saved);
    return initial;
  };

  return { messages, currentChat, setUserId, setCurrentChat, sendMessage };
};
