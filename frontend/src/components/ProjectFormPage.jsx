import React, { useState, useEffect } from 'react';
import { useNavigate, useParams, useLocation } from 'react-router-dom';
import api from '../api';
import Alert from './Alert';
import LoadingScreen from './LoadingScreen';
import ErrorScreen from './ErrorScreen';
import Navigation from './Navigation';
import "../styles/common.css";
import { getCurrentUser } from '../auth';




function ProjectFormPage({ mode }) {
    const { id } = useParams(); // project id from URL if editing
    const location = useLocation();
	const [alert, setAlert] = useState(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
	const navigate = useNavigate();
    const [user, setUser] = useState(null)
    const [name, setName] = useState('');
    const [course, setCourse] = useState('');
    const [description, setDescription] = useState('');
    const [maxTeamSize, setMaxTeamSize] = useState('');
    const [capacity, setCapacity] = useState('');
    const [deadline, setDeadline] = useState('');

    // passed project from ProjectPage, no need to fetch
    const passedProject = location.state?.project;

    // Fetch project data if needed
    useEffect(() => {
 
        async function fetchProject(fetchedUser) {
            
            setLoading(true);
            try {
                const res = await api.get(`/projects/${id}`);
                setFormFields(res.data, fetchedUser);
            } catch (error) {
                setError({ type: 'Missing data', message: 'Failed to load project data, project you want to edit probably does not exists.' });
            } finally {
                setLoading(false);
            }
        }
    
        function setFormFields(project, fetchedUser) {
            setName(project.name);
            setCourse(project.course);
            setDescription(project.description);
            setMaxTeamSize(project.maxTeamSize);
            setCapacity(project.capacity);
            setDeadline(project.deadline.slice(0, 16));
            
            if (project.owner.id !== fetchedUser?.id) {
                setError({ type: 'Unauthorized action', message: 'You are not authorized to edit this project.' });
            }
        }
        

        function resetFormFields() {
            setName('');
            setCourse('');
            setDescription('');
            setMaxTeamSize('');
            setCapacity('');
            setDeadline('');
        }
        async function initialize() {
            setLoading(true);
    
            try {
                // 1. Fetch user first
                const fetchedUser = await getCurrentUser();
                setUser(fetchedUser);
    
                if (mode === 'edit' && id) {
                    if (passedProject) {
                        setFormFields(passedProject, fetchedUser); // Pass fetchedUser
                    } else {
                        await fetchProject(fetchedUser); // Pass fetchedUser
                    }
                } else if (mode === 'create') {
                    resetFormFields();
                }
            } catch (error) {
                console.error('Initialization error:', error);
                setError({ type: 'Initialization error', message: 'Failed to initialize.' });
            } finally {
                setLoading(false);
            }
        }
    
        initialize()
    }, [mode, id, passedProject]);

    // Handle submission POST/PUT
    const handleSubmit = async (e) => {
		e.preventDefault();

        const projectData = {
            name,
            course,
            description,
            maxTeamSize: parseInt(maxTeamSize, 10),
            capacity: parseInt(capacity, 10),
            deadline: new Date(deadline).toISOString(),
            ownerId: user.id,
        };

        // Checks for submitted data
        if (new Date(deadline) <= new Date()) {
            setAlert({ type: 'error', message: 'Deadline must be in the future.' });
            setLoading(false);
            return;
        }
        if (projectData.capacity <= 0) {
            setAlert({ type: 'error', message: 'Capacity must be greater then zero.' })
            setLoading(false);
            return;
        }
        if (projectData.maxTeamSize <= 0) {
            setAlert({ type: 'error', message: 'Capacity must be greater then zero.' })
            setLoading(false);
            return;
        }

        setLoading(true);

        try {
            let response;
            if (mode === 'edit'){
                // Send PUT
                response = await api.put(`/projects/${id}`, projectData);
            } else {
                // Send POST
                response = await api.post('/projects', projectData);
            }

            if (response.status === 200){
                setAlert({ type: 'success', message: 'Project edited successfully!' });
            } else if (response.status === 201) {
                setAlert({ type: 'success', message: 'Project created successfully!' });
            }
            setTimeout(() => navigate(`/project/${response.data.id}`), 1500);

        } catch (error) {
            if (error.response && error.response.data) {
                setAlert({ type: 'error', message: error.response.data.message || 'Failed to create project.' });
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
                <h2>{mode === 'edit' ? 'Edit Project' : 'Create new project'}</h2>
                <form onSubmit={handleSubmit}>
                <label htmlFor="name">Name of the project:</label>
                <input className="input-empty" id="name" type="text" placeholder="Project name" value={name} onChange={e => setName(e.target.value)} required />
                <label htmlFor="course">Course name:</label>
                <input className="input-empty" id="course" type="text" placeholder="Course name" value={course} onChange={e => setCourse(e.target.value)} required />
                <label htmlFor="description">Project description:</label>
                <textarea className="input-empty" id="description" type="text" placeholder="Description of the project" value={description} onChange={e => setDescription(e.target.value)} required />
                <label htmlFor="maxTeamSize">Maximal team size:</label>
                <input className="input-empty short" id="maxTeamSize" type="number" placeholder="Team size" value={maxTeamSize} onChange={e => setMaxTeamSize(e.target.value)} required />
                <label htmlFor="capacity">Maximal number of teams:</label>
                <input className="input-empty short" id="capacity" type="number" placeholder="Capacity" value={capacity} onChange={e => setCapacity(e.target.value)} required />
                <label htmlFor="deadline">The projects must be submitted by (deadline):</label>
                <input className="input-empty" id="deadline" type="datetime-local" value={deadline} onChange={e => setDeadline(e.target.value)} required />
                <span>
                    <button className="btn-filled-round" type="submit">{mode === 'edit' ? 'Save changes' : 'Create project'}</button>
                    <button className="btn-empty-round" onClick={() => navigate(-1)}>Go back</button>
                </span>
                </form>
            </div>
        </div>
    );
}

export default ProjectFormPage;