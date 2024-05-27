/* Calendar */

document.addEventListener('DOMContentLoaded', function () {
	console.log('Entra JS')
	var calendarEl = document.getElementById('calendar');
	var calendar = new FullCalendar.Calendar(calendarEl, {
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

/* Exportar calendario */
/*$("#btnPDF").click(function () {
	var sHtml = $("pdfContainer").html();
	sHtml = sHtml.replace(/</g, "StrTag").replace(/>/g, "EndTag");
	window.open('../Requests/GeneratePDF?html=' + sHtml, '_blank');
});*/

/* NavBar Requests */