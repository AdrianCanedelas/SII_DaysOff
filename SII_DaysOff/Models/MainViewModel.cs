using SII_DaysOff.Areas.Identity.Data;
using SII_DaysOff.Data;

namespace SII_DaysOff.Models
{
	public class MainViewModel
	{
		public ApplicationUser? User { get; set; }
		public PaginatedList<Requests>? Requests { get; set; }
		public PaginatedList<Reasons>? Reasons { get; set; }
		public PaginatedList<Statuses>? Statuses { get; set; }
		public PaginatedList<VacationDays>? VacationDays { get; set; }
		public PaginatedList<UserVacationDays>? UserVacationDays { get; set; }
		public int TotalRequest {  get; set; }
		public int PageSize {  get; set; }
		public string Year {  get; set; }
		public Guid AdminId { get; set; }
	}
}
