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
import { useAuthContext } from "./providers/AuthProvider";
import { SignalRProvider } from "./providers/SignalRProvider";


library.add(faPaperPlane, faSearch, faCircle);

function App() {
  const auth = useAuthContext();
  return (
    <div className="App container">
      <SignalRProvider user={auth.user}>
        <Routes>
          <Route path="/avatar" element={<Avatar />} />
          <Route path="/login" element={<Login />} />
          <Route path="/logout" element={<Logout />} />
          <Route path="/register" element={<Register />} />
          <Route path="/" element={<Chat />} />
        </Routes>
      </SignalRProvider>
    </div>
  );
}

export default App;
