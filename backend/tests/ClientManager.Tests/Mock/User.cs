using MongoDB.Bson;

namespace ClientManager.Test.Mock
{
    public static class User
    {
        public static Model.Common.User Null;
        public static Model.Common.User Failed;
        public static Model.Common.User Invalid;
        public static Model.Common.User Success;

        static User()
        {
            Null = null;

            Failed = new Model.Common.User()
            {
                Id = ObjectId.Parse("507f191e810c19729de860ea"),
                Login = "Failed",
                Password = "Failed"
            };

            Invalid = new Model.Common.User()
            {
                Id = ObjectId.Parse("5099803df3f4948bd2f98391"),
                Login = "Invalid",
                Password = "Invalid"
            };

            Success = new Model.Common.User()
            {
                Id = ObjectId.Parse("507f1f77bcf86cd799439011"),
                Login = "Success",
                Password = "Success"
            };
        }
    }
}
