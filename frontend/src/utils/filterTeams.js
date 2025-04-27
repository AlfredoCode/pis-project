

export function filterTeams(teams, { searchTerm = '', sortOption = 'name-asc' } = {}) {
	const now = new Date();
	let result = [...teams];

	// Search
	if (searchTerm) {
		result = result.filter(t =>
			t.name.toLowerCase().includes(searchTerm.toLowerCase())
		);
	}

	// Sort
	result.sort((a, b) => {
		switch (sortOption) {
			case 'name-asc': return a.name.localeCompare(b.name);
			case 'name-desc': return b.name.localeCompare(a.name);
			default: return 0;
		}
	});

	return result;
}