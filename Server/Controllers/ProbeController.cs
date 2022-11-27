using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Shared;

namespace Server.Controllers
{
    [Route("")]
    [ApiController]
    public class ProbeController : ControllerBase
    {
        [HttpGet]
        public IActionResult probe()
        {
            return Ok("It's alive");
        }
    }
}
