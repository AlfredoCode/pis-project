import MainContent from "./components/MainContent";
import Navigation from "./components/Navigation";
import { useState } from "react";

function App() {
  const [isMenuHovered, setIsMenuHovered] = useState(false);

  return (
    <>
      <Navigation
        isMenuHovered={isMenuHovered}
        setIsMenuHovered={setIsMenuHovered}
      />

      {isMenuHovered && <div className="focus-booster"></div>}
      <MainContent />
    </>
  );
}

export default App;
