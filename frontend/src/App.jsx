import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import LoginPage from './components/LogInPage';
import SignInPage from './components/SignInPage';
import HomePage from './components/HomePage';

function App() {
	return (
		<Router>
		<Routes>
			<Route path='/' element={<LoginPage />} />
			<Route path='/signup' element={<SignInPage />} />
			<Route path='/home' element={<HomePage />} />
		</Routes>
		</Router>
	);
}

export default App;
