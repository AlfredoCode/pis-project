import React from 'react';
import '../styles/error-screen.css';


function ErrorScreen({ type, message }) {
    return (
        <div className="error-overlay">
            <div className="error-message">
                <h2>{type}</h2>
                <p>{message}</p>
            </div>
        </div>
    );
}

export default ErrorScreen;