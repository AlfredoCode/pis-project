import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { FaArrowRight } from 'react-icons/fa'; // Import an icon
import SolutionDetail from './SolutionDetail';
import { getCurrentUser } from '../auth';
import api from '../api';
import Navigation from './Navigation';
import Alert from './Alert';
import '../styles/solution-page.css';

function SolutionDetailPage() {
  const [team, setTeam] = useState(null);
  const [projectId, setProjectId] = useState(null);
  const [user, setUser] = useState(null);
  const { solutionId } = useParams();
  const navigate = useNavigate();
  const [alert, setAlert] = useState(location.state?.alert || null);

  useEffect(() => {
    const fetchUser = async () => {
      const fetchedUser = await getCurrentUser();
      setUser(fetchedUser);
    };

    const fetchTeamIdFromSolution = async () => {
      try {
        if (!solutionId) return;
        const solutionResponse = await api(`/solutions/${solutionId}`);
        const fetchedSolution = solutionResponse.data;

        if (fetchedSolution && fetchedSolution.teamId) {
          setTeam({
            teamId: fetchedSolution.teamId,
            teamName: fetchedSolution.teamName,
          });
          setProjectId(fetchedSolution.projectId);
        }
      } catch (err) {
        // Handle error silently or log
      }
    };

    fetchTeamIdFromSolution();
    fetchUser();
  }, []);

  const handleTeamClick = () => {
    if (team?.teamId) {
      navigate(`/team/${team.teamId}`);
    }
  };

  return (
    <div className="main-content-wrapper">
      <Navigation user={user} />
      {alert && (
        <Alert
          type={alert.type}
          message={alert.message}
          duration={alert.duration}
          onClose={() => setAlert(null)}
        />
      )}
      {team && team.teamId ? (
        <div className="content-solution">
          <h1>
            Team: {team.teamName}{' '}
            <FaArrowRight
              size={20}
              onClick={handleTeamClick}
              title="Go to team page"
              style={{
                cursor: 'pointer',
                color: '#007bff',
                marginLeft: '8px',
              }}
            />
          </h1>
          <SolutionDetail user={user} teamId={team.teamId} projectId={projectId} history/>
        </div>
      ) : null}
    </div>
  );
}

export default SolutionDetailPage;
