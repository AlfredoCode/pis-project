import React from 'react';
import { NavLink, useNavigate } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faRightFromBracket } from '@fortawesome/free-solid-svg-icons'
import '../styles/navigation.css';

function Navigation({ user }) {
	const navigate = useNavigate();
	const handleLogout = () => {
		localStorage.removeItem('token');
		navigate('/', {state: {
			alert: {type: 'success', message: 'Logout successful.'}
		}});
	};

	return (
		<nav className="nav-bar">
			<div className="nav-left">
				<NavLink className="nav-logo" to="/home">Project Manager</NavLink>
			</div>
		  	<div className="nav-middle">
				<NavLink className={({ isActive }) => `nav-item ${isActive ? 'nav-item-selected' : ''}`} to="/home">Home</NavLink>
				<NavLink className={({ isActive }) => `nav-item ${isActive ? 'nav-item-selected' : ''}`} to="/projects">Projects</NavLink>
				{user.role === 'Teacher' && (
					<NavLink className={({ isActive }) => `nav-item ${isActive ? 'nav-item-selected' : ''}`} to="/project/new">Create New Project</NavLink>
				)}
			</div>
		  	<div className="nav-right">
				<span className="nav-user">
					{user.login} ({user.name} {user.surname})
				</span>
				<span className="nav-role">[{user.role}]</span>
			</div>
			<div className="logout-icon">
					<button onClick={handleLogout}><FontAwesomeIcon icon={faRightFromBracket}/></button>
			</div>
		</nav>
	);
}

export default Navigation;
