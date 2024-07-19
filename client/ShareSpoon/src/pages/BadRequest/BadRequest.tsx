import React from 'react'
import { Helmet } from 'react-helmet-async'
import "./BadRequest.css"
import { useNavigate } from 'react-router-dom';
import { useUser } from '../../hooks/useUser';

const BadRequest: React.FC = () => {
  const navigate = useNavigate();
  const { user } = useUser();

  const handleNavigate = () => {
    if (user && user.role === 0) {
      navigate("/admin");
    } else {
      navigate("/");
    }
  };
  
  return (
    <>
      <Helmet>
          <title>Page Not Found</title>
      </Helmet>
      <div className="notFoundContainer">
          <div className="notFoundContent">
              <h1>Hoopsie!</h1>
              <p>Page Not Found :(</p>
              <span>The page you are looking for could not be found.</span>
              <span className="returnLink" onClick={handleNavigate}>Return to website</span>
          </div>
      </div>
    </>
  );
};

export default BadRequest;