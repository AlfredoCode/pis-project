import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import '../styles/teamdetail.css';
import Navigation from './Navigation';
import api from '../api';
import { FiTrash, FiCheck, FiX } from 'react-icons/fi';
import { LuCrown } from "react-icons/lu";
import SolutionDetail from './SolutionDetail';

function ConfirmModal({ open, onConfirm, onCancel, studentName }) {
  if (!open) return null;

  return (
    <div className="modal-overlay">
      <div className="modal">
        <h3>Remove Student</h3>
        <p>Are you sure you want to remove <strong>{studentName}</strong> from the team?</p>
        <div className="modal-actions">
          <button className="confirm-button" onClick={onConfirm}>Yes, Remove</button>
          <button className="cancel-button" onClick={onCancel}>Cancel</button>
        </div>
      </div>
    </div>
  );
}

// TODO FETCH USER
const user = {
  login: 'alice',
  name: 'Alice',
  surname: 'Wonder',
  role: 'Teacher',
  id: 1,
};

function TeamDetailPage() {
  const { tId } = useParams();
  const navigate = useNavigate();
  const [team, setTeam] = useState(null);
  const [students, setStudents] = useState([]);
  const [signupRequests, setSignupRequests] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [selectedStudent, setSelectedStudent] = useState(null);

  const handleTeamEdit = () => {
    navigate(`/team/edit/${team.id}`, { state: { team } });
  }

  useEffect(() => {
    if (!tId) return;
  
    Promise.all([
      api.get(`/teams/${tId}`),
      api.get(`/teams/${tId}/students`),
      api.get(`/teams/${tId}/signuprequests`),
    ])
      .then(([teamRes, studentsRes, signupRequestsRes]) => {
        setTeam(teamRes.data);
        setStudents(studentsRes.data);
        // Filter only requests with state === "Created"
        const createdRequests = signupRequestsRes.data.filter(req => req.state === "Created");
        setSignupRequests(createdRequests);
        setLoading(false);
      })
      .catch((error) => {
        console.error('Error fetching data:', error);
        setError('Failed to load team details. Please try again later.');
        setLoading(false);
      });
  }, [tId]);
  

  const handleDeleteStudent = async () => {
    if (!selectedStudent) return;
    try {
      await api.delete(`/teams/${tId}/students/${selectedStudent.id}`);
      setStudents((prev) => prev.filter((s) => s.id !== selectedStudent.id));
      setSelectedStudent(null);
    } catch (error) {
      console.error('Failed to delete student:', error);
      setSelectedStudent(null);
      alert('Failed to remove student from the team.');
    }
  };

  const handleAcceptRequest = async (requestId) => {
    try {
      await api.put(`/signuprequests/${requestId}/state`, null, {
        params: { newState: 'Approved' },
      });

      setSignupRequests(prev => prev.filter(r => r.id !== requestId));

      const updatedStudents = await api.get(`/teams/${tId}/students`);
      setStudents(updatedStudents.data);
    } catch (error) {
      console.error('Failed to accept request:', error);
      alert('Failed to accept request.');
    }
  };

  const handleRejectRequest = async (requestId) => {
    try {
      await api.delete(`/signuprequests/${requestId}`);
      setSignupRequests(prev => prev.filter(req => req.id !== requestId));
    } catch (error) {
      console.error('Failed to reject signup request:', error);
      alert('Failed to reject request.');
    }
  };

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
          <div className="team-header">
            <h1 className="team-name">{team.name}</h1>

            {/* Show edit button only for the team leader */}
            {team.leader?.id === user.id && (
              <button className="team-edit-button" onClick={handleTeamEdit} title="Edit Team">
                Edit
              </button>
            )}
          </div>

          <p className="team-description">{team.description || 'No description provided.'}</p>

   

          <div className="info-section">
            <h2>Project Details</h2>
            <ul className="project-details">
              <li><strong>Name:</strong> {team.project?.name}</li>
              <li><strong>Course:</strong> {team.project?.course}</li>
              <li><strong>Description:</strong> {team.project?.description}</li>
              <li><strong>Max Team Size:</strong> {team.project?.maxTeamSize}</li>
              <li><strong>Deadline:</strong> {new Date(team.project?.deadline).toLocaleString()}</li>
              <li><strong>Project Owner:</strong> {team.project?.owner?.fullName}</li>
            </ul>
          </div>
              <div className="info-section">
                <SolutionDetail teamId={team.id} user={user}/>
              </div>
          <div className="info-section">
            <h2>Team Members</h2>
            {students.length > 0 ? (
              <ul className="student-list">
                {students.map((student) => (
                  <li key={student.id} className="student-item">
                    {(team.leader?.id === student.id)  && (
                      <span className='leader-crown'>
                        <LuCrown />
                      </span>
                    )}
                    <span className='student-name'>{student.fullName} ({student.username})</span>

                    {/* Show delete button only for the team leader or for the current user */}
                    {(team.leader?.id === user.id || student.id === user.id || user.role == 'Teacher') && (
                      <button
                        className="delete-button"
                        onClick={() => setSelectedStudent(student)}
                        title="Remove from team"
                      >
                        <FiTrash />
                      </button>
                    )}
                  </li>
                ))}
              </ul>
            ) : (
              <p>No students in this team.</p>
            )}
          </div>

          <div className="info-section">
            <h2>Pending join requests</h2>
            {signupRequests.length > 0 ? (
              <ul className="signup-request-list">
                {signupRequests.map((req) => (
                  <li key={req.id} className="signup-request-item">
                    <span>{req.student.fullName} ({req.student.username})</span>
                    {(user.id == team.leader.id || user.role == 'Teacher') &&
                    <div className="signup-request-actions">
                      <button
                        className="accept-button"
                        onClick={() => handleAcceptRequest(req.id)}
                        title="Accept request"
                      >
                        <FiCheck />
                      </button>
                      <button
                        className="reject-button"
                        onClick={() => handleRejectRequest(req.id)}
                        title="Reject request"
                      >
                        <FiX />
                      </button>
                    </div>
                    }
                  </li>
                ))}
              </ul>
            ) : (
              <p>No pending join requests.</p>
            )}
          </div>
        </div>
      </div>

      {/* Confirm Modal */}
      <ConfirmModal
        open={!!selectedStudent}
        studentName={selectedStudent?.fullName}
        onConfirm={handleDeleteStudent}
        onCancel={() => setSelectedStudent(null)}
      />
    </div>
  );
}

export default TeamDetailPage;
