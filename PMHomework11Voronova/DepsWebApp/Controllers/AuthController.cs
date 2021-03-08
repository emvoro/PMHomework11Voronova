using DepsWebApp.Filters;
using DepsWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DepsWebApp.Controllers
{
    /// <summary>
    /// Authentication Controller for registration
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
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
            throw new NotImplementedException();
        }
    }
}
