import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import "../styles/login.css";
import "../styles/common.css";

function SignInPage({ onSuccess }) {
    const [formData, setFormData] = useState({
        login: '',
        firstName: '',
        lastName: '',
        password: '',
        confirmPassword: '',
        role: 'student'
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const navigate = useNavigate();

    const handleSubmit = (e) => {
        e.preventDefault();
        if (formData.password !== formData.confirmPassword) {
            alert('Passwords do not match');
            return;
        }
        // DUMMY: Accept any registration
        navigate('/home');
    };

    return (
    <div className="auth-container">
        <h2>Sign up</h2>
        <form onSubmit={handleSubmit}>
            <input className="input-empty" type="text" name="login" placeholder="Login" value={formData.login} onChange={handleChange} required />
            <input className="input-empty" type="text" name="firstName" placeholder="First Name" value={formData.firstName} onChange={handleChange} required />
            <input className="input-empty" type="text" name="lastName" placeholder="Last Name" value={formData.lastName} onChange={handleChange} required />
            <input className="input-empty" type="password" name="password" placeholder="Password" value={formData.password} onChange={handleChange} required />
            <input className="input-empty" type="password" name="confirmPassword" placeholder="Confirm Password" value={formData.confirmPassword} onChange={handleChange} required />
            <select className="select-empty" name="role" value={formData.role} onChange={handleChange}>
                <option value="student">Student</option>
                <option value="teacher">Teacher</option>
            </select>
            <button className="btn-filled-round" type="submit">Create Account</button>
        </form>
        <p>Already have an account? <Link className="btn-empty-sharp" to="/">Log in</Link></p>
    </div>
    );
    }

export default SignInPage;