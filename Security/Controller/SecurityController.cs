using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Security.Inferastructure.IRepository;
using Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ICustomAuthicationManager customAuthicationManager;
        public SecurityController(ICustomAuthicationManager customAuthicationManager)
        {
            this.customAuthicationManager = customAuthicationManager;
        }
        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody] User user)
        {
            var token = customAuthicationManager.Authenicate
                (user.Username, user.Password);
            if (token==null)
            {
                return Unauthorized();
            }
            return Ok(token);
        }
    }
}
