import React, { createContext, useContext, useState, useEffect } from "react";
import { HubConnectionBuilder } from "@microsoft/signalr";
import { chatHubUrl } from "../helpers/ApiRequests";
const SignalRContext = createContext();

const useSignalRContext = () => useContext(SignalRContext);

const SignalRProvider = ({ children, user }) => {
  const [connection, setConnection] = useState(null);
  
  useEffect(() => {
    
    const setConnectionAsync = async () => {
        const newConnection = new HubConnectionBuilder()
          .withUrl(chatHubUrl, { accessTokenFactory: () => user.token })
          .withAutomaticReconnect()
          .build();
      
        newConnection
          .start()
          .then(() => console.log("Chat websocket connected."))
          .catch((e) => console.error("Chat websocket failed", e));
      
        setConnection(newConnection);
    };
    if(user && user.token) {
      setConnectionAsync();
    }
  }, [user]);

  return (
    <SignalRContext.Provider value={connection}>
      {children}
    </SignalRContext.Provider>
  );
};

export { useSignalRContext, SignalRProvider };
