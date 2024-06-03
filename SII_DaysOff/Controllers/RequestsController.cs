using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using SelectPdf;
using SII_DaysOff.Areas.Identity.Data;
using SII_DaysOff.Data;
using SII_DaysOff.Models;

namespace SII_DaysOff.Controllers
{
    public class RequestsController : Controller
    {
        private readonly DbContextBD _context;
        private readonly IServiceProvider _serviceProvider;
		private readonly IRazorViewEngine _razorViewEngine;
		private readonly ITempDataProvider _tempDataProvider;
		private UserManager<ApplicationUser> _userManager;

        public RequestsController(DbContextBD context, UserManager<ApplicationUser> userManager, IServiceProvider serviceProvider, 
            IRazorViewEngine razorViewEngine, ITempDataProvider tempDataProvider)
        {
            _context = context;
            _userManager = userManager;
            _serviceProvider = serviceProvider;
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
        }

        // GET: Requests
        public async Task<IActionResult> Index()
        {
            var dbContextBD = _context.Requests.Include(r => r.CreatedByNavigation).Include(r => r.ModifiedByNavigation).Include(r => r.Reason).Include(r => r.Status).Include(r => r.User);
            return View(await dbContextBD.ToListAsync());
        }
        
        public IActionResult Calendar()
        {
            return View();
        }

        public async Task<IActionResult> ManageIndex(string sortOrder, string searchString, int? numPage, string currentFilter)
        {
			//Ordenación
			ViewData["ReasonOrder"] = String.IsNullOrEmpty(sortOrder) ? "Reason_desc" : "";
			ViewData["NameOrder"] = sortOrder == "Name" ? "Name_desc" : "Name";
			ViewData["StartDayOrder"] = sortOrder == "StartDay" ? "StartDay_desc" : "StartDay";
			ViewData["HalfDayStartOrder"] = sortOrder == "HalfDayStart" ? "HalfDayStart_desc" : "HalfDayStart";
			ViewData["EndDayOrder"] = sortOrder == "EndDay" ? "EndDay_desc" : "EndDay";
			ViewData["HalfDayEndOrder"] = sortOrder == "HalfDayEnd" ? "HalfDayEnd_desc" : "HalfDayEnd";
			ViewData["RequestDayOrder"] = sortOrder == "RequestDay" ? "RequestDay_desc" : "RequestDay";
			ViewData["CommentsOrder"] = sortOrder == "Comments" ? "Comments_desc" : "Comments";

			//Cuadro de búsqueda
			ViewData["CurrentFilter"] = searchString;

			var user = await _userManager.GetUserAsync(User);

			var statusId = _context.Statuses.FirstOrDefault(s => s.Name.Equals("Pending"))?.StatusId;
			//var userId = _context.AspNetUsers.FirstOrDefault(u => u.Name.Equals(user.Name))?.Id;

			if (statusId == null)
			{
				return View(new List<Requests>());
			}

			var managerUserIds = _context.Users
	            .Where(u => u.Manager == user.Id)
	            .Select(u => u.Id)
	            .ToList();
            var requests = _context.Requests
                .Include(r => r.Reason)
                .Include(r => r.User)
                .Where(r => r.StatusId == statusId)
                .Where(r => managerUserIds.Contains(r.UserId))
                .AsQueryable();

			//Paginacion
			if (searchString != null) numPage = 1;
			else searchString = currentFilter;

			if (!String.IsNullOrEmpty(searchString))
			{
				requests = requests.Where(r => r.Reason.Name.Contains(searchString) 
                || r.StartDate.ToString().Contains(searchString) 
				|| r.EndDate.ToString().Contains(searchString)
				|| r.RequestDate.ToString().Contains(searchString)
				|| r.Comments.Contains(searchString));
			}

			ViewData["CurrentOrder"] = sortOrder;
			ViewData["CurrentFilter"] = searchString;

			switch (sortOrder)
			{
				case "Reason_desc":
					requests = requests.OrderByDescending(r => r.Reason.Name);
					break;
                case "Name_desc":
					requests = requests.OrderByDescending(r => r.User.Name);
					break;
				case "StartDay_desc":
					requests = requests.OrderByDescending(r => r.StartDate);
					break;
				case "StartDay":
					requests = requests.OrderBy(r => r.StartDate);
					break;
				case "HalfDayStart":
					requests = requests.OrderBy(r => r.HalfDayStart);
					break;
				case "HalfDayStart_desc":
					requests = requests.OrderByDescending(r => r.HalfDayStart);
					break;
				case "EndDay":
					requests = requests.OrderBy(r => r.EndDate);
					break;
				case "EndDay_desc":
					requests = requests.OrderByDescending(r => r.EndDate);
					break;
				case "HalfDayEnd":
					requests = requests.OrderBy(r => r.HalfDayEnd);
					break;
				case "HalfDayEnd_desc":
					requests = requests.OrderByDescending(r => r.HalfDayEnd);
					break;
				case "RequestDay":
					requests = requests.OrderBy(r => r.RequestDate);
					break;
				case "RequestDay_desc":
					requests = requests.OrderByDescending(r => r.RequestDate);
					break;
				case "Comments":
					requests = requests.OrderBy(r => r.Comments);
					break;
				case "Comments_desc":
					requests = requests.OrderByDescending(r => r.Comments);
					break;
				case "Status_desc":
					requests = requests.OrderByDescending(r => r.Status.Name);
					break;
			}

			int registerCount = 8;

			return View(await PaginatedList<Requests>.CreateAsync(requests.AsNoTracking(), numPage ?? 1, registerCount));
		}

