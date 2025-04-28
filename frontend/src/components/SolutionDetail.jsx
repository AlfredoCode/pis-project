import React, { useEffect, useState, useRef } from 'react';
import api from '../api';
import Alert from './Alert'; // Import the Alert component
import '../styles/solution-detail.css';

function SolutionDetail({ teamId, user, projectId }) {
  const [solution, setSolution] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [points, setPoints] = useState(null);
  const [comment, setComment] = useState('');
  const [evaluating, setEvaluating] = useState(false);
  const [solutionHistory, setSolutionHistory] = useState([]);
  const [selectedFile, setSelectedFile] = useState(null);
  const [fileExtension, setFileExtension] = useState('');
  const [showAlert, setShowAlert] = useState(false); // State to show alert
  const [alertMessage, setAlertMessage] = useState('');
  const [alertType, setAlertType] = useState('info');
  
  // Create a ref for the file input
  const fileInputRef = useRef(null);

  async function fetchSolutionAndEvaluation() {
    try {
      const teamResponse = await api.get(`/teams/${teamId}/`);
      const lastSolution = teamResponse.data.lastSolution;
      setSolution(lastSolution);

      // Fetching solution history
      const solutionsResponse = await api.get(`/teams/${teamId}/solutions`);
      setSolutionHistory(solutionsResponse.data);

      if (lastSolution && lastSolution.id) {
        try {
          const evaluationResponse = await api.get(`/solutions/${lastSolution.id}/evaluation`);
          const evaluation = evaluationResponse?.data;

          setPoints(evaluation?.points);
          setComment(evaluation?.comment);
        } catch (evaluationError) {
          setError('Failed to load evaluation details.');
        }
      }

      setLoading(false);
    } catch (teamError) {
      setError('Failed to load solution details.');
      setLoading(false);
    }
  }

  useEffect(() => {
    if (!teamId) return;
    fetchSolutionAndEvaluation();
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
      teacherId: user.id,
      solutionId: solution.id,
    };

    api.post('/evaluations', evaluationData)
      .then(() => {
        alert('Evaluation submitted successfully');
        setEvaluating(false);
      })
      .catch((err) => {
        setEvaluating(false);
        alert('Failed to submit evaluation');
      });
  };

  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onloadend = () => {
        setSelectedFile(reader.result.split(',')[1]); // Removing the "data:*/*;base64," part
        setFileExtension(file.name.split('.').pop()); // Extracting the file extension
      };
      reader.readAsDataURL(file);
    }
  };

  const handleUploadSolution = async () => {
    if (!selectedFile) {
      alert('No file selected');
      return;
    }

    try {
      const data = {
        file: selectedFile, // Base64 encoded file content
        fileExtension: fileExtension, // Sending the file extension
        teamId: teamId, // Team ID
        projectId: projectId, // Project ID
      };

      await api.post('/solutions', data);
      
      // Show success alert
      setAlertMessage('Solution uploaded successfully');
      setAlertType('success');
      setShowAlert(true);

      // Reset the file input field using the ref
      fileInputRef.current.value = '';  // Reset file input field
      setSelectedFile(null); // Clear selectedFile state
      setFileExtension(''); // Clear file extension

      // Fetch updated solution and history
      fetchSolutionAndEvaluation();
    } catch (err) {
      // Show error alert
      setAlertMessage('Failed to upload solution');
      setAlertType('error');
      setShowAlert(true);
    }
  };

  // Function to download the file using a Blob
  const downloadFile = (base64Data, filename, mimeType = 'application/octet-stream') => {
    const byteCharacters = atob(base64Data); // Decode base64
    const byteArrays = [];

    for (let offset = 0; offset < byteCharacters.length; offset += 1024) {
      const slice = byteCharacters.slice(offset, Math.min(offset + 1024, byteCharacters.length));
      const byteNumbers = new Array(slice.length);

      for (let i = 0; i < slice.length; i++) {
        byteNumbers[i] = slice.charCodeAt(i);
      }

      const byteArray = new Uint8Array(byteNumbers);
      byteArrays.push(byteArray);
    }

    const blob = new Blob(byteArrays, { type: mimeType });
    const link = document.createElement('a');
    link.href = URL.createObjectURL(blob); // Create object URL for the blob
    link.download = filename;
    link.click(); // Trigger the download
  };

  // Dynamically determine MIME type based on file extension
  const getMimeTypeFromExtension = (extension) => {
    const mimeTypes = {
      txt: 'text/plain',
      pdf: 'application/pdf',
      docx: 'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
      pptx: 'application/vnd.openxmlformats-officedocument.presentationml.presentation',
      xlsx: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      zip: 'application/zip',
      // Add more file types as needed
    };
    return mimeTypes[extension] || 'application/octet-stream'; // Default to 'application/octet-stream' if unknown
  };

  const handleDownload = (fileBase64, fileName, extension) => {
    const mimeType = getMimeTypeFromExtension(extension);
    if (fileBase64) {
      downloadFile(fileBase64, `${fileName}.${extension}`, mimeType);
    } else {
      alert('No file available to download');
    }
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

  return (
    <div className="solution-detail-wrapper">
      {showAlert && <Alert type={alertType} message={alertMessage} onClose={() => setShowAlert(false)} />}
      <h2>Solution</h2>
      <div className="solution-detail-card">
        {solution ? (
          <>
            {user.role === 'Student' && (
              <div className="upload-solution-section">
                <input
                  ref={fileInputRef} // Attach the ref to the input field
                  type="file"
                  onChange={handleFileChange}
                  className="file-input"
                />
                <button
                  className="upload-button"
                  onClick={handleUploadSolution}
                  disabled={!selectedFile}
                >
                  Upload Solution
                </button>
              </div>
            )}
            <div className="download-solution">
              <p><strong>Submission Date:</strong> {new Date(solution.submissionDate).toLocaleString()} </p>
              {solution.file && solution.fileExtension && (
                <div className="download-section">
                  <button
                    className="download-button"
                    onClick={() => handleDownload(solution.file, `solution-${solution.id}`, solution.fileExtension)}
                  >
                    Download
                  </button>
                </div>
              )}
            </div>
            <p><strong>Evaluated By:</strong> {solution.evaluatedBy}</p>

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
                  placeholder="0"
                />
              ) : (
                <span>{solution.evaluationPoints}</span>
              )}
            </div>

            <div className="comment-container">
              <strong>Comment:</strong>
              {user.role === 'Teacher' ? (
                <input
                  value={comment}
                  onChange={handleCommentChange}
                  className="comment-input"
                  placeholder="Enter your comment"
                  type="text"
                />
              ) : (
                <span>{comment}</span>
              )}
            </div>

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
          </>
        ) : (
          user.role === 'Student' && (
            <div className="upload-solution-section">
              <input
                ref={fileInputRef} // Attach the ref to the input field
                type="file"
                onChange={handleFileChange}
                className="file-input"
              />
              <button
                className="upload-button"
                onClick={handleUploadSolution}
                disabled={!selectedFile}
              >
                Upload Solution
              </button>
            </div>
          )
        )}

        {solutionHistory.length > 0 && (
          <div className="solution-history-section">
            <h3>Solution History</h3>
            <div className="solution-history-list">
              {solutionHistory.map((historyItem) => (
                <div key={historyItem.id} className="solution-history-item">
                  <p><strong>Submission Date:</strong> {new Date(historyItem.submissionDate).toLocaleString()}</p>
                  {historyItem.file && historyItem.fileExtension && (
                    <div className="download-section">
                      <button
                        className="download-button history-download"
                        onClick={() => handleDownload(historyItem.file, `solution-history-${historyItem.id}`, historyItem.fileExtension)}
                      >
                        Download
                      </button>
                    </div>
                  )}
                </div>
              ))}
            </div>
          </div>
        )}
      </div>
    </div>
  );
}

export default SolutionDetail;
