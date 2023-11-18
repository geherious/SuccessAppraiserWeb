using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuccessAppraiserWeb.Data.Identity;
using System.Text.Json.Serialization;
using System.Text.Json;
using AutoMapper;
using SuccessAppraiserWeb.Areas.Goal.models;
using SuccessAppraiserWeb.Data.Goal.Interfaces;
using Microsoft.EntityFrameworkCore;
using SuccessAppraiserWeb.Areas.Goal.DTO;
using SuccessAppraiserWeb.Data;

namespace SuccessAppraiserWeb.Areas.Goal.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        private readonly CustomUserManager _userManager;
        private readonly IGoalRepository _goalRepository;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GoalController(CustomUserManager userManager, IGoalRepository goalRepository, ApplicationDbContext dbContext, IMapper mapper)
        {
            _userManager = userManager;
            _goalRepository = goalRepository;
            _dbContext = dbContext;
            _mapper = mapper;
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateGoal([FromForm] CreateGoalDto model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null) return Unauthorized();

            GoalItem newGoal = _mapper.Map<CreateGoalDto, GoalItem>(model);

            newGoal.User = user;
            _dbContext.Goals.Add(newGoal);
            await _dbContext.SaveChangesAsync();

            return RedirectToActionPermanent("Index", controllerName: "Home", new {area = "Goal"});
        }

        public async Task<IActionResult> CreateGoalDate([FromBody] CreateGoalDateDto model)
        {
            if (!ModelState.IsValid) return BadRequest();

            GoalDate newGoalDate = _mapper.Map<CreateGoalDateDto, GoalDate>(model);
            _dbContext.GoalDates.Add(newGoalDate);
            await _dbContext.SaveChangesAsync();
            return Ok(new {id = newGoalDate.Id});

        }
    }
}
