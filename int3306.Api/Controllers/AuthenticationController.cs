using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using int3306.Api.Structures;
using int3306.Entities;
using int3306.Entities.Models;
using int3306.Repository;
using int3306.Repository.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using BC = BCrypt.Net.BCrypt;
using ClaimTypes = int3306.Api.Structures.ClaimTypes;

namespace int3306.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : Controller
    {
        private readonly UserRepository userRepository;
        private readonly JwtOptions jwtOptions;

        public AuthenticationController(UserRepository userRepository, JwtOptions jwtOptions)
        {
            this.userRepository = userRepository;
            this.jwtOptions = jwtOptions;
        }
        
        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<IBaseResult<bool>>> Register([FromBody] RegisterModel credentials)
        {
            var user = await userRepository.GetByUsername(credentials.Username);
            if (user != null)
            {
                return Conflict(BaseResult<bool>.FromError("Username already exists!"));
            }

            var result = await userRepository.Register(
                credentials.Username, BC.HashPassword(credentials.Password), 
                credentials.Name,
                credentials.Email
            );
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<ActionResult<IBaseResult<LoginResultModel>>> Login([FromBody] CredentialModel credentials)
        {
            var user = await userRepository.GetByUsername(credentials.Username);
            if (user == null)
            {
                return Unauthorized(BaseResult<int>.FromError("No such combination found"));
            }

            var pass = BC.Verify(credentials.Password, user.Password);
            if (!pass)
            {
                return Unauthorized(BaseResult<int>.FromError("No such combination found"));
            }

            var token = GenerateJwt(user.Id);
            var model = new LoginResultModel { Token = token };
            return Ok(BaseResult<LoginResultModel>.FromSuccess(model));
        }

        private string GenerateJwt(int userId)
        {
            var securityKey = jwtOptions.SecurityKey;
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Id, userId.ToString())
            };
            var token = new JwtSecurityToken(jwtOptions.Issuer,
                jwtOptions.Audience,
                claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}