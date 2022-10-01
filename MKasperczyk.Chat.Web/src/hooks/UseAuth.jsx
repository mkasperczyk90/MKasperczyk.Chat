import axios from "axios";
import { toast } from "react-toastify";
import { registerUrl, loginUrl } from "../helpers/ApiRequests";
import { useLocalStorage } from "../hooks/UseLocalStorage";

export const useAuth = () => {
  const [user, setUser] = useLocalStorage("user", null);

  const toastOption = {
    position: "bottom-right",
    pauseOnHover: true,
    draggable: true,
  };

  const signIn = async ({ userName, password }) => {
    try {
      if (validateLoginRequest({ userName, password })) {
        let loginResult = await axios.post(loginUrl, {
          userName,
          password,
        });

        if (!loginResult.data.success) {
          toast.error(loginResult.data.message, toastOption);
          return null;
        } else {
          var userData = {
            id: loginResult.data.id,
            userName: loginResult.data.user,
            token: loginResult.data.token,
            avatar: loginResult.data.avatar,
          };
          setUser(userData);
          return userData;
        }
      }
    } catch (err) {
      console.error(err);
      return null;
    }
  };

  const signUp = async ({ userName, password, confirmPassword }) => {
    try {
      if (validateRegisterRequest(userName, password, confirmPassword)) {
        let registerResult = await axios.post(registerUrl, {
          userName,
          password,
        });

        if (!registerResult.data.success) {
          toast.error(registerResult.data.message, toastOption);
          return null;
        } else {
          var userData = {
            id: registerResult.data.id,
            userName: registerResult.data.user,
            token: registerResult.data.token,
            avatar: registerResult.data.avatar,
          };
          setUser(userData);
          return userData;
        }
      }
    } catch (err) {
      console.error(err);
      return null;
    }
  };

  const signOut = () => {
    setUser(null);
  };

  const validateRegisterRequest = (userName, password, confirmPassword) => {
    if (password !== confirmPassword) {
      toast.error(
        "Password and Confirmation password should be the same.",
        toastOption
      );
      return false;
    } else if (userName.length < 3) {
      toast.error("Username should be greater then 3 characters", toastOption);
      return false;
    } else if (password.length < 7) {
      toast.error(
        "Password should be equal or greate then 7 characters",
        toastOption
      );
      return false;
    }
    return true;
  };

  const validateLoginRequest = ({ userName, password }) => {
    if (userName === "" || password === "") {
      toast.error("Username and password should not be empty", toastOption);
      return false;
    }
    return true;
  };

  return { user, signIn, signUp, signOut };
};
