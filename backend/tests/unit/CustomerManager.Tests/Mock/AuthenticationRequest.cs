namespace CustomerManager.Test.Mock
{
    public static class AuthenticationRequest
    {
        public static Model.Transient.AuthenticateRequest Null;
        public static Model.Transient.AuthenticateRequest NullUserName;
        public static Model.Transient.AuthenticateRequest NullPassword;
        public static Model.Transient.AuthenticateRequest Error;
        public static Model.Transient.AuthenticateRequest Ok;

        static AuthenticationRequest()
        {
            Null = null;
            NullUserName = new Model.Transient.AuthenticateRequest() { Username = null, Password = "password" };
            NullPassword = new Model.Transient.AuthenticateRequest() { Username = "username", Password = null };
            Error = new Model.Transient.AuthenticateRequest() { Username = "error", Password = "error" };
            Ok = new Model.Transient.AuthenticateRequest() { Username = "ok", Password = "ok" };
        }
    }
}
