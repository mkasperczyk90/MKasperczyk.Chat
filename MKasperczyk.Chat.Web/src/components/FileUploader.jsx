import React, { useRef, useState } from "react";

export default function FileUploader({
  onFileSelectError,
  onFileSelectSuccess,
}) {
  const fileElement = useRef(null);
  const imageElement = useRef(null);
  const [hasFile, setHasFile] = useState(false);
  var imageVisibility = hasFile ? "visible" : "hidden";

  const handleFileInput = (event) => {
    const file = event.target.files[0];
    if (file.size > 10240) {
      // 10kb
      setHasFile(false);
      onFileSelectError({ error: "File size cannot exceed more than 10MB" });
    } else {
      setHasFile(true);
      onFileSelectSuccess(file);
      imageElement.current.src = URL.createObjectURL(file);
    }
  };

  return (
    <div className="form-group">
      <label htmlFor="formFile" className="form-label">
        Choose file image
      </label>
      <input
        className="form-control"
        type="file"
        id="formFile"
        onChange={handleFileInput}
      />
      <img
        ref={imageElement}
        style={{ visibility: imageVisibility }}
        src="#"
        alt="avatar"
      />
      <br />
      <br />
      <button
        onClick={(e) => fileElement.current && fileElement.current.click()}
        className="btn btn-primary"
      >
        Set Avatar
      </button>
    </div>
  );
}
