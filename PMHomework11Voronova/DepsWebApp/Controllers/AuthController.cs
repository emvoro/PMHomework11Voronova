using DepsWebApp.Filters;
using DepsWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DepsWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Method to register user account
        /// </summary>
        /// <param name="user">JSON of user data</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">this method is not implemented</exception>
        [HttpPost]
        [CustomExceptionFilter]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> Register([FromBody] User user)
        {
            _logger.LogInformation($"Register attempt by user : {user.Login}");
            throw new NotImplementedException();
        }
    }
}
