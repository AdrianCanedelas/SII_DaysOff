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

/* Cambiar icono ordenacion */
/*function cambiarClase() {
    console.log('entraCambiarClase');
    var icono = document.getElementById("miIcono");
    icono.classList.toggle("bi bi-caret-up mx-1");
}*/

/*var myModal = new bootstrap.Modal(document.getElementById('calendarModal'));
myModal.show();*/

/* Calendario años */
document.addEventListener('DOMContentLoaded', function () {
    var yearPicker = document.getElementById('yearPicker');
    var currentYear = new Date().getFullYear();
    /*var decreaseYearBtn = document.getElementById('decreaseYear');
    var increaseYearBtn = document.getElementById('increaseYear');

    decreaseYearBtn.addEventListener('click', function () {
        if (yearPicker != null) {
            var currentYear = parseInt(yearPicker.value);
            yearPicker.value = currentYear - 1;
        }
    });

    increaseYearBtn.addEventListener('click', function () {
        if (yearPicker != null) {
            var currentYear = parseInt(yearPicker.value);
            yearPicker.value = currentYear + 1;
        }
    });*/

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
    /*var decreaseYearBtn = document.getElementById('decreaseYear');
    var increaseYearBtn = document.getElementById('increaseYear');

    decreaseYearBtn.addEventListener('click', function () {
        if (yearPicker != null) {
            var currentYear = parseInt(yearPicker.value);
            yearPicker.value = currentYear - 1;
        }
    });

    increaseYearBtn.addEventListener('click', function () {
        if (yearPicker != null) {
            var currentYear = parseInt(yearPicker.value);
            yearPicker.value = currentYear + 1;
        }
    });*/

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

/* Cambiar imagen ordenacion */
/*document.addEventListener('DOMContentLoaded', function () {
    var links = document.querySelectorAll('filter-link');

    links.forEach(function (link) {
        link.addEventListener('click', function (event) {
            console.log('aaaaaaaa')
            event.preventDefault();

            links.forEach(function (otherLink) {
                if (otherLink !== link) {
                    var otherIcon = otherLink.querySelector('.filter-icon');
                    otherIcon.classList.remove('bi-caret-down');
                    otherIcon.classList.add('bi-caret-up');
                }
            });

            var icon = link.querySelector('.filter-icon')
            if (icon.classList.contains('bi-caret-down')) {
                icon.classList.remove('bi-caret-down');
                icon.classList.add('bi-caret-up');
            } else {
                icon.classList.remove('bi-caret-up');
                icon.classList.add('bi-caret-down');
            }
        });
    })
});*/

//
const myDropDownI = bootstrap.Dropdown.getOrCreateInstance('#myDropdown')
myDropDownI.show()

const toastButton = document.querySelector('#toastCreateRequest');
const toastContent = document.querySelector('.toast');

if (toastButton) {
    toastButton.addEventListener('click', function () {
        console.log('si entra');
        const toast = new bootstrap.Toast(toastContent);
        toast.show();
    })
}