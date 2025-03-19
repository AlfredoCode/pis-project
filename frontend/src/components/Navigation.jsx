import React, { useEffect } from "react";
import { IoMenu } from "react-icons/io5";
import "../styles/navigation.css";

function Navigation({ isMenuHovered, setIsMenuHovered }) {
  return (
    <div
      onMouseEnter={() => setIsMenuHovered(true)}
      onMouseLeave={() => {
        setIsMenuHovered(false);
      }}
      className="navigation-wrapper"
    >
      <IoMenu color="#bbbbbb" size={30} />
    </div>
  );
}

export default Navigation;
