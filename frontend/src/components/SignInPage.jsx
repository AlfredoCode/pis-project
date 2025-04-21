import React, { useState } from 'react';

function SignInPage({ onSwitch, onSuccess }) {
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

    const handleSubmit = (e) => {
        e.preventDefault();
        if (formData.password !== formData.confirmPassword) {
            alert('Passwords do not match');
            return;
        }
        // DUMMY: Accept any registration
        onSuccess();
    };

    return (
    <div className="auth-container">
        <h2>Sign up</h2>
        <form onSubmit={handleSubmit}>
            <input type="text" name="login" placeholder="Login" value={formData.login} onChange={handleChange} required />
            <input type="text" name="firstName" placeholder="First Name" value={formData.firstName} onChange={handleChange} required />
            <input type="text" name="lastName" placeholder="Last Name" value={formData.lastName} onChange={handleChange} required />
            <input type="password" name="password" placeholder="Password" value={formData.password} onChange={handleChange} required />
            <input type="password" name="confirmPassword" placeholder="Confirm Password" value={formData.confirmPassword} onChange={handleChange} required />
            <select name="role" value={formData.role} onChange={handleChange}>
                <option value="student">Student</option>
                <option value="teacher">Teacher</option>
            </select>
            <button type="submit">Create Account</button>
        </form>
        <p>Already have an account? <button onClick={onSwitch}>Log in</button></p>
    </div>
    );
    }

export default SignInPage;