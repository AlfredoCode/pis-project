import React, { useEffect, useState, useMemo } from 'react';
import { useLocation } from 'react-router-dom';
import Alert from './Alert';
import Navigation from "./Navigation";
import RowItemList from './RowItemList';
import {ProjectCardStudent, ProjectCardTeacher} from './ProjectCards';
import { SearchBar, SortSelect, FilterSelect } from './FilterTools';
import '../styles/card-container.css';
import '../styles/home.css'


/* DUMMY DATA */

// DUMMY user data
const user = {
	login: 'jdoe',
	name: 'John',
	surname: 'Doe',
	role: 'student', // change to 'teacher' to see other version
};

const projects = [
	{
		id: 1111,
		name: 'AI Research',
		deadline: '2025-05-15',
		course: 'Artificial Intelligence',
		submission_date: null,
		points: null,
		registered: 12,
		capacity: 15,
		submissions: 8,
		evaluations: 5,
		submitted: false,
		isTeamProject: true,
	},
	{
		id: 2222,
		name: 'Web Development',
		deadline: '2025-04-01',
		course: 'Frontend Basics',
		submission_date: '2025-03-11',
		points: 20,
		registered: 20,
		capacity: 20,
		submissions: 19,
		evaluations: 19,
		submitted: true,
		isTeamProject: false,
	}
];

const teamRequests = [
	{
		id: 1234,
		user: 'Jim',
		date: '2025-03-11',
		team: 'JoeTeam',
		teamId: 5512
	},
	{
		id: 1235,
		user: 'Joe',
		date: '2025-03-07',
		team: 'JoeTeam',
		teamId: 5512
	}
]

/* END DUMMY DATA */

function HomePage() {

    const location = useLocation();
    const [alert, setAlert] = useState(location.state?.alert || null);
	const [searchTerm, setSearchTerm] = useState('');
	const [filterKey, setFilterKey] = useState('');
	const [sortOption, setSortOption] = useState('name-asc');

	const now = new Date();

    // Clear alert after showing once
    useEffect(() => {
        if (alert) {
            const timer = setTimeout(() => setAlert(null), 3000);
            return () => clearTimeout(timer);
        }
    }, [alert]);

	// Dashboard info
	const upcomingDeadlines = projects.filter(proj => new Date(proj.deadline) >= now)
    	.sort((a, b) => new Date(a.deadline) - new Date(b.deadline));
	const pendingTeamRequests = teamRequests;
	const teacherPendingEvaluations = projects.filter(proj => proj.submissions > proj.evaluations);


	// Search, filter and sort projects
	const displayedProjects = useMemo(() => {
		let result = projects
			.filter((proj) => proj.name.toLowerCase().includes(searchTerm.toLocaleLowerCase()));

		if (filterKey) {
			switch (filterKey) {
				case 'submitted':
					result = result.filter(proj => proj.submitted);
				break;
				case 'not-submitted':
					result = result.filter(proj => !proj.submitted);
				break;
				case 'evaluated':
					result = result.filter(proj => proj.points !== null);
				break;
				case 'not-evaluated':
					result = result.filter(proj => proj.points === null);
				break;
				case 'pending-evaluation':
					result = result.filter(proj => proj.submissions > proj.evaluations);
				break;
				case 'before-deadline':
					result = result.filter(proj => new Date(proj.deadline) >= now);
				break;
				case 'after-deadline':
					result = result.filter(proj => new Date(proj.deadline) < now)
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
	}, [searchTerm, filterKey, sortOption]);

	// Sorting options
	const sortOptions = [
		{ value: 'name-asc', label: 'Name A-Z' },
		{ value: 'name-desc', label: 'Name Z-A' },
		{ value: 'deadline-asc', label: 'Deadline ↑' },
		{ value: 'deadline-desc', label: 'Deadline ↓' },
	];

	// Build filter options based on role
	const filterOptions = [
		{ value: 'before-deadline', label: 'Before Deadline' },
		{ value: 'after-deadline', label: 'After Deadline' }
	];
	if (user.role === 'student') {
		filterOptions.push(
			{ value: 'submitted', label: 'Submitted' },
			{ value: 'not-submitted', label: 'Not Submitted' },
			{ value: 'evaluated', label: 'Evaluated' },
			{ value: 'not-evaluated', label: 'Not Evaluated' }
		);
	}
	if (user.role === 'teacher') {
		filterOptions.push(
			{ value: 'pending-evaluation', label: 'Pending Evaluation' }
		);
	}

	return (
		<div className="main-content-wrapper">
			{alert && <Alert type={alert.type} message={alert.message} duration={3500} onClose={() => setAlert(null)}/>}
			<Navigation user={user} />
			<div className="home-container">
				<h2>Dashboard</h2>
				<div className="dashboard">
					{user.role === 'student' && (
					<>
						<RowItemList
						title="Upcoming Deadlines"
						items={upcomingDeadlines.map(proj => ({
							label: proj.name,
							link: `/projects/${proj.id}`
						}))}
						/>
						<RowItemList
						title="Pending Team Requests"
						items={pendingTeamRequests.map(req => ({
							label: req.team,
							link: `/projects/${req.id}/team`
						}))}
						/>
					</>
					)}
					{user.role === 'teacher' && (
						<RowItemList
							title="Projects Pending Evaluation"
							items={teacherPendingEvaluations.map(proj => ({
							label: proj.name,
							link: `/projects/${proj.id}/evaluations`
							}))}
						/>
					)}
				</div>
				<h2>My projects</h2>
				<div className="filter-tools">
					<SearchBar
						placeholder="Search projects..."
						value={searchTerm}
						onChange={setSearchTerm}
					/>
					<SortSelect value={sortOption} onChange={setSortOption} options={sortOptions} />
					<FilterSelect value={filterKey} onChange={setFilterKey} options={filterOptions} placeholder="All" />
				</div>
				<div className="card-container">
					{displayedProjects.map((project) => 
						user.role === 'student' ?
						<ProjectCardStudent key={project.id} project={project} /> :
						<ProjectCardTeacher key={project.id} project={project} />
					)}
				</div>
			</div>
		</div>
	);
}

export default HomePage;
