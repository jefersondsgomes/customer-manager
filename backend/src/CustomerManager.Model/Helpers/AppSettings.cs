using CustomerManager.Models.Helpers.Interfaces;

namespace CustomerManager.Models.Helpers
{
    public class AppSettings : IAppSettings
    {
        public string Secret { get; set; }
    }
}
