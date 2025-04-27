

export function filterProjects(projects, { searchTerm = '', filterKey = '', sortOption = 'name-asc' } = {}) {
	const now = new Date();
	let result = [...projects];

	// Search
	if (searchTerm) {
		result = result.filter(p =>
			p.name.toLowerCase().includes(searchTerm.toLowerCase())
		);
	}

	// Filter
	if (filterKey) {
		switch (filterKey) {
            case 'submitted':
                result = result.filter(p => p.submitted);
                break;
            case 'not-submitted':
                result = result.filter(p => !p.submitted);
                break;
            case 'evaluated':
                result = result.filter(p => p.points !== null);
                break;
            case 'not-evaluated':
                result = result.filter(p => p.points === null);
                break;
            case 'before-deadline':
                result = result.filter(p => new Date(p.deadline) >= now);
                break;
            case 'after-deadline':
                result = result.filter(p => new Date(p.deadline) < now);
                break;
            default:
                break;
        }
	}

	// Sort
	result.sort((a, b) => {
		switch (sortOption) {
			case 'name-asc': return a.name.localeCompare(b.name);
			case 'name-desc': return b.name.localeCompare(a.name);
			case 'deadline-asc': return new Date(a.deadline) - new Date(b.deadline);
			case 'deadline-desc': return new Date(b.deadline) - new Date(a.deadline);
			default: return 0;
		}
	});

	return result;
}