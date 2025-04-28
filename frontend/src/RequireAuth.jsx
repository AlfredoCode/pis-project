import { Navigate } from 'react-router-dom';

export default function RequireAuth({ children }) {
    const token = localStorage.getItem('token');

    if (!token) {
        // Not logged in redirect to login page
        return <Navigate to="/" state={{ alert: {type: 'error', message: 'Login required!'} }} replace />;
    }

    return children;
}