		/*
         public async Task<IActionResult> ManageIndex(string sortOrder, string searchString, int? numPage, string currentFilter)
        {
			//Ordenación
			ViewData["ReasonOrder"] = String.IsNullOrEmpty(sortOrder) ? "Reason_desc" : "";
			ViewData["StartDayOrder"] = sortOrder == "StartDay" ? "StartDay_desc" : "StartDay";
			ViewData["HalfDayStartOrder"] = sortOrder == "HalfDayStart" ? "HalfDayStart_desc" : "HalfDayStart";
			ViewData["EndDayOrder"] = sortOrder == "EndDay" ? "EndDay_desc" : "EndDay";
			ViewData["HalfDayEndOrder"] = sortOrder == "HalfDayEnd" ? "HalfDayEnd_desc" : "HalfDayEnd";
			ViewData["RequestDayOrder"] = sortOrder == "RequestDay" ? "RequestDay_desc" : "RequestDay";
			ViewData["CommentsOrder"] = sortOrder == "Comments" ? "Comments_desc" : "Comments";

			//Cuadro de búsqueda
			ViewData["CurrentFilter"] = searchString;

			var user = await _userManager.GetUserAsync(User);
			var userId = _context.AspNetUsers.FirstOrDefault(u => u.Name.Equals(user.Name))?.Id;

			var managerUserIds = _context.Users
	            .Where(u => u.Manager == user.Id)
	            .Select(u => u.Id)
	            .ToList();
            var requests = _context.Requests
                .Include(r => r.Reason)
                .Where(r => r.StatusId == userId)
                .Where(r => managerUserIds.Contains(r.UserId)).AsQueryable();

			if (searchString != null) numPage = 1;
			else searchString = currentFilter;

			if (!String.IsNullOrEmpty(searchString))
			{
				requests = requests.Where(r => r.Reason.Name.Contains(searchString) 
                || r.StartDate.ToString().Contains(searchString) 
				|| r.EndDate.ToString().Contains(searchString)
				|| r.RequestDate.ToString().Contains(searchString)
				|| r.Comments.Contains(searchString));
			}

			ViewData["CurrentOrder"] = sortOrder;
			ViewData["CurrentFilter"] = currentFilter;

			switch (sortOrder)
			{
				case "Rreason_desc":
					requests = requests.OrderByDescending(r => r.Reason.Name);
					break;
				case "StartDay_desc":
					requests = requests.OrderByDescending(r => r.StartDate);
					break;
				case "StartDay":
					requests = requests.OrderBy(r => r.StartDate);
					break;
				case "HalfDayStart":
					requests = requests.OrderBy(r => r.HalfDayStart);
					break;
				case "HalfDayStart_desc":
					requests = requests.OrderByDescending(r => r.HalfDayStart);
					break;
				case "EndDay":
					requests = requests.OrderBy(r => r.EndDate);
					break;
				case "EndDay_desc":
					requests = requests.OrderByDescending(r => r.EndDate);
					break;
				case "HalfDayEnd":
					requests = requests.OrderBy(r => r.HalfDayEnd);
					break;
				case "HalfDayEnd_desc":
					requests = requests.OrderByDescending(r => r.HalfDayEnd);
					break;
				case "RequestDay":
					requests = requests.OrderBy(r => r.RequestDate);
					break;
				case "RequestDay_desc":
					requests = requests.OrderByDescending(r => r.RequestDate);
					break;
				case "Comments":
					requests = requests.OrderBy(r => r.Comments);
					break;
				case "Comments_desc":
					requests = requests.OrderByDescending(r => r.Comments);
					break;
				case "Status_desc":
					requests = requests.OrderByDescending(r => r.Status.Name);
					break;
			}

			int registerCount = 5;

			return View(await PaginatedList<Requests>.CreateAsync(requests.AsNoTracking(), numPage?? 1, registerCount));
        }
         */

