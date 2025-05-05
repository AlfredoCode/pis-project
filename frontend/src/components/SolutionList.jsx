import React, { useState, useMemo } from 'react';
import { useNavigate } from 'react-router-dom';
import { SearchBar, FilterSelect } from './FilterTools';
import { filterSolutions } from '../utils/filterSolutions';
import { formatDate } from '../utils/formatDate';
import '../styles/table.css';
import '../styles/common.css';


function SolutionList({ solutions, individual = false }) {
	const [searchTerm, setSearchTerm] = useState('');
	const [sortOption, setSortOption] = useState('date-desc');
	const navigate = useNavigate();

	const sortOptions = [
		{ value: 'date-desc', label: 'Newest First' },
		{ value: 'date-asc', label: 'Oldest First' },
		{ value: 'teamname-asc', label: 'Name A-Z' },
		{ value: 'teamname-desc', label: 'Name Z-A' },
		{ value: 'points-desc', label: 'Points High-Low' },
		{ value: 'points-asc', label: 'Points Low-High' },
	];

	const displayedSolutions = useMemo(() => {
		return filterSolutions(solutions, { searchTerm, sortOption });
	}, [solutions, searchTerm, sortOption]);

	return (
		<div className="table-container">
			<h2>Submitted solutions</h2>
			<div className="filter-tools">
				<SearchBar value={searchTerm} onChange={setSearchTerm} placeholder="Search by team..." />
				<FilterSelect value={sortOption} onChange={setSortOption} options={sortOptions} />
			</div>
			<table className="styled-table">
				<thead>
					<tr>
						<th>{individual ? 'Student' : 'Team Name'}</th>
						<th>Submission Date</th>
						<th>Points</th>
						<th>Actions</th>
					</tr>
				</thead>
				<tbody>
					{displayedSolutions.map((solution) => (
						<tr key={solution.id}>
							<td>{solution.teamName}</td>
							<td>{formatDate(solution.submissionDate)}</td>
							<td>{solution.evaluationPoints !== null ? solution.evaluationPoints : '-'}</td>
							<td>
								<button className="btn-filled-round" onClick={() => navigate(`/solution/${solution.teamId}`)}>
									Detail
								</button>
							</td>
						</tr>
					))}
					{displayedSolutions.length === 0 && (
						<tr>
							<td colSpan="4" style={{ textAlign: 'center' }}>No solutions found.</td>
						</tr>
					)}
				</tbody>
			</table>
		</div>
	);
}

export default SolutionList;
