import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import RequireAuth from './RequireAuth';
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
			<Route path='/home' element={<RequireAuth> <HomePage /> </RequireAuth>} />
			<Route path='/projects' element={<RequireAuth> <ProjectsPage /> </RequireAuth>} />
			<Route path='/project/:projectId' element={<RequireAuth> <ProjectPage /> </RequireAuth>} />
			<Route path='/project/new' element={<RequireAuth> <ProjectFormPage mode='create' /> </RequireAuth>} />
			<Route path='/project/edit/:id' element={<RequireAuth> <ProjectFormPage mode='edit' /> </RequireAuth>} />
			<Route path='/team/new/:id' element={<RequireAuth> <TeamFormPage mode='create' /> </RequireAuth>} />
			<Route path='/team/edit/:id' element={<RequireAuth> <TeamFormPage mode='edit' /> </RequireAuth>} />
			<Route path='/team/:tId' element={<RequireAuth> <TeamDetailPage /> </RequireAuth>} />
		</Routes>
		</Router>
	);
}

export default App;
