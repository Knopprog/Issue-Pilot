// source: https://fullcalendar.io/
document.addEventListener('DOMContentLoaded', function () {
	var calendarEl = document.getElementById('calendar');

	// Create list of events
	var listOfEvents = [];
	for (let i = 0; i < listOfTicketCreateEvents.length; i++) {
		var dateString = formatDate(listOfTicketCreateEvents[i].Date);

		// Add event to list
		listOfEvents.push({
			title: listOfTicketCreateEvents[i].TicketsCount + " Ticket(s) hinzugefügt",
			start: dateString,
			backgroundColor: '#1bb28a',
			borderColor: '#1bb28a'
		})
	}
	for (let i = 0; i < listOfTicketClosedEvents.length; i++) {

		var dateString = formatDate(listOfTicketClosedEvents[i].Date);

		// Add event to list
		listOfEvents.push({
			title: listOfTicketClosedEvents[i].TicketsCount + " Ticket(s) geschlossen",
			start: dateString,
			backgroundColor: '#464c50',
			borderColor: '#464c50'
		})
	}
	for (let i = 0; i < listOfDeadlineEvents.length; i++) {

		var dateString = formatDate(listOfDeadlineEvents[i].Date);

		// Add event to list
		listOfEvents.push({
			title: listOfDeadlineEvents[i].TicketsCount + " Deadline(s) fällig",
			start: dateString,
			backgroundColor: '#C14640',
			borderColor: '#C14640'
		})
	}


	// Add options to calendar
	var calendar = new FullCalendar.Calendar(calendarEl, {
		initialView: 'dayGridMonth',
		initialDate: Date.now(),
		headerToolbar: {
			left: 'dayGridMonth,dayGridWeek,listYear today',
			center: 'title',
			right: 'prevYear,prev,next,nextYear'
		},
		weekNumbers: true,
		navLinks: true,
		allDaySlot: false,
		dayMaxEvents: true,
		locale: 'de',
		eventDidMount: function (info) {
			$(info.el).tooltip({
				title: info.event.title
			});
		},
		events: listOfEvents
	});

	calendar.render();
});

function formatDate(oldFormatDate) {
	var newDate = new Date(oldFormatDate);
	var date = newDate.getDate();
	var month = newDate.getMonth();
	var year = newDate.getFullYear();
	month = month + 1;

	// Format date for fullcalendar
	if (month < 10) {
		month = "0" + month;
	}
	if (date < 10) {
		date = "0" + date;
	}
	return dateString = year + "-" + month + "-" + date;
}