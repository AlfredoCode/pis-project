import React from 'react';
import { useNavigate } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faClock, faBook, faFile, faPlus, faUsers } from '@fortawesome/free-solid-svg-icons'
import '../styles/card-container.css';

function ProjectCardStudent({ project }) {
	const navigate = useNavigate();
	return (
		<div className="project-card">
			<div className="card-label"><h3>{project.name}</h3><div className="card-id">{project.id}</div></div>
			<div className="card-line"><FontAwesomeIcon icon={faBook}/><p>Course: {project.course}</p></div>
			<div className="card-line"><FontAwesomeIcon icon={faClock}/><p>Deadline: {project.deadline}</p></div>
			<div className="card-line"><FontAwesomeIcon icon={faFile}/><p>Submitted: {project.submission_date !== null ? project.submission_date : 'No submission'}</p></div>
			<div className="card-line"><FontAwesomeIcon icon={faPlus}/><p>Evaluation: {project.points !== null ? project.points : 'None'}</p></div>
			<div className="card-line">
				{project.isTeamProject && (
				<button className="btn-filled-round" onClick={() => navigate(`/projects/${project.id}/team`)}>
					Team Page
				</button>
				)}
				<button className="btn-filled-round" onClick={() => navigate(`/projects/${project.id}`)}>
					Details
				</button>
			</div>
		</div>
	);
}


function ProjectCardTeacher({ project }) {
	const navigate = useNavigate();
	return (
		<div className="project-card">
			<div className="card-label"><h3>{project.name}</h3><div className="card-id">{project.id}</div></div>
			<div className="card-line"><FontAwesomeIcon icon={faBook}/><p>Course: {project.course}</p></div>
			<div className="card-line"><FontAwesomeIcon icon={faClock}/><p>Deadline: {project.deadline}</p></div>
			<div className="card-line"><FontAwesomeIcon icon={faUsers}/><p>Registered: {project.registered}/{project.capacity}</p></div>
			<div className="card-line"><FontAwesomeIcon icon={faPlus}/><p>Evaluations: {project.evaluations}/{project.submissions}</p></div>
			<div className="card-line">
				{project.isTeamProject && (
				<button className="btn-filled-round" onClick={() => navigate(`/projects/${project.id}/team`)}>
					Team Page
				</button>
				)}
				<button className="btn-filled-round" onClick={() => navigate(`/projects/${project.id}`)}>
					Details
				</button>
			</div>
		</div>
	);
}



export {ProjectCardStudent, ProjectCardTeacher};