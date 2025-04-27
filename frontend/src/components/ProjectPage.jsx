import React, { useEffect, useState } from 'react';
import { useParams, useNavigate, useLocation } from 'react-router-dom';
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
    const { projectId } = useParams(); // project id from URL
    const location = useLocation();
    const [alert, setAlert] = useState(null);
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();
    
    const [isInv, setIsInv] = useState(false); // True if Student is registerd on project OR Teacher is owner of project
    const [project, setProject] = useState(null);
	const [teams, setTeams] = useState([]);
	const [solutions, setSolutions] = useState([]);

	useEffect(() => {
		async function fetchData() {
            setLoading(true);

            let isInvTemp = false;
		    let studentTeamIdTemp = null;

			try {
				// Try fetching user-specific project info
				const res = await api.get(`/users/${user.id}/projects/${projectId}`);
				setProject(res.data);
                setIsInv(true);
                isInvTemp = true;
                studentTeamIdTemp = res.data.team.id;
			} catch (err) {
				// If not found, fallback
                if (err.response?.status === 404) {
                    try {
                        const res = await api.get(`/projects/${projectId}`);
                        setProject(res.data);
                        setIsInv(false);
                    } catch (err) {
                        console.error('Error fetching projects:', err);
                    }
                } else {
                    console.error('Error fetching projects:', err);
                }
			}

			// Always fetch teams
            try {
                const res = await api.get(`/projects/${projectId}/teams`);
                setTeams(res.data);
            } catch (err) {
                console.error('Error fetching teams:', err);
            }

			// Fetch solutions if needed
			if (user.role === 'Student' && isInvTemp && studentTeamIdTemp) {
				const res = await api.get(`/teams/${studentTeamIdTemp}/solutions`);
				setSolutions(res.data);
			} else if (user.role === 'Teacher' && isInvTemp) {
				const res = await api.get(`/projects/${projectId}/solutions`);
				setSolutions(res.data);
			}

			setLoading(false);
		}

		fetchData();
	}, [projectId, user]);


    // Handle project editing
    const handleEdit = () => {
        navigate(`/project/edit/${project.id}`, { state: { project } });
    };

    // Handle team creating
    const handleTeamCreate = () => {
        navigate(`/team/new/${project.id}`, { state: { project: project } });
    }

    // Handle team editing
    const handleTeamEdit = () => {
        navigate(`/team/edit/${team.id}`, { state: { project: project, team: team } });
    }

    console.log(project);
    console.log(teams);
    console.log(solutions);

    return (
        <div className="main-content-wrapper">
            {alert && (<Alert type={alert.type} message={alert.message} duration={3500} onClose={() => setAlert(null)} />)}
            {loading && <LoadingScreen />}
            <Navigation user={user} />

        </div>
    );
}

export default ProjectPage;