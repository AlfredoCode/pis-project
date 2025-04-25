import React from 'react';
import { Link } from 'react-router-dom';
import '../styles/navigation.css';

function Navigation({ user }) {
	return (
		<nav className="nav-bar">
			<div className="nav-left">
				<Link className="nav-logo" to="/home">Project Manager</Link>
			</div>
		  	<div className="nav-middle">
				<Link className="nav-item nav-item-selected" to="/home">Home</Link>
				{user.role === 'Student' && (
					<Link className="nav-item" to="/projects">Projects</Link>
				)}
				{user.role === 'Teacher' && (
					<Link className="nav-item" to="/projects/new">Create New Project</Link>
				)}
			</div>
		  	<div className="nav-right">
				<span className="nav-user">
					{user.login} ({user.name} {user.surname})
				</span>
				<span className="nav-role">[{user.role}]</span>
			</div>
		</nav>
	);
}

export default Navigation;
