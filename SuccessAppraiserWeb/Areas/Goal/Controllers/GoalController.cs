using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuccessAppraiserWeb.Data.Identity;
using System.Text.Json.Serialization;
using System.Text.Json;
using SuccessAppraiserWeb.Areas.Goal.models;
using SuccessAppraiserWeb.Data.Goal.Interfaces;

namespace SuccessAppraiserWeb.Areas.Goal.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        private readonly CustomUserManager _userManager;
        private readonly IGoalRepository _goalRepository;

        public GoalController(CustomUserManager userManager, IGoalRepository goalRepository)
        {
            _userManager = userManager;
            _goalRepository = goalRepository;
        }

        [Authorize]
        public async Task<IActionResult> UserGoals()
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            var data = await _goalRepository.GetGoalsByUserAsync(HttpContext.User);
            return new JsonResult(data, options);
        }

        [Authorize]
        [HttpPost]
        async public Task<IActionResult> DeleteGoal(int Id)
        {
            await _goalRepository.DeleteByUser(HttpContext.User, Id);
            return Ok();
        }
    }
}
