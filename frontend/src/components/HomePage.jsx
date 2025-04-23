import React from 'react';
import { useEffect, useState } from "react";
import { useLocation } from 'react-router-dom';
import Alert from './Alert';
import Navigation from "./Navigation";
import MainContent from "./MainContent";

function HomePage() {

    const [isMenuHovered, setIsMenuHovered] = useState(false);
    const location = useLocation();
    const [alert, setAlert] = useState(location.state?.alert || null);

    // Clear alert after showing once
    useEffect(() => {
        if (alert) {
            const timer = setTimeout(() => setAlert(null), 3000);
            return () => clearTimeout(timer);
        }
    }, [alert]);

    return (
        <>
            {alert && <Alert type={alert.type} message={alert.message} duration={3000} onClose={() => setAlert(null)}/>}
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
