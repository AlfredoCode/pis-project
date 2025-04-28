import React, { useEffect, useState, useMemo } from 'react';
import { useLocation } from 'react-router-dom';
import api from '../api.js';
import Alert from './Alert';
import LoadingScreen from './LoadingScreen.jsx';
import Navigation from "./Navigation";
import RowItemList from './RowItemList';
import { ProjectCardStudent, ProjectCardTeacher } from './ProjectCards';
import { SearchBar, SortSelect, FilterSelect } from './FilterTools';
import { filterProjects } from '../utils/filterProjects.js';
import { formatRemainingTime } from '../utils/formatDate.js';
import '../styles/card-container.css';
import '../styles/home.css';
import { getCurrentUser } from '../auth.js';



function HomePage() {
	const location = useLocation();
	const [alert, setAlert] = useState(location.state?.alert || null);
	const [loading, setLoading] = useState(false);

	const [projects, setProjects] = useState([]);
	const [teamRequests, setTeamRequests] = useState([]);

	const [searchTerm, setSearchTerm] = useState('');
	const [filterKey, setFilterKey] = useState('');
	const [sortOption, setSortOption] = useState('name-asc');
	const [user, setUser] = useState(null)
	const now = new Date();

	useEffect(() => {
		const fetchData = async () => {
			setLoading(true);
			try {
				let fetchedUser = await getCurrentUser();
				setUser(fetchedUser);
	
				if (fetchedUser && fetchedUser.role === 'Student') {
					const resProjects = await api.get(`/users/${fetchedUser.id}/projects`);
					const resTeamRequests = await api.get(`/students/${fetchedUser.id}/signuprequests`);
					setTeamRequests(resTeamRequests.data);
	
					const mappedProjects = resProjects.data.map(project => ({
						id: project.id,
						name: project.name,
						course: project.course,
						owner: project.owner,
						deadline: project.deadline,
						submissionDate: project.team.solution?.submissionDate ?? null,
						points: project.team.solution?.evaluationPoints ?? null,
						isTeamProject: project.maxTeamSize === 1 ? false : true,
						teamName: project.team.name
					}));
					setProjects(mappedProjects);
				} else if (fetchedUser?.role === 'Teacher') {
					const resProjects = await api.get(`/users/${fetchedUser.id}/projects`);
					const mappedProjects = resProjects.data.map(project => ({
						id: project.id,
						name: project.name,
						course: project.course,
						deadline: project.deadline,
						registered: project.registeredTeams,
						submissions: project.teamsWithSubmissions,
						capacity: project.capacity,
						isTeamProject: project.maxTeamSize === 1 ? false : true
					}));
					setProjects(mappedProjects);
				}
			} catch (err) {
				console.error('Error fetching user or projects:', err);
			} finally {
				setLoading(false);
			}
		}
	
		fetchData();
	}, []);
	

	useEffect(() => {
		if (alert) {
			const timer = setTimeout(() => setAlert(null), 3000);
			return () => clearTimeout(timer);
		}
	}, [alert]);

	const sortOptions = [
		{ value: 'name-asc', label: 'Name A-Z' },
		{ value: 'name-desc', label: 'Name Z-A' },
		{ value: 'deadline-asc', label: 'Deadline ↑' },
		{ value: 'deadline-desc', label: 'Deadline ↓' },
	];

	const filterOptions = [
		{ value: 'before-deadline', label: 'Before Deadline' },
		{ value: 'after-deadline', label: 'After Deadline' }
	];
	if (user?.role === 'Student') {
		filterOptions.push(
			{ value: 'submitted', label: 'Submitted' },
			{ value: 'not-submitted', label: 'Not Submitted' },
			{ value: 'evaluated', label: 'Evaluated' },
			{ value: 'not-evaluated', label: 'Not Evaluated' }
		);
	}

	// Projects filtering
	const displayedProjects = useMemo(() => {
		return filterProjects(projects, { searchTerm, filterKey, sortOption });
	}, [projects, searchTerm, filterKey, sortOption]);


	// Dashboard filtering
	const upcomingDeadlines = projects
		.filter(p => new Date(p.deadline) >= now)
		.sort((a, b) => new Date(a.deadline) - new Date(b.deadline));

	// Recent (last 3 months) Team Requests
	const threeMonthsAgo = new Date();
	threeMonthsAgo.setMonth(now.getMonth() - 3);
	const recentTeamRequests = teamRequests
		.filter(req => new Date(req.creationDate) >= threeMonthsAgo)
		.sort((a, b) => new Date(a.creationDate) - new Date(b.creationDate));


	return (
		<div className="main-content-wrapper">
			{alert && <Alert type={alert.type} message={alert.message} duration={3500} onClose={() => setAlert(null)} />}
			{loading && <LoadingScreen />}
			<Navigation user={user} />
			<div className="home-container">
				<h2>Dashboard</h2>
				<div className="dashboard">
					{user && user.role === 'Student' && (
						<>
							<RowItemList
								title="Upcoming Deadlines"
								items={upcomingDeadlines.map(p => ({ 
									key: p.id,
									label: `${p.name} (${formatRemainingTime(p.deadline)})`,
									link: `/project/${p.id}` 
								}))}
							/>
							<RowItemList
								title="Pending Team Requests"
								items={recentTeamRequests.map(req => ({
									key: req.id,
									label: `${req.team.name} (${req.state})`,
									link: `/team/${req.id}/`
								}))}
							/>
						</>
					)}
					{user && user.role === 'Teacher' && (
						<RowItemList
							title="Upcoming Deadlines"
							items={upcomingDeadlines.map(p => ({
								key: p.id, 
								label: `${p.name} (${formatRemainingTime(p.deadline)})`, 
								link: `/project/${p.id}` 
							}))}
						/>
					)}
				</div>
				<h2>My Projects</h2>
				<div className="filter-tools">
					<SearchBar value={searchTerm} onChange={setSearchTerm} placeholder="Search projects..." />
					<SortSelect value={sortOption} onChange={setSortOption} options={sortOptions} />
					<FilterSelect value={filterKey} onChange={setFilterKey} options={filterOptions} placeholder="All" />
				</div>
				<div className="card-container">
					{displayedProjects.map((p) =>
						user.role === 'Student'
							? <ProjectCardStudent key={p.id} project={p} />
							: <ProjectCardTeacher key={p.id} project={p} />
					)}
				</div>
			</div>
		</div>
	);
}

export default HomePage;
