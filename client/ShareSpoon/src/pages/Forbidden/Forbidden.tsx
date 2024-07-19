import React from 'react'
import { Helmet } from 'react-helmet-async'
import "./Forbidden.css"
import { useNavigate } from 'react-router-dom';
import { useUser } from '../../hooks/useUser';

const Forbidden: React.FC = () => {
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
          <title>Forbidden</title>
      </Helmet>
      <div className="forbiddenContainer">
          <div className="forbiddenContent">
              <h1>Forbidden</h1>
              <p>Your account does not have the rights to access this page.</p>
              <span onClick={handleNavigate}>Return to website</span>
          </div>
      </div>
    </>
  );
};

export default Forbidden;