import React, { useState } from 'react';
import { useNavigate, Link, useLocation } from 'react-router-dom';
import Alert from './Alert';
import LoadingScreen from './LoadingScreen';
import api from '../api';
import "../styles/login.css";
import "../styles/common.css";


function LoginPage() {
	const location = useLocation();
	const [alert, setAlert] = useState(location.state?.alert || null);
	const [loading, setLoading] = useState(false);
	const navigate = useNavigate();

	const [username, setLogin] = useState('');
	const [password, setPassword] = useState('');

	const handleLogin = async (e) => {
		e.preventDefault();
		setLoading(true);
		try {
			const res = await api.post('/login', { username, password });
			localStorage.setItem('token', res.data.token);
			navigate('/home', {state: {
				alert: {type: 'success', message: 'Logged in successfully!'}
			}});
		} catch (err) {
			console.error(err);
			setAlert({ type: 'error', message: 'Invalid login credentials!' });
		} finally {
			setLoading(false);
		}
	};

	return (
		<div className="main-content-wrapper">
			{alert && (<Alert type={alert.type} message={alert.message} duration={3500} onClose={() => setAlert(null)} />)}
			{loading && <LoadingScreen />}
			<div className="auth-container">
				<h2>Log in</h2>
					<form onSubmit={handleLogin}>
					<input className="input-empty" type="text" placeholder="Login" value={username} onChange={e => setLogin(e.target.value)} required />
					<input className="input-empty" type="password" placeholder="Password" value={password} onChange={e => setPassword(e.target.value)} required />
					<button className="btn-filled-round" type="submit">Log In</button>
				</form>
				<p>Don't have an account? <Link className="btn-empty-sharp" to="/signup" >Sign up</Link></p>
			</div>
		</div>
	);
}

export default LoginPage;