import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuthContext } from "../../providers/AuthProvider"

export default function Logout() {
  const chatLocalStorageKey = "chat";
  const navigate = useNavigate();
  const auth = useAuthContext();
  
  useEffect(() => {
    auth.signOut()
    navigate("/Login");
    localStorage.setItem(chatLocalStorageKey, JSON.stringify({}));
  }, [auth, navigate]);

  return (<></>);
}
