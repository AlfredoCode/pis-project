import React, { useEffect, useState } from 'react';
import api from '../api';
import '../styles/solution-detail.css';

function SolutionDetail({ teamId, user }) {
  const [solution, setSolution] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [points, setPoints] = useState(null); // To track the points that the teacher will assign
  const [comment, setComment] = useState(''); // To track the comment input by the teacher
  const [evaluating, setEvaluating] = useState(false); // To manage loading state for the evaluation

  useEffect(() => {
    if (!teamId) return;

    // Fetch the solution data for the given teamId
    api.get(`/teams/${teamId}/solutions`)
      .then((response) => {
        setSolution(response.data[0]);
        setPoints(response.data[0].evaluationPoints); // Initialize points with the current points
        setLoading(false);
      })
      .catch((err) => {
        console.error('Error fetching solution:', err);
        setError('Failed to load solution details.');
        setLoading(false);
      });
  }, [teamId]);

  const handlePointsChange = (e) => {
    setPoints(e.target.value);
  };

  const handleCommentChange = (e) => {
    setComment(e.target.value);
  };

  const handleEvaluate = () => {
    setEvaluating(true);
    
    const evaluationData = {
      points: points,
      comment: comment,
      teacherId: user.id, // Assuming user.id holds the teacher's ID
    };

    api.post('/evaluations', evaluationData)
      .then(() => {
        alert('Evaluation submitted successfully');
        setEvaluating(false);
      })
      .catch((err) => {
        console.error('Error submitting evaluation:', err);
        setEvaluating(false);
        alert('Failed to submit evaluation');
      });
  };

  if (loading) {
    return (
      <div className="solution-detail-wrapper">
        <div className="loading">Loading solution details...</div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="solution-detail-wrapper">
        <div className="error-message">{error}</div>
      </div>
    );
  }

  if (!solution) {
    return (
      <div className="solution-detail-wrapper">
        <div className="error-message">No solution found for this team.</div>
      </div>
    );
  }

  return (
    <div className="solution-detail-wrapper">
      <h2>Solution</h2>
      <div className="solution-detail-card">
        {/* If a solution file exists, provide a download link */}
        {solution.file && (
          <div className="download-section">
            <a
              href={`data:application/octet-stream;base64,${solution.file}`}
              download={`solution-${solution.id}.zip`}
              className="download-button"
            >
              Download
            </a>
          </div>
        )}
        <p><strong>Submission Date:</strong> {new Date(solution.submissionDate).toLocaleString()}</p>
        <p><strong>Evaluation By:</strong> {solution.evaluatedBy}</p>
        
        <div className="points-container">
          <strong>Points:</strong>
          {user.role === 'Teacher' ? (
            <input
              type="number"
              value={points}
              onChange={handlePointsChange}
              className="points-input"
              min={0}
              max={100}
              placeholder='0'
            />
          ) : (
            <span>{solution.evaluationPoints}</span>
          )}
        </div>

        <div className="comment-container">
          <strong>Comment:</strong>
          {user.role === 'Teacher' && (
            <input
              value={comment}
              onChange={handleCommentChange}
              className="comment-input"
              placeholder="Enter your comment"
              type='text'
            />
          )}
        </div>

        

        {/* If the user is a Teacher, show the Evaluate button */}
        {user.role === 'Teacher' && (
          <div className="evaluate-section">
            <button
              className="evaluate-button"
              onClick={handleEvaluate}
              disabled={evaluating}
            >
              {evaluating ? 'Evaluating...' : 'Evaluate'}
            </button>
          </div>
        )}
      </div>
    </div>
  );
}

export default SolutionDetail;
