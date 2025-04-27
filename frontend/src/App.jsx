import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import LoginPage from './components/LogInPage';
import SignInPage from './components/SignInPage';
import HomePage from './components/HomePage';
import ProjectsPage from './components/ProjectsPage';
import ProjectPage from './components/ProjectPage';
import TeamDetailPage from './components/TeamDetailPage';
import ProjectFormPage from './components/ProjectFormPage';
import TeamFormPage from './components/TeamFormPage';

function App() {
	return (
		<Router>
		<Routes>
			<Route path='/' element={<LoginPage />} />
			<Route path='/signup' element={<SignInPage />} />
			<Route path='/home' element={<HomePage />} />
			<Route path='/projects' element={<ProjectsPage />} />
			<Route path='/project/:projectId' element={<ProjectPage />} />
			<Route path='/project/new' element={<ProjectFormPage mode='create' />} />
			<Route path='/project/edit/:id' element={<ProjectFormPage mode='edit' />} />
			<Route path='/team/new/:id' element={<TeamFormPage mode='create' />} />
			<Route path='/team/edit/:id' element={<TeamFormPage mode='edit' />} />
			<Route path='/team/:tId' element={<TeamDetailPage />} />
		</Routes>
		</Router>
	);
}

export default App;
