import React, { useState } from 'react';

function LoginPage({ onSwitch, onSuccess }) {
  const [login, setLogin] = useState('');
  const [password, setPassword] = useState('');

  const handleSubmit = (e) => {
    e.preventDefault();
    // DUMMY: Hardcoded authentication check
    if (login === 'user' && password === 'password') {
        onSuccess();
    } else {
        alert('Invalid login credentials');
    }
  };

  return (
    <div className="auth-container">
        <h2>Log in</h2>
            <form onSubmit={handleSubmit}>
            <input type="text" placeholder="Login" value={login} onChange={e => setLogin(e.target.value)} required />
            <input type="password" placeholder="Password" value={password} onChange={e => setPassword(e.target.value)} required />
            <button type="submit">Log In</button>
        </form>
        <p>Don't have an account? <button onClick={onSwitch}>Sign up</button></p>
    </div>
  );
}

export default LoginPage;