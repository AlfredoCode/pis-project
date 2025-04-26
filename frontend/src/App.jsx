import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import LoginPage from './components/LogInPage';
import SignInPage from './components/SignInPage';
import HomePage from './components/HomePage';
import ProjectsPage from './components/ProjectsPage';

function App() {
	return (
		<Router>
		<Routes>
			<Route path='/' element={<LoginPage />} />
			<Route path='/signup' element={<SignInPage />} />
			<Route path='/home' element={<HomePage />} />
			<Route path='/projects' element={<ProjectsPage />} />
		</Routes>
		</Router>
	);
}

export default App;
