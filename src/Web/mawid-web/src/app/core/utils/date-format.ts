export function formatTime(iso: string, timeZone: string) {
  return new Intl.DateTimeFormat('en-US', { hour: 'numeric', minute: '2-digit', timeZone }).format(new Date(iso));
}

export function formatDate(iso: string, timeZone: string) {
  return new Intl.DateTimeFormat('en-GB', { weekday: 'short', day: 'numeric', month: 'short', timeZone }).format(new Date(iso));
}