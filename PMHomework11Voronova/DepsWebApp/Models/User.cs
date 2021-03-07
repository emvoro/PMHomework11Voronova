using System.Text.Json.Serialization;

namespace DepsWebApp.Models
{
    public class User
    {
        [JsonPropertyName("login")]
        public string Login { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
