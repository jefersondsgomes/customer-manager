namespace CustomerManager.Test.Mock
{
    public static class User
    {
        public static Model.Common.User Null;
        public static Model.Common.User Failed;
        public static Model.Common.User Invalid;
        public static Model.Common.User Exists;
        public static Model.Common.User Success;

        static User()
        {
            Null = null;

            Failed = new Model.Common.User()
            {
                Id = "507f191e810c19729de860ea",
                Name = "Failed",
                Login = "Failed",
                Password = "Failed"
            };

            Invalid = new Model.Common.User()
            {
                Id = "5099803df3f4948bd2f98391",
                Name = "Invalid",
                Login = "Invalid",
                Password = "Invalid"
            };

            Exists = new Model.Common.User()
            {
                Id = "5099223df433b948bd2e123153",
                Name = "Exists",
                Login = "Exists",
                Password = "Exists"
            };

            Success = new Model.Common.User()
            {
                Id = "507f1f77bcf86cd799439011",
                Name = "Success",
                Login = "Success",
                Password = "Success"
            };
        }
    }
}