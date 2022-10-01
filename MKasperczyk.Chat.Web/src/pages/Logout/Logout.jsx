import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuthContext } from "../../providers/AuthProvider"
import { useSignalRContext } from "../../providers/SignalRProvider";

export default function Logout() {
  const chatLocalStorageKey = "chat";
  const navigate = useNavigate();
  const auth = useAuthContext();
  const connection = useSignalRContext();
  
  useEffect(() => {
    auth.signOut()
    connection.stop()
    localStorage.setItem(chatLocalStorageKey, JSON.stringify(null));
    navigate("/Login");
  }, [auth, navigate, connection]);

  return (<></>);
}
