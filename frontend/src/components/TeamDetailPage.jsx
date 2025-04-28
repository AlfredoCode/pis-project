import React, { useEffect, useRef, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import api from '../api';
import Navigation from './Navigation';
import { FiTrash, FiCheck, FiX } from 'react-icons/fi';
import { LuCrown } from "react-icons/lu";
import SolutionDetail from './SolutionDetail';
import ProjectDetails from './ProjectDetails';
import Alert from './Alert';
import { getCurrentUser } from '../auth';

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

function DisbandConfirmModal({ open, onConfirm, onCancel }) {
  if (!open) return null;
  return (
    <div className="modal-overlay">
      <div className="modal">
        <h3>Disband Team</h3>
        <p>Are you sure you want to disband this team? <strong>This action cannot be undone.</strong></p>
        <div className="modal-actions">
          <button className="confirm-button" onClick={onConfirm}>Yes, Disband</button>
          <button className="cancel-button" onClick={onCancel}>Cancel</button>
        </div>
      </div>
    </div>
  );
}



function TeamDetailPage() {
  const { tId } = useParams();
  const navigate = useNavigate();
  const [team, setTeam] = useState(null);
  const [students, setStudents] = useState([]);
  const [signupRequests, setSignupRequests] = useState([]);
  const [userSignupRequests, setUserSignupRequests] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [selectedStudent, setSelectedStudent] = useState(null);
  const [project, setProject] = useState(null);
  const [isUserMember, setIsUserMember] = useState(false);
  const [showDisbandConfirm, setShowDisbandConfirm] = useState(false);
  const [alert, setAlert] = useState(null);
  const [contextMenuStudent, setContextMenuStudent] = useState(null);
  const menuRef = useRef(null);
  const [user, setUser] = useState(null)
 
  const handleOpenContextMenu = (student) => {
    if (contextMenuStudent?.id === student.id) {
      setContextMenuStudent(null);
    } else {
      setContextMenuStudent(student);
    }
  };
  useEffect(() => {
    const handleClickOutside = (event) => {
      if (menuRef.current && !menuRef.current.contains(event.target)) {
        setContextMenuStudent(null);
      }
    };
  
    if (contextMenuStudent) {
      document.addEventListener('mousedown', handleClickOutside);
    } else {
      document.removeEventListener('mousedown', handleClickOutside);
    }
  
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, [contextMenuStudent]);
  
  const handleTransferLeadership = async (student) => {
    try {
      const updatedTeamData = {
        name: team.name,
        description: team.description,
        leaderId: student.id,
        projectId: team.project.id,
      };
  
      await api.put(`/teams/${team.id}`, updatedTeamData);
  
      const updatedTeam = await api.get(`/teams/${team.id}`);
      setTeam(updatedTeam.data);
      setContextMenuStudent(null);
      showAlert('success', `Leadership transferred to ${student.fullName}`);
    } catch (error) {
      console.error(error);
      showAlert('error', 'Failed to transfer leadership');
    }
  };
  
  const showAlert = (type, message, duration = 3000) => {
    setAlert({ type, message, duration });
  };

  const handleCancelApplication = async () => {
    const application = userSignupRequests.find(req => req.team.id === team.id && req.state === 'Created');
    if (application) {
      try {
        await api.delete(`/signuprequests/${application.id}`);
        setUserSignupRequests(prev => prev.filter(req => req.id !== application.id));
        showAlert('success', 'Application canceled');
      } catch (error) {
        showAlert('error', 'Failed to cancel application');
      }
    } else {
      showAlert('info', 'You have no application to cancel');
    }
  };

  const handleTeamEdit = () => {
    navigate(`/team/edit/${team.id}`, { state: { team } });
  };

  useEffect(() => {
    async function fetchAllData() {
      if (!tId) return;
  
      setLoading(true);
      try {
        // First, fetch user
        const fetchedUser = await getCurrentUser();
        setUser(fetchedUser);
  
        // Then fetch team info
        const teamRes = await api.get(`/teams/${tId}`);
        setTeam(teamRes.data);
  
        const projectId = teamRes.data.project.id;
  
        // Fetch project, students, signup requests at the same time
        const [projectRes, studentsRes, signupRequestsRes] = await Promise.all([
          api.get(`/projects/${projectId}`),
          api.get(`/teams/${tId}/students`),
          api.get(`/teams/${tId}/signuprequests`)
        ]);
  
        setStudents(studentsRes.data);
  
        const createdRequests = signupRequestsRes.data.filter(req => req.state === "Created");
        setSignupRequests(createdRequests);
  
        setProject(projectRes.data);
  
        // Finally, now that user is loaded, fetch user-specific signup requests
        const userSignupRequestsRes = await api.get(`/students/${fetchedUser.id}/signuprequests`);
        setUserSignupRequests(userSignupRequestsRes.data);
  
      } catch (error) {
        console.error('Error fetching data:', error);
        setError('Failed to load team details. Please try again later.');
      } finally {
        setLoading(false);
      }
    }
  
    fetchAllData();
  }, [tId]);
  

  useEffect(() => {
    if (students && user) {
      const isMemberOfTeam = students.some(student => student.id === user.id) || user?.role === "Teacher";
      setIsUserMember(isMemberOfTeam);
      console.log(students, user)
    }
  }, [user, students]);

  const handleDeleteStudent = async () => {
    if (!selectedStudent) return;
    try {
      await api.delete(`/teams/${tId}/students/${selectedStudent.id}`);
      setStudents((prev) => prev.filter((s) => s.id !== selectedStudent.id));
      setSelectedStudent(null);
    } catch (error) {
      setSelectedStudent(null);
      if(selectedStudent.id == team.leader.id){
        showAlert('error', 'Cannot remove leader of the team! Transfer the leadership first!');

      }
      else{

        showAlert('error', 'Failed to remove student');
      }
    }
  };

  const confirmDisbandTeam = async () => {
    try {
      await api.delete(`/teams/${tId}`);
      navigate(`/project/${project.id}`);
    } catch (error) {
      showAlert('error', 'Team disband failed!');
    } finally {
      setShowDisbandConfirm(false);
    }
  };

  const handleAcceptRequest = async (requestId) => {
    try {
      await api.put(`/signuprequests/${requestId}/state`, null, {
        params: { newState: 'Approved' },
      });
      setSignupRequests(prev => prev.filter(r => r.id !== requestId));
      showAlert('success', 'Request accepted');
      const updatedStudents = await api.get(`/teams/${tId}/students`);
      setStudents(updatedStudents.data);
    } catch (error) {
      showAlert('error', 'Failed to accept request');
    }
  };

  const handleDisbandTeam = () => {
    setShowDisbandConfirm(true);
  };

  const handleApplyToTeam = async () => {
    try {
      const response = await api.post(`/signuprequests`, {
        teamId: team.id,
        studentId: user.id,
      });
  
      const newSignupRequest = response.data;
  
      showAlert('success', 'Request sent');
  
      setUserSignupRequests(prev => [...prev, newSignupRequest]);
  
    } catch (error) {
      showAlert('error', 'Failed to send the request, you are probably already in a team!');
    }
  };
  
  useEffect(() => {
    console.log(userSignupRequests)
  }, [userSignupRequests])
  const handleRejectRequest = async (requestId) => {
    try {
      await api.delete(`/signuprequests/${requestId}`);
      setSignupRequests(prev => prev.filter(req => req.id !== requestId));
      showAlert('success', 'Request rejected');
    } catch (error) {
      showAlert('error', 'Failed to reject the request');
    }
  };

  const hasAlreadyApplied = userSignupRequests.some(req => req.team.id === team.id && req.state === 'Created');

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
      {alert && (
        <Alert
          type={alert.type}
          message={alert.message}
          duration={alert.duration}
          onClose={() => setAlert(null)}
        />
      )}
      <div className="page-content">
        <button className="back-button" onClick={() => navigate(`/project/${project.id}`)}>← Back to project</button>
        <div className="team-detail-card">
          <div className="team-header">
            <h1 className="team-name">{team.name}</h1>

            {user.role === "Student" && !isUserMember && students.length != project.maxTeamSize &&(
              hasAlreadyApplied ? (
                <div className="already-applied-text">
                  <button
                    className="team-edit-button cancel-application-button"
                    onClick={handleCancelApplication}
                    title="Cancel application"
                  >
                    Cancel application
                  </button>
                </div>
              ) : (
                <button
                  className="apply-button team-edit-button"
                  onClick={handleApplyToTeam}
                  title="Apply to join this team"
                >
                  Send request to join
                </button>
              )
            )}

            {(team && user) && (team.leader?.id === user.id || user.role === "Teacher") && (
              <div className='team-administration' style={{ display: 'flex', gap: 20 }}>
                <button className="team-edit-button team-disband-button" onClick={handleDisbandTeam}>
                  Disband team
                </button>
                <button className="team-edit-button" onClick={handleTeamEdit}>
                  Edit
                </button>
              </div>
            )}
          </div>

          <p className="team-description">{team.description || 'No description provided.'}</p>

          <ProjectDetails project={project} />

          {isUserMember && (
            <div className="info-section">
              <SolutionDetail teamId={team.id} user={user} projectId={team.project.id}/>
            </div>
          )}

          <div className="info-section">
            <h2>Team Members</h2>
            {students.length > 0 ? (
              <ul className="student-list">
                {students.map((student) => (
                  <li key={student.id} className="student-item">
                    {(team.leader?.id === student.id) && (
                      <span className='leader-crown'><LuCrown /></span>
                    )}
                      <span className='student-name'>{student.fullName} ({student.username})</span>
                    <div className='team-right-options'>
                      {/* Transfer leadership menu for leader */}
                      {(team.leader?.id === user.id && student.id !== user.id) && (
                        <div className="context-menu-wrapper">
                          <button
                            className="context-menu-button"
                            onClick={() => handleOpenContextMenu(student)}
                            title="More actions"
                          >
                            &#x22EE; {/* Vertical 3 dots */}
                          </button>

                          {/* Context menu shown when selected */}
                          {contextMenuStudent?.id === student.id && (
                            <div className="context-menu">
                              <button
                                onClick={() => handleTransferLeadership(student)}
                                className="context-menu-item"
                                ref={menuRef}
                              >
                                Transfer leadership
                              </button>
                            </div>
                          )}
                        </div>
                      )}
                      {/* Delete button for leader/self/teacher */}
                      {(team.leader?.id === user.id || student.id === user.id || user.role === 'Teacher') && (
                        <button
                          className="delete-button"
                          onClick={() => setSelectedStudent(student)}
                          title="Remove from team"
                        >
                          <FiTrash />
                        </button>
                      )}
                      
                    </div>

              </li>
          ))}

              </ul>
            ) : (
              <p>No students in this team.</p>
            )}
          </div>

          {isUserMember && (
            <div className="info-section">
              <h2>Pending join requests</h2>
              {signupRequests.length > 0 ? (
                <ul className="signup-request-list">
                  {signupRequests.map((req) => (
                    <li key={req.id} className="signup-request-item">
                      <span>{req.student.fullName} ({req.student.username})</span>
                      {(user &&team )&&(user.id === team.leader.id || user.role === 'Teacher') && (
                        <div className="signup-request-actions">
                          <button className="accept-button" onClick={() => handleAcceptRequest(req.id)} title="Accept request">
                            <FiCheck />
                          </button>
                          <button className="reject-button" onClick={() => handleRejectRequest(req.id)} title="Reject request">
                            <FiX />
                          </button>
                        </div>
                      )}
                    </li>
                  ))}
                </ul>
              ) : (
                <p>No pending join requests.</p>
              )}
            </div>
          )}
        </div>
      </div>

      {/* Confirm Modals */}
      <ConfirmModal
        open={!!selectedStudent}
        studentName={selectedStudent?.fullName}
        onConfirm={handleDeleteStudent}
        onCancel={() => setSelectedStudent(null)}
      />
      <DisbandConfirmModal
        open={showDisbandConfirm}
        onConfirm={confirmDisbandTeam}
        onCancel={() => setShowDisbandConfirm(false)}
      />
    </div>
  );
}

export default TeamDetailPage;
