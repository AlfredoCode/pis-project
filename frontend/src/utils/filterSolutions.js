export function filterSolutions(solutions, { searchTerm = '', sortOption = 'date-desc' } = {}) {
	let result = [...solutions];

	// Search
	if (searchTerm) {
		result = result.filter(sol =>
			sol.teamName?.toLowerCase().includes(searchTerm.toLowerCase())
		);
	}

	// Sort
	result.sort((a, b) => {
		switch (sortOption) {
			case 'date-desc':
				return new Date(b.submissionDate) - new Date(a.submissionDate);
			case 'date-asc':
				return new Date(a.submissionDate) - new Date(b.submissionDate);
			case 'teamname-asc':
				return a.teamName.localeCompare(b.teamName);
			case 'teamname-desc':
				return b.teamName.localeCompare(a.teamName);
			case 'points-desc':
				return (b.evaluationPoints ?? -1) - (a.evaluationPoints ?? -1);
			case 'points-asc':
				return (a.evaluationPoints ?? Infinity) - (b.evaluationPoints ?? Infinity);
			default:
				return 0;
		}
	});

	return result;
}
