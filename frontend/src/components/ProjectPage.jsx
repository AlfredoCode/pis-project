import React, { useEffect, useState } from 'react';
import { useParams, useNavigate, useLocation } from 'react-router-dom';
import api from '../api';
import Navigation from '../components/Navigation';
import LoadingScreen from '../components/LoadingScreen';
import Alert from '../components/Alert';
import ProjectDetails from './ProjectDetails';
import TeamList from '../components/TeamList';
import SolutionList from '../components/SolutionList';
import { formatDate } from '../utils/formatDate';
import '../styles/project.css';
import { getCurrentUser } from '../auth';


function ProjectPage() {
    const location = useLocation();
    const { projectId } = useParams(); // project id from URL
    const [alert, setAlert] = useState(location.state?.alert || null);
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();
    
    const [isInv, setIsInv] = useState(false); // True if Student is registerd on project OR Teacher is owner of project
    const [project, setProject] = useState(null);
	const [teams, setTeams] = useState([]);
    const [teamSolution, setTeamSolution] = useState([]); 
	const [solutions, setSolutions] = useState([]);
    const [user, setUser] = useState(null)
    const now = new Date();

	useEffect(() => {
        async function fetchData() {
          setLoading(true);
          let fetchedUser = await getCurrentUser();
          setUser(fetchedUser);
          let isInvTemp = false;
          let studentTeamIdTemp = null;
      
          try {
            // Use fetchedUser.id here, NOT user.id
            const res = await api.get(`/users/${fetchedUser.id}/projects/${projectId}`);
            setProject(res.data);
            isInvTemp = true;
            if (fetchedUser.role === 'Student') {
              setIsInv(true);
              studentTeamIdTemp = res.data.team.id;
            } else if (fetchedUser.role === 'Teacher') {
              setIsInv(fetchedUser.id === res.data.owner.id);
            }
          } catch (err) {
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
          if (fetchedUser.role === 'Student' && isInvTemp && studentTeamIdTemp) {
            const res = await api.get(`/teams/${studentTeamIdTemp}/solutions`);
            setTeamSolution(res.data[0]);
          } else if (fetchedUser.role === 'Teacher' && isInvTemp) {
            const res = await api.get(`/projects/${projectId}/solutions`);
            // Pick just lates solution for each team
            const latestSolutions = {};
            res.data.forEach(solution => {
                const existing = latestSolutions[solution.teamId];
                if (!existing || new Date(solution.submissionDate) > new Date(existing.submissionDate)) {
                    latestSolutions[solution.teamId] = solution;
                }
            });
            const latestSolutionsArray = Object.values(latestSolutions);
            setSolutions(latestSolutionsArray);
          }
      
          setLoading(false);
        }
      
        fetchData();
      }, [projectId]);
      


    // Handle project editing
    const handleProjectEdit = () => {
        navigate(`/project/edit/${project.id}`, { state: { project } });
    };

    // Handle project deleting
    const handleProjectDelete = async () => {
        if (!window.confirm('Are you sure you want to delete this project?')) {
			return;
		}

        setLoading(true);
        try {
            await api.delete(`/projects/${projectId}`);
            navigate('/projects', {state: {
                alert: { type: 'success', message: 'Project deleted successfully!' }
            }});
        } catch (err) {
            console.error('Failed to delete project:', err);
			setAlert({ type: 'error', message: 'Failed to delete the project.' });
        } finally {
            setLoading(false);
        }
    };

    // Handle add solution
    const handleAddSolution = () => {
        navigate(`/team/${project.team.id}`);
    }

    // Handle team creating
    const handleTeamCreate = async () => {
        if (teams.length >= project.capacity){
            setAlert({ type: 'error', message: 'Project capacity is full.' });
            return;
        }

        /// Individual project
        if (project.maxTeamSize === 1) {
            setLoading(true);
            try {
                const teamData = {
                    name: user.name,
                    description: `Individual project, student: ${user.name}`,
                    leaderId: user.id,
                    projectId: project.id
                };
                const res = await api.post('/teams', teamData);
                if (res.status === 201) {
                    navigate(`/team/${res.data.id}`, {state: {
                        alert: { type: 'success', message: 'Project registered successfully!' }
                    }});
                }
            } catch (err) {
                if (err.response && err.response.data) {
                    setAlert({ type: 'error', message: err.response.data.message || 'Project registration failed.' });
                } else {
                    setAlert({ type: 'error', message: 'Server is not responding, registration failed.' });
                }
            } finally {
                setLoading(false);
            }
        } else {
            navigate(`/team/new/${project.id}`, { state: { project: project } });
        }
    }

    return (
        <div className="main-content-wrapper">
            {alert && (<Alert type={alert.type} message={alert.message} duration={3500} onClose={() => setAlert(null)} />)}
            {loading && <LoadingScreen />}
            <Navigation user={user} />
            <div className="page-container">
                <ProjectDetails project={project} />
                {(user && user.role === 'Teacher' && isInv) &&(
                    <div className="teacher-actions">
                        <button className="btn-filled-round" onClick={handleProjectDelete}>Delete this project</button>
                        <button className="btn-filled-round" onClick={handleProjectEdit}>Edit this project</button>
                    </div>
                )}
                {(user && user.role === 'Student' && !isInv) && (
                    <div className="student-actions">
                        <button className="btn-filled-round" onClick={handleTeamCreate}>
                            {project?.maxTeamSize === 1 ? 'Register on this project' : 'Create team for this project'}
                        </button>
                    </div>
                )}
                {(user && user.role === 'Student' && isInv) && (
                    <div className="student-team">
                        <h2>My solution</h2>
                        <ul>
                            {project.maxTeamSize !== 1 && (
                                <>
                                    <li>
                                        <strong>Team name:</strong>
                                        <span>
                                            {project.team.name}
                                            <button className="btn-filled-round" onClick={() => navigate(`/team/${project.team.id}`)}>Team detail</button>
                                        </span>
                                    </li>
                                </>
                            )}
                            <li>
                                <strong>Submission date:</strong> 
                                <span>
                                    {teamSolution.submissionDate ? formatDate(teamSolution.submissionDate) : 'No submissions'}
                                    {teamSolution.submissionDate && (
                                        <button className="btn-filled-round" onClick={() => navigate(`/solution/${teamSolution.teamId}`)}>Solution detail</button>
                                    )}
                                </span>
                            </li>
                            <li><strong>Evaluation:</strong> {teamSolution.evaluationPoints || 'Not evaluated'}</li>
                        </ul>
                        {(new Date(project.deadline) >= now) && (
                            <button className="btn-filled-round" onClick={handleAddSolution}>Add new solution</button>
                        )}
                    </div>
                )}
                <TeamList teams={teams} individual={project?.maxTeamSize === 1} />
                {(user && user.role === 'Teacher' && isInv) && (
                    <div className="solutions">
                        <SolutionList solutions={solutions} individual={project.maxTeamSize === 1} />
                    </div>
                )}
            </div>
        </div>
    );
}

export default ProjectPage;