using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuccessAppraiserWeb.Data.Identity;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace SuccessAppraiserWeb.Areas.Goal.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        private readonly CustomUserManager _userManager;

        public GoalController(CustomUserManager userManager)
        {
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult UserGoals()
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            return new JsonResult(_userManager.GetUserGoals(HttpContext.User), options);
        }

    }
}
