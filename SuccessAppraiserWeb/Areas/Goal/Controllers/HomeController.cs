using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuccessAppraiserWeb.Areas.Goal.models;
using SuccessAppraiserWeb.Areas.Identity.models;
using SuccessAppraiserWeb.Data;
using SuccessAppraiserWeb.Data.Goal.Interfaces;
using SuccessAppraiserWeb.Data.Goal.Repositories;
using SuccessAppraiserWeb.Data.Identity;
using System.Diagnostics;

namespace SuccessAppraiserWeb.Areas.Goal.Controllers
{
    [Area("Goal")]
    public class HomeController : Controller
    {
        private readonly CustomUserManager _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IGoalTemplateRepository _goalTemplateRepository;

        public HomeController(CustomUserManager userManager, ApplicationDbContext dbContext, IGoalTemplateRepository goalTemplateRepository)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _goalTemplateRepository = goalTemplateRepository;
        }

        [Authorize]
        public async  Task<IActionResult> Index()
        {
            ViewBag.SystemTemplates = await _goalTemplateRepository.GetSystemTemplatesAsync();
            ViewBag.UserTemplates = await _goalTemplateRepository.GetUserTemplatesAsync(HttpContext.User);

            //DayState easy = _dbContext.GoalStates.Find(1);
            //GoalItem goal = _dbContext.Goals.Find(7);
            //GoalDate date = new GoalDate();
            //date.State = easy;
            //date.Goal = goal;
            //date.Date = DateTime.Now;
            //_dbContext.GoalDates.Add(date);
            //_dbContext.SaveChanges();
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateGoal(GoalItem model)
        {
            ApplicationUser? user = await _userManager.GetUserAsync(HttpContext.User);
            
            if(user == null)
            {
                return Unauthorized();
            }

            model.User = user;
            _dbContext.Goals.Add(model);
            await _dbContext.SaveChangesAsync();

            return RedirectToActionPermanent("Index");
            

        }


    }
}
