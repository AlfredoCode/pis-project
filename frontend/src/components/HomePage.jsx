import React from 'react';
import { useState } from "react";
import Navigation from "./Navigation";
import MainContent from "./MainContent";

function HomePage() {

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

export default HomePage;
