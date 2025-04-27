

export function formatDate(dateString) {
	if (!dateString) return '';
	const date = new Date(dateString);

	return date.toLocaleString('en-GB', {
		timeZone: 'Europe/Berlin',
		day: '2-digit',
		month: 'short',
		year: 'numeric',
		hour: '2-digit',
		minute: '2-digit',
		hour12: false,
	});
}


export function formatRemainingTime(deadline) {
	const now = new Date();
	const deadlineDate = new Date(deadline);

	const diffMs = deadlineDate - now;
	if (diffMs <= 0) return 'Deadline passed';

	const diffDays = Math.floor(diffMs / (1000 * 60 * 60 * 24));
	const diffHours = Math.floor((diffMs / (1000 * 60 * 60)) % 24);
	const diffMinutes = Math.floor((diffMs / (1000 * 60)) % 60);

	if (diffDays > 0) {
		return `${diffDays} day${diffDays > 1 ? 's' : ''} left`;
	}
	if (diffHours > 0) {
		return `${diffHours} hour${diffHours > 1 ? 's' : ''} left`;
	}
	return `${diffMinutes} minute${diffMinutes > 1 ? 's' : ''} left`;
}