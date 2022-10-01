import React from "react";
import "./Login.css";
import { ToastContainer } from "react-toastify";
import { Link, useNavigate } from "react-router-dom";
import { useAuthContext } from "../../providers/AuthProvider"

export default function Login() {
  const navigate = useNavigate();
  const auth = useAuthContext();
  const [formData, setFormData] = React.useState({
    userName: "",
    password: "",
  });

  const handleSubmit = async (event) => {
    event.preventDefault();
    var user = await auth.signIn(formData);
    if (user != null) {
      navigate("/");
    }
  };

  const handleChange = (event) => {
    const { name, value } = event.target;
    setFormData((prevFormData) => {
      return {
        ...prevFormData,
        [name]: value,
      };
    });
  };

  return (
    <div className="card">
      <div className="register container">
        <form onSubmit={handleSubmit}>
          <fieldset>
            <div id="legend">
              <legend className="">Login</legend>
            </div>
            <div className="form-group">
              <label htmlFor="userName">User Name</label>
              <input
                id="userName"
                type="text"
                placeholder="UserName"
                className="form-control"
                name="userName"
                onChange={handleChange}
                value={formData.userName}
              />
            </div>

            <div className="form-group">
              <label htmlFor="password">Password</label>
              <input
                id="password"
                type="password"
                placeholder="Password"
                className="form-control"
                name="password"
                onChange={handleChange}
                value={formData.password}
              />
            </div>

            <div className="form-group">
              <button type="submit" className="btn btn-primary">
                Login
              </button>{" "}
              <br />
              <br />
              <span>
                If you do not have account, you can create here{" "}
                <Link to="/register">Register</Link>
              </span>
            </div>
          </fieldset>
        </form>
      </div>
      <ToastContainer />
    </div>
  );
}
