using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Neowrk.Library.Api.Security;
using Google.Apis.Auth;
using System.Collections.Generic;

namespace Neowrk.Library.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        public class AuthenticateRequest
        {
            [Required]
            public string IdToken { get; set; }
        }

        private readonly JwtGenerator _jwtGenerator;

        public UserController(IConfiguration configuration)
        {
            _jwtGenerator = new JwtGenerator(configuration.GetValue<string>("JwtPrivateSigninKey"));
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateRequest data)
        {
            GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings();
            settings.Audience = new List<string>() { "359020175538-oqos2e534hpo2kg68hpj6av0gl3uuobk.apps.googleusercontent.com" };
            GoogleJsonWebSignature.Payload payload = GoogleJsonWebSignature.ValidateAsync(data.IdToken, settings).Result;
            return Ok(new { AuthToken = _jwtGenerator.CreateUserAuthToken(payload.Email) });
        }
    }
}