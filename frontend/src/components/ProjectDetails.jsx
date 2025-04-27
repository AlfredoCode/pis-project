import React from 'react'

function ProjectDetails({project}) {
  return (
    <><h2>Project Details</h2>
    <ul className="project-details">
      <li><strong>Name:</strong> {project?.name}</li>
      <li><strong>Course:</strong> {project?.course}</li>
      <li><strong>Description:</strong> {project?.description}</li>
      <li><strong>Max Team Size:</strong> {project?.maxTeamSize}</li>
      <li><strong>Deadline:</strong> {new Date(project?.deadline).toLocaleString()}</li>
      <li><strong>Project Owner:</strong> {project?.owner?.fullName}</li>
    </ul></>
  )
}

export default ProjectDetails