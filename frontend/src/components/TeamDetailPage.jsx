import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import '../styles/teamdetail.css';
import Navigation from './Navigation';
import api from '../api';

const user = {
  login: 'alice',
  name: 'Alice',
  surname: 'Wonder',
  role: 'Student',
  id: 2,
};

function TeamDetailPage() {
  const { tId } = useParams();
  const navigate = useNavigate();
  const [team, setTeam] = useState(null);
  const [students, setStudents] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (!tId) return;

    Promise.all([
      api.get(`/teams/${tId}`),
      api.get(`/teams/${tId}/students`)
    ])
      .then(([teamRes, studentsRes]) => {
        setTeam(teamRes.data);
        setStudents(studentsRes.data);
        setLoading(false);
      })
      .catch(error => {
        console.error('Error fetching data:', error);
        setError('Failed to load team details. Please try again later.');
        setLoading(false);
      });
  }, [tId]);

  if (loading) {
    return (
      <div className='main-content-wrapper'>
        <Navigation user={user} />
        <div className="page-content">
          <div className="loading">Loading team details...</div>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className='main-content-wrapper'>
        <Navigation user={user} />
        <div className="page-content">
          <div className="error-message">{error}</div>
          <button className="back-button" onClick={() => navigate(-1)}>Back</button>
        </div>
      </div>
    );
  }

  if (!team) {
    return (
      <div className='main-content-wrapper'>
        <Navigation user={user} />
        <div className="page-content">
          <div className="error-message">Team not found.</div>
          <button className="back-button" onClick={() => navigate(-1)}>Back</button>
        </div>
      </div>
    );
  }

  return (
    <div className='main-content-wrapper'>
      <Navigation user={user} />
      <div className="page-content">
        <button className="back-button" onClick={() => navigate(-1)}>← Back to Teams</button>

        <div className="team-detail-card">
          <h1 className="team-name">{team.name}</h1>
          <p className="team-description">{team.description || 'No description provided.'}</p>

          <div className="info-section">
            <h2>Leader</h2>
            <p>{team.leader?.fullName} ({team.leader?.username})</p>
          </div>

          <div className="info-section">
            <h2>Project Details</h2>
            <ul className="project-details">
              <li><strong>Name:</strong> {team.project?.name}</li>
              <li><strong>Course:</strong> {team.project?.course}</li>
              <li><strong>Description:</strong> {team.project?.description}</li>
              <li><strong>Max Team Size:</strong> {team.project?.maxTeamSize}</li>
              <li><strong>Capacity:</strong> {team.project?.capacity}</li>
              <li><strong>Deadline:</strong> {new Date(team.project?.deadline).toLocaleString()}</li>
              <li><strong>Project Owner:</strong> {team.project?.owner?.fullName}</li>
            </ul>
          </div>

          <div className="info-section">
            <h2>Team Members</h2>
            {students.length > 0 ? (
              <ul className="student-list">
                {students.map((student) => (
                  <li key={student.id}>
                    {student.fullName} ({student.username})
                  </li>
                ))}
              </ul>
            ) : (
              <p>No students in this team.</p>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}

export default TeamDetailPage;
