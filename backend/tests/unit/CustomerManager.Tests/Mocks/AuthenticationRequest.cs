namespace CustomerManager.Tests.Mocks
{
    public static class AuthenticationRequest
    {
        public static Models.Transients.AuthenticateRequest Null;
        public static Models.Transients.AuthenticateRequest NullUserName;
        public static Models.Transients.AuthenticateRequest NullPassword;
        public static Models.Transients.AuthenticateRequest Error;
        public static Models.Transients.AuthenticateRequest Ok;

        static AuthenticationRequest()
        {
            Null = null;
            NullUserName = new Models.Transients.AuthenticateRequest() { Username = null, Password = "password" };
            NullPassword = new Models.Transients.AuthenticateRequest() { Username = "username", Password = null };
            Error = new Models.Transients.AuthenticateRequest() { Username = "error", Password = "error" };
            Ok = new Models.Transients.AuthenticateRequest() { Username = "ok", Password = "ok" };
        }
    }
}
