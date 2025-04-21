import React, { useState } from 'react';
import LoginPage from './components/LogInPage';
import SignInPage from './components/SignInPage';
import HomePage from './components/HomePage';

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [showSignIn, setShowSignIn] = useState(false);

  if (isAuthenticated) {
    return <HomePage />;
  }

  return showSignIn ? (
    <SignInPage onSwitch={() => setShowSignIn(false)} onSuccess={() => setIsAuthenticated(true)} />
  ) : (
    <LoginPage onSwitch={() => setShowSignIn(true)} onSuccess={() => setIsAuthenticated(true)} />
  );
}

export default App;
