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
        navLinks: true, 
        editable: true,
        dayMaxEvents: true, 
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

$("#printPDF").click(function () {
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
    var yearPickerVacationDays = document.getElementById('yearPickerVacationDays');
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

    $(yearPickerVacationDays).datepicker({
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

//Cambiar año combo UserVacationDays
document.getElementById("selectUser").addEventListener("click", changeYears())
$(function changeYears() {
    $.ajax({
        type: "Get",
        url: "UserVacationsDays/yearSelectList?selectedUserId=" + $(this).val(),
        success: function (data) {
            $('#Year').empty(); // Limpiar opciones actuales
            $.each(data, function (index, item) {
                $('#Year').append($('<option>').text(item.text).attr('value', item.value));
            });
        },
        error: function (xhr, status, error) {
            console.error("Error al obtener los años disponibles:", error);
        }
    });
});

//Toasts
document.getElementById("toastbtn").onclick = function () {
    console.log('entra toast');
    var toastElList = [].slice.call(document.querySelectorAll('.toast'))
    var toastList = toastElList.map(function (toastEl) {
        return new bootstrap.Toast(toastEl)
    })
    toastList.forEach(toast => toast.show())
}

//