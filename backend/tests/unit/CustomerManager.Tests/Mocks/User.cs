namespace CustomerManager.Tests.Mocks
{
    public static class User
    {
        public static Models.Entities.User Null;
        public static Models.Entities.User Failed;
        public static Models.Entities.User Invalid;
        public static Models.Entities.User Exists;
        public static Models.Entities.User Success;

        static User()
        {
            Null = null;

            Failed = new Models.Entities.User()
            {
                Id = "507f191e810c19729de860ea",
                FirstName = "Failed",
                UserName = "Failed",
                Password = "Failed"
            };

            Invalid = new Models.Entities.User()
            {
                Id = "5099803df3f4948bd2f98391",
                FirstName = "Invalid",
                UserName = "Invalid",
                Password = "Invalid"
            };

            Exists = new Models.Entities.User()
            {
                Id = "5099223df433b948bd2e123153",
                FirstName = "Exists",
                UserName = "Exists",
                Password = "Exists"
            };

            Success = new Models.Entities.User()
            {
                Id = "507f1f77bcf86cd799439011",
                FirstName = "Success",
                UserName = "Success",
                Password = "Success"
            };
        }
    }
}