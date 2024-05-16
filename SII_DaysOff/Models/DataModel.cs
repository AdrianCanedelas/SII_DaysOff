using Microsoft.AspNetCore.Mvc;

namespace SII_DaysOff.Models
{
	public class DataModel : Controller
	{
		public string imgNavBar;
		public string imgLogin;

		public DataModel() 
		{
			imgNavBar = "~/img/siigroup-spain-whiteline.png";
			imgLogin = "~/img/siigroup-spain-blueline.png";
		}
	}
}
