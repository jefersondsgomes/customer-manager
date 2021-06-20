using CustomerManager.Models.Helpers.Interfaces;

namespace CustomerManager.Models.Helpers
{
    public class Settings : ISettings
    {
        public string Secret { get; set; }
    }
}