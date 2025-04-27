import React, { useState, useMemo } from 'react';
import { useNavigate } from 'react-router-dom';
import { SearchBar, FilterSelect } from './FilterTools';
import { filterTeams } from '../utils/filterTeams';
import '../styles/table.css';
import '../styles/common.css';


function TeamList({ teams, individual = false, isInv = false }) {
	const navigate = useNavigate();
    const [searchTerm, setSearchTerm] = useState('');
    const [sortOption, setSortOption] = useState('name-asc');

    const sortOptions = [
		{ value: 'name-asc', label: 'Name A-Z' },
		{ value: 'name-desc', label: 'Name Z-A' },
	];

    // Teams filtering
    const displayedTeams = useMemo(() => {
        return filterTeams(teams, { searchTerm, sortOption });
    }, [teams, searchTerm, sortOption]);



	return (
		<div className="table-container">
			<h2>{individual ? 'Registered Students' : 'Registered Teams'}</h2>
			<div className="filter-tools">
                <SearchBar value={searchTerm} onChange={setSearchTerm} placeholder="Search team..." />
                <FilterSelect value={sortOption} onChange={setSortOption} options={sortOptions} />
			</div>
			<table className="styled-table">
				<thead>
					<tr>
						<th>{individual ? 'Student' : 'Team Name'}</th>
						{!individual && (
                            <>
                                <th>Leader username</th>
                                <th>Leader fullname</th>
                            </>
                        )}
						{isInv && (
							<th>Actions</th>
						)}
					</tr>
				</thead>
				<tbody>
					{displayedTeams.map((team) => (
						<tr key={team.id}>
							<td>{individual ? team.leader?.fullName : team.name}</td>
							{!individual && (
                                <>
                                    <td>{team.leader?.username}</td>
                                    <td>{team.leader?.fullName}</td>
                                </>
                            )}
							{isInv && (
								<td>
									<button className="btn-filled-round" onClick={() => navigate(`/team/${team.id}`)}>Detail</button>
								</td>
							)}
						</tr>
					))}
					{displayedTeams.length === 0 && (
						<tr>
							<td colSpan="4" style={{ textAlign: 'center' }}>No teams found.</td>
						</tr>
					)}
				</tbody>
			</table>
		</div>
	);
}

export default TeamList;