// src/components/Alert.jsx
import React, { useEffect } from 'react';
import "../styles/alert.css"; // Optional: style it separately

function Alert({ type = 'info', message, duration = 3000, onClose }) {
  useEffect(() => {
    if (duration > 0) {
      const timer = setTimeout(() => {
        onClose?.();
      }, duration);
      return () => clearTimeout(timer);
    }
  }, [duration, onClose]);

  return (
    <div className={`alert alert-${type}`}>
      {type + ": " + message}
    </div>
  );
}

export default Alert;