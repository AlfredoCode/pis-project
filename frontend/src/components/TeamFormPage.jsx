import React, { useState, useEffect } from 'react';
import { useNavigate, useParams, useLocation } from 'react-router-dom';
import api from '../api';
import Alert from './Alert';
import LoadingScreen from './LoadingScreen';
import ErrorScreen from './ErrorScreen';
import Navigation from './Navigation';
import "../styles/common.css";


// DUMMY user data
const user = {
	login: 'alice',
	name: 'Alice',
	surname: 'Wonder',
	role: 'Teacher', // change to 'Teacher' to see other version
	id: 8 // Needed to match with teams or owner
};


function TeamFormPage({ mode }) {
    const { id } = useParams(); // team id from URL, if editing it is teamId, if creating it is projectId !!!
    const location = useLocation();
	const [alert, setAlert] = useState(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
	const navigate = useNavigate();

    const [name, setName] = useState('');
    const [description, setDescription] = useState('');
    const [projectId, setProjectId] = useState('');

    // passed team from TeamPage, no need to fetch
    const passedTeam = location.state?.team;

    // Fetch project data if needed
    useEffect(() => {
        async function fetchProject() {
            setLoading(true);
            try {
                const res = await api.get(`/teams/${id}`);
                setFormFields(res.data);
            } catch (error) {
                setError({ type: 'Missing data', message: 'Failed to load team data, team you want to edit probably does not exists.' });
            } finally {
                setLoading(false);
            }
        }
    
        function setFormFields(team) {
            setName(team.name);
            setDescription(team.description);
            setProjectId(team.project.id);

            if (team.leader.id !== user.id) {
                setError({ type: 'Unauthorized action', message: 'You are not authorized to edit a team for this project.' });
            }
        }

        function resetFormFields() {
            setName('');
            setDescription('');
            setProjectId(id);
        }
        
        if (mode === 'edit' && id) {
            if (passedTeam) {
                setFormFields(passedTeam);
            } else {
                fetchProject();
            }
        } else if (mode === 'create' && id) {
            resetFormFields();
        }
    }, [mode, id, passedTeam]);

    // Handle submission POST/PUT
    const handleSubmit = async (e) => {
		e.preventDefault();

        const teamData = {
            name,
            description,
            leaderId: user.id,
            projectId: projectId
        };
        
        // Checks for submitted data
        if (!teamData.leaderId) {
            setAlert({ type: 'error', message: 'Team leader is not properly set, please fill in the form again.' });
            setLoading(false);
            return;
        }
        if (!teamData.projectId) {
            setAlert({ type: 'error', message: 'Project identifier is not properly set, please fill in the form again.' });
            setLoading(false);
            return;
        }

        setLoading(true);

        try {
            let response;
            if (mode === 'edit'){
                // Send PUT
                response = await api.put(`/teams/${id}`, teamData);
            } else {
                // Send POST
                response = await api.post('/teams', teamData);
            }

            if (response.status === 200){
                setAlert({ type: 'success', message: 'Project edited successfully!' });
            } else if (response.status === 201) {
                setAlert({ type: 'success', message: 'Project created successfully!' });
            }
            setTimeout(() => navigate(`/team/${response.data.id}`), 1500);

        } catch (error) {
            if (error.response && error.response.data) {
                setAlert({ type: 'error', message: error.response.data.message || 'Failed to create team.' });
            } else {
                setAlert({ type: 'error', message: 'Server is not responding.' });
            }

        } finally {
            setLoading(false);
        }
	};

    return (
        <div className="main-content-wrapper">
            {alert && (<Alert type={alert.type} message={alert.message} duration={3500} onClose={() => setAlert(null)} />)}
            {loading && <LoadingScreen />}
            {error && <ErrorScreen type={error.type} message={error.message} />}
            <Navigation user={user} />
            <div className="form-container">
                <h2>{mode === 'edit' ? 'Edit Team' : 'Create new team'}</h2>
                <form onSubmit={handleSubmit}>
                <label htmlFor="name">Name of the team:</label>
                <input className="input-empty" id="name" type="text" placeholder="Team name" value={name} onChange={e => setName(e.target.value)} required />
                <label htmlFor="description">Team Description:</label>
                <textarea className="input-empty" id="description" placeholder="Description" value={description} onChange={e => setDescription(e.target.value)} required/>
                <button className="btn-filled-round" type="submit">{mode === 'edit' ? 'Save changes' : 'Create team'}</button>
                </form>
            </div>
        </div>
    );
}

export default TeamFormPage;