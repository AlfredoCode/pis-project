import React, { useEffect, useState, useMemo } from 'react';
import { useLocation } from 'react-router-dom';
import api from '../api.js';
import Alert from './Alert';
import LoadingScreen from './LoadingScreen.jsx';
import Navigation from "./Navigation";
import { ProjectCard } from './ProjectCards';
import { SearchBar, SortSelect, FilterSelect } from './FilterTools';
import { filterProjects } from '../utils/filterProjects.js';
import '../styles/card-container.css';
import { getCurrentUser } from '../auth.js';


function ProjectsPage() {
	const location = useLocation();
	const [alert, setAlert] = useState(location.state?.alert || null);
	const [projects, setProjects] = useState([]);
	const [loading, setLoading] = useState(true);
	const [user, setUser] = useState(null)
	const [searchTerm, setSearchTerm] = useState('');
	const [filterKey, setFilterKey] = useState('');
	const [sortOption, setSortOption] = useState('name-asc');

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

    // Projects fetching
    useEffect(() => {
		async function init ()  {

			let fetchedUser = await getCurrentUser();
			setUser(fetchedUser);
			api.get('/projects')
				.then(response => {
					setProjects(response.data);
					setLoading(false);
				})
				.catch(error => {
					console.error('Error fetching projects:', error);
					setAlert({
						type: 'error',
						message: 'Failed to load projects. Please try again later.'
					});
					setLoading(false);
				});
		}
		init()
    }, []);

    // Projects filtering
	const displayedProjects = useMemo(() => {
        return filterProjects(projects, { searchTerm, filterKey, sortOption });
	}, [projects, searchTerm, filterKey, sortOption]);


	return (
		<div className="main-content-wrapper">
			{alert && <Alert type={alert.type} message={alert.message} duration={3500} onClose={() => setAlert(null)} />}
            {loading && <LoadingScreen />}
			<Navigation user={user} />
			<div className="projects-container">
				<div className="filter-tools">
					<SearchBar value={searchTerm} onChange={setSearchTerm} placeholder="Search projects..." />
					<SortSelect value={sortOption} onChange={setSortOption} options={sortOptions} />
					<FilterSelect value={filterKey} onChange={setFilterKey} options={filterOptions} placeholder="All" />
				</div>
				<div className="projects-content">
					<div className="card-container">
						{displayedProjects.map((p) =>
							<ProjectCard key={p.id} project={p} />
						)}
					</div>
				</div>
			</div>
		</div>
	);
}

export default ProjectsPage;