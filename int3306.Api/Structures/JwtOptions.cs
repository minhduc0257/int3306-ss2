using Microsoft.IdentityModel.Tokens;

namespace int3306.Api.Structures
{
    public class JwtOptions
    {
#pragma warning disable CS8618
        public SecurityKey SecurityKey { get; init; }
        public string Issuer { get; init; }
        public string Audience { get; init; }
#pragma warning restore CS8618
    }
}