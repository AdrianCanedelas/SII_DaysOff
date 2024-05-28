using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
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

        public async Task<IActionResult> ManageIndex()
        {
			var user = await _userManager.GetUserAsync(User);

			var managerUserIds = _context.Users
	            .Where(u => u.Manager == user.Id)
	            .Select(u => u.Id)
	            .ToList();
            var requests = _context.Requests
                .Include(r => r.Reason)
                .ToList()
                .Where(r => r.StatusId == (_context.Statuses.FirstOrDefault(s => s.Name.Equals("Pending"))?.StatusId))
                .Where(r => managerUserIds.Contains(r.UserId));

			return View(requests);
        }

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
				.ToList()
				.Where(r => r.StatusId == (_context.Statuses.FirstOrDefault(s => s.Name.Equals("Approved"))?.StatusId))
                .Select(r => new
                {
                    title = r.User.Name + " " + r.User.Surname,
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

        public async Task<FileResult> ExportExcel()
        {
            var daysOff = _context.Requests
                .Include(r => r.User)
                .Include(r => r.Status)
                .Include(r => r.Reason)
                .ToList()
                .Where(r => r.StatusId == (_context.Statuses.FirstOrDefault(s => s.Name.Equals("Approved"))?.StatusId));
            var fileName = "calendar.xlsx";
            return GenerateExcel(fileName, daysOff);
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
            if (_context.Requests == null)
            {
                return Problem("Entity set 'DbContextBD.Requests'  is null.");
            }
            var requests = await _context.Requests.FindAsync(id);
            if (requests != null)
            {
                _context.Requests.Remove(requests);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestsExists(Guid id)
        {
          return (_context.Requests?.Any(e => e.RequestId == id)).GetValueOrDefault();
        }
    }
}
