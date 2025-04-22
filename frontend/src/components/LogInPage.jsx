import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import "../styles/LogIn.css";
import "../styles/common.css";

function LoginPage({ onSuccess }) {
	const [login, setLogin] = useState('');
	const [password, setPassword] = useState('');
	const navigate = useNavigate();

	const handleSubmit = (e) => {
		e.preventDefault();
		// DUMMY: Hardcoded authentication check
		if (login === 'user' && password === 'password') {
			navigate('/home');
		} else {
			alert('Invalid login credentials');
		}
	};

  return (
    <div className="auth-container">
        <h2>Log in</h2>
            <form onSubmit={handleSubmit}>
            <input className="input-empty" type="text" placeholder="Login" value={login} onChange={e => setLogin(e.target.value)} required />
            <input className="input-empty" type="password" placeholder="Password" value={password} onChange={e => setPassword(e.target.value)} required />
            <button className="btn-filled-round" type="submit">Log In</button>
        </form>
        <p>Don't have an account? <Link className="btn-empty-sharp" to="/signup" >Sign up</Link></p>
    </div>
  );
}

export default LoginPage;