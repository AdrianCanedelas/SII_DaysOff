using SII_DaysOff.Areas.Identity.Data;
using SII_DaysOff.Data;

namespace SII_DaysOff.Models
{
	public class MainViewModel
	{
		public ApplicationUser User { get; set; }
		public PaginatedList<Requests> Requests { get; set; }
		public int TotalRequest {  get; set; }
		public int PageSize {  get; set; }
	}
}
