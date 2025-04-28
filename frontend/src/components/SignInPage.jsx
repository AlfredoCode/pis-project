import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import Alert from './Alert';
import LoadingScreen from './LoadingScreen';
import api from '../api';
import '../styles/login.css';
import '../styles/common.css';

function SignInPage() {
    const [alert, setAlert] = useState(null);
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();

    const [username, setLogin] = useState('');
	const [password, setPassword] = useState('');
	const [confirmPassword, setConfirmPassword] = useState('');
	const [firstName, setFirstName] = useState('');
	const [lastName, setLastName] = useState('');
	const [userType, setUserType] = useState('Student');
    
	const handleSignUp = async (e) => {
		e.preventDefault();

		if (password !== confirmPassword) {
			setAlert({ type: 'error', message: 'Passwords do not match.' });
			return;
		}

		setLoading(true);
		try {
			const res = await api.post('/register', { username, password, firstName, lastName, userType });
			localStorage.setItem('token', res.data.token);
			navigate('/home', { state: { alert: { type: 'success', message: 'Registered successfully!' } } });
		} catch (err) {
			console.error(err);
			setAlert({ type: 'error', message: err.response?.data?.message || 'Registration failed.' });
		} finally {
			setLoading(false);
		}
	};


    return (
        <div className="main-content-wrapper">
            {alert && (<Alert type={alert.type} message={alert.message} duration={3500} onClose={() => setAlert(null)} />)}
            {loading && <LoadingScreen />}
            <div className="auth-container">
                <h2>Sign up</h2>
                <form onSubmit={handleSignUp}>
                    <input className="input-empty" type="text" placeholder="Login" value={username} onChange={e => setLogin(e.target.value)} required />
                    <input className="input-empty" type="text" placeholder="First Name" value={firstName} onChange={e => setFirstName(e.target.value)} required />
                    <input className="input-empty" type="text" placeholder="Last Name" value={lastName} onChange={e => setLastName(e.target.value)} required />
                    <input className="input-empty" type="password" placeholder="Password" value={password} onChange={e => setPassword(e.target.value)} required />
                    <input className="input-empty" type="password" placeholder="Confirm Password" value={confirmPassword} onChange={e => setConfirmPassword(e.target.value)} required />
                    <select className="select-empty" value={userType} onChange={e => setUserType(e.target.value)}>
                        <option value="student">Student</option>
                        <option value="teacher">Teacher</option>
                    </select>
                    <button className="btn-filled-round" type="submit">Create Account</button>
                </form>
                <p>Already have an account? <Link className="btn-empty-sharp" to="/">Log in</Link></p>
            </div>
        </div>
    );
}

export default SignInPage;