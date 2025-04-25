import React, { useEffect, useState, useMemo } from 'react';
import { useLocation } from 'react-router-dom';
import api from '../api.js';
import Alert from './Alert';
import Navigation from "./Navigation";
import RowItemList from './RowItemList';
import { ProjectCardStudent, ProjectCardTeacher } from './ProjectCards';
import { SearchBar, SortSelect, FilterSelect } from './FilterTools';
import '../styles/card-container.css';
import '../styles/home.css';

// DUMMY user data
const user = {
	login: 'alice',
	name: 'Alice',
	surname: 'Wonder',
	role: 'Student', // change to 'Teacher' to see other version
	id: 2 // Needed to match with teams or owner
};

function HomePage() {
	const location = useLocation();
	const [alert, setAlert] = useState(location.state?.alert || null);
	const [projects, setProjects] = useState([]);
	const [teamRequests, setTeamRequests] = useState([]);
	const [loading, setLoading] = useState(true);

	const [searchTerm, setSearchTerm] = useState('');
	const [filterKey, setFilterKey] = useState('');
	const [sortOption, setSortOption] = useState('name-asc');

	const now = new Date();

	useEffect(() => {
		const fetchData = async () => {
			try {
				const res = await api.get('/projects');
				let fetchedProjects = res.data;

				// Filter only projects user is related to
				let relevantProjects = fetchedProjects.filter(proj =>
					user.role === 'Student'
						? proj.teams.some(t => t.students?.some(s => s.id === user.id))
						: proj.ownerId === user.id
				);

				let allSolutions = [];
				for (const proj of relevantProjects) {
					const res = await api.get(`/projects/${proj.id}/solutions`);
					allSolutions = allSolutions.concat(res.data);
				}

				// Enhance project with computed fields
				const enhancedProjects = relevantProjects.map(proj => {
					const projSolutions = allSolutions.filter(sol => sol.projectId === proj.id);
					const submitted = projSolutions.some(sol => {
						if (user.role === 'Student') {
							return sol.team?.students?.some(s => s.id === user.id);
						}
						return true;
					});
					const studentEvaluation = projSolutions.find(sol =>
						sol.evaluation && sol.team?.students?.some(s => s.id === user.id)
					);

					return {
						id: proj.id,
						name: proj.name,
						course: proj.course,
						deadline: proj.deadline,
						capacity: proj.capacity,
						registered: proj.teams.reduce((sum, t) => sum + (t.students?.length || 0), 0),
						submissions: projSolutions.length,
						evaluations: projSolutions.filter(sol => sol.evaluation !== null).length,
						submission_date: studentEvaluation?.submissionDate || null,
						points: studentEvaluation?.evaluation?.points || null,
						submitted,
						isTeamProject: proj.maxTeamSize > 1,
						teamRequests: proj.teams.flatMap(t => t.signRequests || []).filter(req => req.state === 'Created')
					};
				});

				setProjects(enhancedProjects);

				// Set team requests if Student
				if (user.role === 'Student') {
					const requests = enhancedProjects.flatMap(p => p.teamRequests.map(req => ({
						id: req.id,
						user: req.student.username,
						date: req.creationDate,
						team: req.teamId,
						teamId: req.teamId,
					})));
					setTeamRequests(requests);
				}

				setLoading(false);
			} catch (err) {
				console.error(err);
				setAlert({ type: 'error', message: 'Failed to load project data' });
			}
		};

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
	if (user.role === 'Student') {
		filterOptions.push(
			{ value: 'submitted', label: 'Submitted' },
			{ value: 'not-submitted', label: 'Not Submitted' },
			{ value: 'evaluated', label: 'Evaluated' },
			{ value: 'not-evaluated', label: 'Not Evaluated' }
		);
	}
	if (user.role === 'Teacher') {
		filterOptions.push(
			{ value: 'pending-evaluation', label: 'Pending Evaluation' }
		);
	}

	const displayedProjects = useMemo(() => {
		let result = [...projects].filter(p =>
			p.name.toLowerCase().includes(searchTerm.toLowerCase())
		);

		if (filterKey) {
			switch (filterKey) {
				case 'submitted':
					result = result.filter(p => p.submitted);
					break;
				case 'not-submitted':
					result = result.filter(p => !p.submitted);
					break;
				case 'evaluated':
					result = result.filter(p => p.points !== null);
					break;
				case 'not-evaluated':
					result = result.filter(p => p.points === null);
					break;
				case 'pending-evaluation':
					result = result.filter(p => p.submissions > p.evaluations);
					break;
				case 'before-deadline':
					result = result.filter(p => new Date(p.deadline) >= now);
					break;
				case 'after-deadline':
					result = result.filter(p => new Date(p.deadline) < now);
					break;
				default:
					break;
			}
		}

		result.sort((a, b) => {
			switch (sortOption) {
				case 'name-asc': return a.name.localeCompare(b.name);
				case 'name-desc': return b.name.localeCompare(a.name);
				case 'deadline-asc': return new Date(a.deadline) - new Date(b.deadline);
				case 'deadline-desc': return new Date(b.deadline) - new Date(a.deadline);
				default: return 0;
			}
		});

		return result;
	}, [projects, searchTerm, filterKey, sortOption]);

	const upcomingDeadlines = projects.filter(p => new Date(p.deadline) >= now)
		.sort((a, b) => new Date(a.deadline) - new Date(b.deadline));

	const teacherPendingEvaluations = projects.filter(p => p.submissions > p.evaluations);

	if (loading) return <div>Loading...</div>;

	return (
		<div className="main-content-wrapper">
			{alert && <Alert type={alert.type} message={alert.message} duration={3500} onClose={() => setAlert(null)} />}
			<Navigation user={user} />
			<div className="home-container">
				<h2>Dashboard</h2>
				<div className="dashboard">
					{user.role === 'Student' && (
						<>
							<RowItemList
								title="Upcoming Deadlines"
								items={upcomingDeadlines.map(p => ({ label: p.name, link: `/projects/${p.id}` }))}
							/>
							<RowItemList
								title="Pending Team Requests"
								items={teamRequests.map(req => ({
									label: `Team: ${req.team}`,
									link: `/projects/${req.id}/team`
								}))}
							/>
						</>
					)}
					{user.role === 'Teacher' && (
						<RowItemList
							title="Projects Pending Evaluation"
							items={teacherPendingEvaluations.map(p => ({
								label: p.name,
								link: `/projects/${p.id}/evaluations`
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
