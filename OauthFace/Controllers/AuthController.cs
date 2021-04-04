using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhoOwesWhom.Controllers
{
    [Route("Auth")]
    public class AuthController : Controller
    {
        [Route("sigin")]
        public IActionResult SigIn()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/" });

        }
    }
}
