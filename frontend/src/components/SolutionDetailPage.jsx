import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { FaArrowRight } from 'react-icons/fa';
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
  const [solutionId, setSolutionId] = useState(null);
  const { teamId } = useParams();
  const navigate = useNavigate();
  const [alert, setAlert] = useState(location.state?.alert || null);

  useEffect(() => {
    const fetchUser = async () => {
      const fetchedUser = await getCurrentUser();
      setUser(fetchedUser);
    };

    const fetchTeamAndSolution = async () => {
      try {
        if (!teamId) return;
        
        const teamResponse = await api.get(`/teams/${teamId}`);
        const fetchedTeam = teamResponse.data;
        console.log(fetchedTeam)
        setTeam({
          teamId: fetchedTeam.id,
          teamName: fetchedTeam.name,
        });
        
        if (fetchedTeam?.lastSolution?.id) {
          const solutionId = fetchedTeam.lastSolution.id;
          setSolutionId(solutionId);

          const solutionResponse = await api.get(`/solutions/${solutionId}`);
          const fetchedSolution = solutionResponse.data;

          if (fetchedSolution) {
            setProjectId(fetchedSolution.projectId);
          }
        }
      } catch (err) {
        console.error('Error fetching team or solution data:', err);
      }
    };

    fetchUser();
    fetchTeamAndSolution();
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
      {team ? (
        <div className="content-solution">
          <h1>
            Team: {team.teamName}
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
          {projectId ? (

          <SolutionDetail
            user={user}
            teamId={team.teamId}
            projectId={projectId}
            history
          />
          ) :
          <span>This team has not submitted any solution!</span>}
        </div>
      ) : null

    }
      
    </div>
  );
}

export default SolutionDetailPage;
