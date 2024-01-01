using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace int3306.Entities.Models
{
    public class CredentialModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string? Name { get; set; } = "";
    }
}