		// GET: Requests/Details/5
		public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var requests = await _context.Requests
                .Include(r => r.CreatedByNavigation)
                .Include(r => r.ModifiedByNavigation)
                .Include(r => r.Reason)
                .Include(r => r.Status)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (requests == null)
            {
                return NotFound();
            }

            return View(requests);
        }

        [HttpGet]
        public async Task<JsonResult> GetUsersReasonsAndStatuses()
        {
            var users = await _context.AspNetUsers.Select(u => new { Id = u.Id, UserName = u.UserName }).ToListAsync();
            var reasons = await _context.Reasons.Select(r => new { Id = r.ReasonId, Name = r.Name }).ToListAsync();
            var statuses = await _context.Statuses.Select(s => new { Id = s.StatusId, Name = s.Name }).ToListAsync();

            return Json(new { users = users, reasons = reasons, statuses = statuses });
        }

        // GET: Requests/Create
        public IActionResult Create()
        {
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            ViewData["ReasonId"] = new SelectList(_context.Reasons, "ReasonId", "ReasonId");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId");
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return PartialView("ModalCreateRequest");
        }

        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestId,UserId,ReasonId,StatusId,RequestDate,StartDate,EndDate,HalfDayStart,HalfDayEnd,Comments,CreatedBy,CreationDate,ModifiedBy,ModificationDate")] Requests requests)
        {
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    var fieldName = entry.Key;
                    var fieldValue = entry.Value;

                    foreach (var error in fieldValue.Errors)
                    {
                        var errorMessage = error.ErrorMessage;
                        Console.WriteLine($"Error en el campo: {fieldName}. Mensaje: {errorMessage}");
                    }
                }
            }
			if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                requests.RequestId = Guid.NewGuid();
                requests.UserId = user.Id;
                requests.StatusId = _context.Statuses
                    .Where(s => s.Name.Equals("Pending")).FirstOrDefault().StatusId;
                requests.CreatedBy = user.Id;
                requests.ModifiedBy = user.Id;
                requests.CreationDate = DateTime.Now;
                requests.ModificationDate = DateTime.Now;
                requests.RequestDate = DateTime.Now;
                        
                _context.Add(requests);
                await _context.SaveChangesAsync();
                return LocalRedirect("~/Home/Main");
            }
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.CreatedBy);
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.ModifiedBy);
            ViewData["ReasonId"] = new SelectList(_context.Reasons, "ReasonId", "ReasonId", requests.ReasonId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", requests.StatusId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.UserId);
            var allRequests = await _context.Requests.ToListAsync();

            return View("~/Views/Home/Main.cshtml", allRequests);
        }

        // GET: Requests/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            Console.WriteLine("editGET");
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var requests = await _context.Requests.FindAsync(id);
            if (requests == null)
            {
                return NotFound();
            }
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.CreatedBy);
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.ModifiedBy);
            ViewData["ReasonId"] = new SelectList(_context.Reasons, "Name", "Name");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", requests.StatusId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.UserId);
            return View(requests);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("RequestId,UserId,ReasonId,StatusId,RequestDate,StartDate,EndDate,HalfDayStart,HalfDayEnd,Comments,CreatedBy,CreationDate,ModifiedBy,ModificationDate")] Requests requests)
        {
            Console.WriteLine("editPOST");
            if (id != requests.RequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);

                    requests.RequestDate = DateTime.Now;
                    requests.ModificationDate = DateTime.Now;
                    requests.ModifiedBy = user.Id;
                    requests.ModificationDate = DateTime.Now;
                    _context.Update(requests);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestsExists(requests.RequestId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.CreatedBy);
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.ModifiedBy);
            ViewData["ReasonId"] = new SelectList(_context.Reasons, "ReasonId", "ReasonId", requests.ReasonId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", requests.StatusId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.UserId);
            return View(requests);
        }

        // GET: Requests/Edit/5
        public async Task<IActionResult> Manage(int? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var requests = await _context.Requests.FindAsync(id);
            if (requests == null)
            {
                return NotFound();
            }
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.CreatedBy);
            ViewData["ModifiedBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.ModifiedBy);
            ViewData["ReasonId"] = new SelectList(_context.Reasons, "ReasonId", "ReasonId", requests.ReasonId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", requests.StatusId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", requests.UserId);
            return View(requests);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(Guid id, string option)
        {
			var request = await _context.Requests.FindAsync(id);
			if (request == null)
			{
				return NotFound();
			}

			if (option == "Approved")
			{
				request.StatusId = Guid.Parse("98D18765-5BF7-41BD-8727-A39F7F95B9AC"); 
			}
			else if (option == "Cancelled")
			{
				request.StatusId = Guid.Parse("67AD8346-6F99-4465-9F33-4E0AC387D5D1");
			}

			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(ManageIndex));
		}

        public JsonResult getDaysOff()
		{
			Console.WriteLine("Entra");
			var daysOff = _context.Requests
                .Include(r => r.User)
                .Include(r => r.Reason)
				.ToList()
				.Where(r => r.StatusId == (_context.Statuses.FirstOrDefault(s => s.Name.Equals("Approved"))?.StatusId))
                .Select(r => new
                {
                    title = r.User.Name + " " + r.User.Surname + " / " + r.Reason.Name,
                    start = r.StartDate.ToString("yyyy-MM-dd"),
                    end = r.EndDate.ToString("yyyy-MM-dd"),
                    description = r.Comments,
                });

            return new JsonResult(daysOff);
        }

        public IActionResult GeneratePDF(string html)
        {
            html = html.Replace("StrTag", "<").Replace("EndTag", ">");

            HtmlToPdf oHtmlToPdf = new HtmlToPdf();
            PdfDocument oPdfDocument = oHtmlToPdf.ConvertHtmlString(html);
            byte[] pdf = oPdfDocument.Save();
            oPdfDocument.Close();

            return File(
                pdf,
                "application/pdf",
                "Calendar.pdf"
                );
        }

		public async Task<FileResult> ExportExcel(int year, int month, string type)
		{
            Console.WriteLine("year/month -> " + year + " - " + month);
			var daysOff = _context.Requests
				.Include(r => r.User)
				.Include(r => r.Status)
				.Include(r => r.Reason)
				.ToList()
				.Where(r => r.StatusId == (_context.Statuses.FirstOrDefault(s => s.Name.Equals("Approved"))?.StatusId));

			var fileName = type + ".xlsx";

            if(type.Equals("requests")) return GenerateExcel(fileName, daysOff);
            return GenerateExcel(fileName, daysOff, year, month);
		}

		private FileResult GenerateExcel(string fileName, IEnumerable<Requests> requests, int year, int month)
		{
			using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Calendario");
                worksheet.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#f2f2f2"));

                DateTime firstDayOfMonth = new DateTime(year, month, 1);
                int daysMonth = DateTime.DaysInMonth(year, month);
                int row = 5;
                int column = ((int)firstDayOfMonth.DayOfWeek + 1) % 7;

                //Estilos celdas y celda titulo
                cellStyles(worksheet);
                titleCell(worksheet, month, year);
                daysCells(worksheet);
                bgCellStyles(worksheet, column, row);

                for (int day = 1; day <= daysMonth; day++)
                {
                    numberCellStyles(worksheet, column, row, day);

                    if (requests.Any(r => r.StartDate <= new DateTime(year, month, day) && r.EndDate >= new DateTime(year, month, day)))
                    {
                        //worksheet.Cell(row, column).Style.Fill.SetBackgroundColor(XLColor.BluePigment).Font.SetFontColor(XLColor.White);

                        row = bottomCells(worksheet, row, column, XLColor.FromHtml("#f2f2f2"));

                        int pastRow = row;
						foreach (Requests r in requests)
                        {
                            if(r.StartDate <= new DateTime(year, month, day) && r.EndDate >= new DateTime(year, month, day))
                            {
                                row++;
                                if (row>pastRow) worksheet.Cell(row, column).Value += ("     ");
								worksheet.Cell(row, column).Value += ("             |" + r.User.Name + " " + r.User.Surname + "   |" + r.Reason.Name);
                                worksheet.Cell(row, column).Style.Fill.SetBackgroundColor(XLColor.FromHtml("#007eb5")).Font.SetFontColor(XLColor.FromHtml("#f2f2f2"))
                                    .Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetBottomBorderColor(XLColor.CoolGrey)
                                    .Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetTopBorderColor(XLColor.CoolGrey);
                            }
                        }
                        row = pastRow;
                    } 
                    else
                    {
						row = bottomCells(worksheet, row, column, XLColor.FromHtml("#f2f2f2"));
					}

                    if(column == 8)
                    {
                        row += 5;
						column = 2;
                    } 
                    else {
						column++;
					}
                }
				using (MemoryStream stream = new MemoryStream())
				{
					workbook.SaveAs(stream);
					return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
				}
			}
		}

        public void cellStyles(IXLWorksheet worksheet)
        {
            for (int i = 2; i <= 29; i++)
            {
                for (int j = 2; j <= 8; j++)
                {
                    worksheet.Cell(i, j).Style.Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetRightBorderColor(XLColor.CoolGrey);
                    worksheet.Cell(i, j).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetLeftBorderColor(XLColor.CoolGrey);
                    worksheet.Column(j).Width = 35;
                    worksheet.Cell(4, j).Style.Fill.SetBackgroundColor(XLColor.FromHtml("#004278")).Font.SetFontColor(XLColor.White).Font.Bold = true;
                    worksheet.Cell(4, j).Style.Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetTopBorderColor(XLColor.CoolGrey);
                    worksheet.Cell(4, j).Style.Font.SetFontSize(16);
                    worksheet.Cell(4, j).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(4, j).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    if (i == 2) worksheet.Cell(i, j).Style.Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetTopBorderColor(XLColor.CoolGrey);
                    if (i == 29) worksheet.Cell(i, j).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetBottomBorderColor(XLColor.CoolGrey);
                }
                if (i > 4) worksheet.Row(i).Height = 25;
                else worksheet.Row(i).Height = 20;
            }
        }

        public void bgCellStyles(IXLWorksheet worksheet, int column, int row)
        {
            int cont = 0;
            for (int i = (cont < (6 - column) ? 2 : column); i < (cont < (6 - column) ? column : 9); i++)
            {
                for (int j = (cont < (6 - column) ? row : 25); j < (cont < (6 - column) ? (row + 6) : 30); j++)
                {
                    worksheet.Cell(j, i).Style.Fill.SetBackgroundColor(XLColor.FromHtml("#f2f2f2"));
                }
                cont++;
            }
        }

        public void numberCellStyles(IXLWorksheet worksheet, int column, int row, int day)
        {
            worksheet.Cell(row, column).Value = day;
            worksheet.Cell(row, column).Style.Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetTopBorderColor(XLColor.CoolGrey)
                .Font.SetFontColor(XLColor.FromHtml("#007eb5"));
            worksheet.Cell(row, column).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            worksheet.Cell(row, column).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        }

        public void titleCell(IXLWorksheet worksheet, int month, int year)
        {
            var mergedCell = worksheet.Range("B2:H3").Merge();

            mergedCell.Value = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month).ToUpper() + " " + year;
            mergedCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            mergedCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            mergedCell.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#004278"));
            mergedCell.Style.Font.Bold = true;
            mergedCell.Style.Font.SetFontSize(22);
            mergedCell.Style.Font.SetFontColor(XLColor.White);
        }

        public void daysCells(IXLWorksheet worksheet)
        {
            worksheet.Cell(4, 2).Value = "Mon";
            worksheet.Cell(4, 3).Value = "Tue";
            worksheet.Cell(4, 4).Value = "Wed";
            worksheet.Cell(4, 5).Value = "Thu";
            worksheet.Cell(4, 6).Value = "Fri";
            worksheet.Cell(4, 7).Value = "Sat";
            worksheet.Cell(4, 8).Value = "Sun";
        }

        public int bottomCells(IXLWorksheet worksheet, int row, int column, XLColor color)
        {
            for (int i = 0; i < 4; i++)
            {
                row++;
                worksheet.Cell(row, column).Style.Fill.SetBackgroundColor(color);
            }
            worksheet.Cell(row, column).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetBottomBorderColor(XLColor.CoolGrey);
            return row -= 4;
        }

        private FileResult GenerateExcel(string fileName, IEnumerable<Requests> requests)
        {
            DataTable dataTable = new DataTable("DaysOff");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Id"),
                new DataColumn("User"),
                new DataColumn("Reason"),
                new DataColumn("StartDate"),
                new DataColumn("HalfDayStart"),
                new DataColumn("EndDate"),
                new DataColumn("HalfDayEnd"),
                new DataColumn("RequestDay"),
                new DataColumn("Comments"),
                new DataColumn("Status"),
            });

            foreach (var request in requests)
            {
                dataTable.Rows.Add(request.RequestId, request.User.Email, request.Reason.Name, request.StartDate, request.HalfDayStart.Equals(true) ? "Yes":"No", request.EndDate, request.HalfDayEnd.Equals(true) ? "Yes" : "No", request.RequestDate, request.Comments, request.Status.Name);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

		// GET: Requests/Delete/5
		public async Task<IActionResult> Delete(Guid? id)
        {
            Console.WriteLine("getttttt");
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var requests = await _context.Requests
                .Include(r => r.CreatedByNavigation)
                .Include(r => r.ModifiedByNavigation)
                .Include(r => r.Reason)
                .Include(r => r.Status)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (requests == null)
            {
                return NotFound();
            }

            return View(requests);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            Console.WriteLine("posttttttt - GUID - " + id);
            if (_context.Requests == null)
            {
                return Problem("Entity set 'DbContextBD.Requests'  is null.");
            }
            var requests = await _context.Requests.FindAsync(id);
            if (requests != null)
            {
                Console.WriteLine("|||entraRemove");
                _context.Requests.Remove(requests);
            }
            
            await _context.SaveChangesAsync();

			return LocalRedirect("~/Home/Main");
		}

        private bool RequestsExists(Guid id)
        {
          return (_context.Requests?.Any(e => e.RequestId == id)).GetValueOrDefault();
        }
    }
}
