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
    });
    calendar.render();
});

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

document.addEventListener('DOMContentLoaded', function () {
    var yearPicker = document.getElementById('yearPicker');
    var currentYear = new Date().getFullYear();

    $(yearPicker).datepicker({
        format: "yyyy",
        viewMode: "years",
        minViewMode: "years",
        startDate: "2000",
        endDate: "2030",
        defaultViewDate: { year: currentYear },
        autoclose: true
    });
});

document.addEventListener('DOMContentLoaded', function () {
    var yearPicker = document.getElementById('yearPickerVacationDays');
    var currentYear = new Date().getFullYear();

    $(yearPicker).datepicker({
        format: "yyyy",
        viewMode: "years",
        minViewMode: "years",
        startDate: "2000",
        endDate: "2030",
        defaultViewDate: { year: currentYear },
        autoclose: true
    });
});

/* Calendario meses */
document.addEventListener('DOMContentLoaded', function () {
    var yearPicker = document.getElementById('monthPicker');

    $(yearPicker).datepicker({
        format: "mm",
        viewMode: "months",
        minViewMode: "months",
        autoclose: true
    });
});

//Dropdows
const myDropDownI = bootstrap.Dropdown.getOrCreateInstance('#myDropdown')
myDropDownI.show()

//Toasts
const toastButton = document.querySelector('#toastCreateRequest');
const toastContent = document.querySelector('.toast');

if (toastButton) {
    toastButton.addEventListener('click', function () {
        console.log('si entra');
        const toast = new bootstrap.Toast(toastContent);
        toast.show();
    })
}

//
/*
document.addEventListener('DOMContentLoaded', (event) => {
			console.log('Script is running');
			let yearPicker = document.getElementById('yearPickerMain');
			var currentYear = new Date().getFullYear();

			$(yearPicker).datepicker({
				format: "yyyy",
				viewMode: "years",
				minViewMode: "years",
				startDate: "2000",
				endDate: "2030",
				defaultViewDate: { year: currentYear },
				autoclose: true
			});
			yearPicker.addEventListener('blur', function () {
				console.log('Blur event triggered');
				document.getElementById('autoSubmitForm').submit();
			});
			yearPicker.addEventListener('input', function () {
				console.log('Input event triggered');
			});
			yearPicker.addEventListener('click', function () {
				console.log('Click event triggered');
			});
		});
*/