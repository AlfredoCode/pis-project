import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import api from '../api';
import Navigation from '../components/Navigation';
import LoadingScreen from '../components/LoadingScreen';
//import ProjectHeader from '../components/ProjectHeader';
//import TeamList from '../components/TeamList';
//import SolutionList from '../components/SolutionList';
import Alert from '../components/Alert';
import { formatDate } from '../utils/formatDate';
import '../styles/project.css';


// DUMMY user data
const user = {
	login: 'alice',
	name: 'Alice',
	surname: 'Wonder',
	role: 'Student', // change to 'Teacher' to see other version
	id: 2 // Needed to match with teams or owner
};


function ProjectPage() {
    const [alert, setAlert] = useState(null);
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();

    // Handle project editing
    const handleEdit = () => {
        navigate(`/project/edit/${project.id}`, { state: { project } });
    };

    return (
        <div className="main-content-wrapper">
            {alert && (<Alert type={alert.type} message={alert.message} duration={3500} onClose={() => setAlert(null)} />)}
            {loading && <LoadingScreen />}
            <Navigation user={user} />

        </div>
    );
}

export default ProjectPage;