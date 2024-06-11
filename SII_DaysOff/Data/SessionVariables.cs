namespace SII_DaysOff.Data
{
    public class SessionVariables
    {
        public const string SessionKeyUserName = "SessionKeyUsername";
        public const string SessionKeySessionId = "SessionKeySessionId";
        public const string SessionKeyYear = "SessionKeyYear";
    }

    public enum SessionKeyEnum
    {
        SessionKeyUserName = 0,
        SessionKeySessionId = 1,
        SessionKeyYear = 2
    }
}
