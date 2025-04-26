import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import Alert from './Alert';
import "../styles/login.css";
import "../styles/common.css";

function LoginPage() {
	const [login, setLogin] = useState('');
	const [password, setPassword] = useState('');
	const [alert, setAlert] = useState(null);
	const navigate = useNavigate();

	const handleSubmit = (e) => {
		e.preventDefault();
		// DUMMY: Hardcoded authentication check
		if (login === 'user' && password === 'password') {
			navigate('/home', {state: {
				alert: {type: 'success', message: 'Logged in successfully!'}
			}});
		} else {
			setAlert({ type: 'error', message: 'Invalid login credentials!' });
		}
	};

  return (
	<div className="main-content-wrapper">
		{alert && (<Alert type={alert.type} message={alert.message} duration={3500} onClose={() => setAlert(null)} />)}
		<div className="auth-container">
			<h2>Log in</h2>
				<form onSubmit={handleSubmit}>
				<input className="input-empty" type="text" placeholder="Login" value={login} onChange={e => setLogin(e.target.value)} required />
				<input className="input-empty" type="password" placeholder="Password" value={password} onChange={e => setPassword(e.target.value)} required />
				<button className="btn-filled-round" type="submit">Log In</button>
			</form>
			<p>Don't have an account? <Link className="btn-empty-sharp" to="/signup" >Sign up</Link></p>
		</div>
	</div>
  );
}

export default LoginPage;