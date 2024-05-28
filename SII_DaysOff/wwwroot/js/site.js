/* Calendar */
document.addEventListener('DOMContentLoaded', function () {
	console.log('Entra JS')
	var calendarEl = document.getElementById('calendar');
	var calendar = new FullCalendar.Calendar(calendarEl, {
		height: 750,
		themeSystem: "bootstrap5",
		headerToolbar: {
			left: "prevYear,prev,next,nextYear today",
			center: "title",
			right: "dayGridMonth,timeGridWeek,timeGridDay,listMonth",
		},
		buttonIcons: true, 
		weekNumbers: false,
		navLinks: true, // can click day/week names to navigate views
		editable: true,
		dayMaxEvents: true, // allow "more" link when too many events
		firstDay: 1,
		fixedWeekCount: false,
		dayMaxEventRows: true,
		displayEventTime: true,
		displayEventEnd: true,
		editable: true,
		selectable: true,
		events: '/Requests/getDaysOff',
		/*events: [
			{
				title: 'test',
				start: '2024-05-27T13:00:00',
				constraint: 'test'
			}
		]*/
	});
	calendar.render();
});

/* Exportar calendario -- format(C2) element.classList.add('rotate'); */
console.log("entraaaa");
$("#printPDF").click(function () {
	console.log("entraaaa2");
	var element = document.getElementById('parentDiv');
	html2pdf().from(element).set({
		margin: [50, 50, 50, 50],
		enableLinks: true,
		jsPDF: { orientation: 'landscape', unit: 'pt', format: 'A2', compressPDF: true },
		filename: "Calendar.pdf",
	}).save();
});
document.styleSheets[0].insertRule("td { page-break-inside: avoid; }");
/* NavBar Requests */

