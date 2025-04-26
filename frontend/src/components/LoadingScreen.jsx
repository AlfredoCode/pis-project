import React from 'react';
import '../styles/loading-screen.css';


function LoadingScreen() {
    return (
        <div className="loading-overlay">
		    <div className="loading-spinner">Loading projects...</div>
	    </div>
    );
}

export default LoadingScreen;