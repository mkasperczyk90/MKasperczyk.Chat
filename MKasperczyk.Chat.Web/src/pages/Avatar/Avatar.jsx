import React, { useEffect, useState } from "react";
import { toast, ToastContainer } from "react-toastify";
import { useNavigate } from "react-router-dom";
import FileUploader from "../../components/FileUploader";
import { useAuthContext } from "../../providers/AuthProvider";
import axios from "axios";
import { avatarUrl } from "../../helpers/ApiRequests";

export default function Avatar() {
  const navigate = useNavigate();
  const auth = useAuthContext();
  const [selectedFile, setSelectedFile] = useState(null);
  
  const toastOption = {
    position: "bottom-right",
    pauseOnHover: true,
    draggable: true,
  };

  useEffect(() => {
    const setData = async () => {
      if (!auth.user) {
        navigate("/login");
      }
    };
    setData();
  }, [auth, navigate]);

  const handleSubmit = async (event) => {
    event.preventDefault();

    const formData = new FormData();
    formData.append("file", selectedFile);

    axios
      .post(avatarUrl, formData)
      .then((res) => {
        navigate("/");
      })
      .catch((err) => toast.error("File Upload Error", toastOption));
  };

  return (
    <div className="card">
      <div className="register container">
        <form onSubmit={handleSubmit}>
          <fieldset>
            <div id="legend">
              <legend className="">Set Avatar</legend>
            </div>

            <FileUploader
              onFileSelectSuccess={(file) => setSelectedFile(file)}
              onFileSelectError={(data) => toast.error(data.error, toastOption)}
            />
          </fieldset>
        </form>
      </div>
      <ToastContainer />
    </div>
  );
}
