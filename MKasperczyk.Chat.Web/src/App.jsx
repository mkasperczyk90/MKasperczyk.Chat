import "./App.css";
import Chat from "./pages/Chat/Chat";
import Login from "./pages/Login/Login";
import Avatar from "./pages/Avatar/Avatar";
import Logout from "./pages/Logout/Logout";
import Register from "./pages/Register/Register";
import { library } from "@fortawesome/fontawesome-svg-core";
import {
  faPaperPlane,
  faSearch,
  faCircle,
} from "@fortawesome/free-solid-svg-icons";
import { Routes, Route } from "react-router-dom";
import { AuthProvider } from "./providers/AuthProvider";

library.add(faPaperPlane, faSearch, faCircle);

function App() {
  return (
    <div className="App container">
      <AuthProvider>
        <Routes>
          <Route path="/avatar" element={<Avatar />} />
          <Route path="/login" element={<Login />} />
          <Route path="/logout" element={<Logout />} />
          <Route path="/register" element={<Register />} />
          <Route path="/" element={<Chat />} />
        </Routes>
      </AuthProvider>
    </div>
  );
}

export default App;
