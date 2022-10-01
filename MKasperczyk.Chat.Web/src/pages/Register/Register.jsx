import React from "react";
import { Link, useNavigate } from "react-router-dom";
import "./Register.css";
import { ToastContainer } from "react-toastify";
import { useAuthContext } from "../../providers/AuthProvider";

export default function Register() {
  const navigate = useNavigate();
  const auth = useAuthContext();
  const [formData, setFormData] = React.useState({
    userName: "",
    password: "",
    confirmPassword: "",
  });

  const handleSubmit = async (event) => {
    event.preventDefault();
    var user = await auth.signUp(formData);
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
              <legend className="">Register</legend>
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
              <label htmlFor="confirmPassword">Confirm Password</label>
              <input
                id="confirmPassword"
                type="password"
                placeholder="Confirm Password"
                className="form-control"
                name="confirmPassword"
                onChange={handleChange}
                value={formData.confirmPassword}
              />
            </div>

            <div className="form-group">
              <button type="submit" className="btn btn-primary">
                Create User
              </button>{" "}
              <br />
              <span>
                Already have an account? <Link to="/login">Login</Link>
              </span>
            </div>
          </fieldset>
        </form>
      </div>
      <ToastContainer />
    </div>
  );
}
