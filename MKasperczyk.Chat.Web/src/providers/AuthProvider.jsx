import React, { createContext, useContext } from "react";
import { useAuth } from "../hooks/UseAuth";
import axios from "axios";

const AuthContext = createContext();

const useAuthContext = () => useContext(AuthContext);

const AuthProvider = ({ children }) => {
  const auth = useAuth();

  if (auth.user) {
    axios.defaults.headers.common[
      "Authorization"
    ] = `Bearer ${auth.user.token}`;
  }

  return <AuthContext.Provider value={auth}>{children}</AuthContext.Provider>;
};

export { useAuthContext, AuthProvider };
