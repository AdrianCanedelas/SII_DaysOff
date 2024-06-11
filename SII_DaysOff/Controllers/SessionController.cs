using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SII_DaysOff.Data;

namespace SII_DaysOff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> GetSessionInfo()
        {
            List<string> sessionInfo = new List<string>();

            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString(SessionVariables.SessionKeyUserName)))
            {
                HttpContext.Session.SetString(SessionKeyEnum.SessionKeyUserName.ToString(), "Current User");
                HttpContext.Session.SetString(SessionKeyEnum.SessionKeySessionId.ToString(), Guid.NewGuid().ToString());
            }

            var username = HttpContext.Session.GetString(SessionVariables.SessionKeyUserName);
            var sessionId = HttpContext.Session.GetString(SessionVariables.SessionKeySessionId);

            sessionInfo.Add(username);
            sessionInfo.Add(sessionId);

            return sessionInfo;
        }
    }
}
