using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using SuccessAppraiserWeb.Areas.Goal.DTO;
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
        private readonly IMapper _mapper;

        public HomeController(CustomUserManager userManager, ApplicationDbContext dbContext, IGoalTemplateRepository goalTemplateRepository,
            IMapper mapper)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _goalTemplateRepository = goalTemplateRepository;
            _mapper = mapper;
        }

        [Authorize]
        public async  Task<IActionResult> Index()
        {
            ViewBag.SystemTemplates = await _goalTemplateRepository.GetSystemTemplatesAsync();
            ViewBag.UserTemplates = await _goalTemplateRepository.GetUserTemplatesAsync(HttpContext.User);

            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateGoal(CreateGoalDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ApplicationUser? user = await _userManager.GetUserAsync(HttpContext.User);
            
            if(user == null)
            {
                return Unauthorized();
            }

            GoalItem newGoal = _mapper.Map<CreateGoalDto, GoalItem>(model);

            newGoal.User = user;
            _dbContext.Goals.Add(newGoal);
            await _dbContext.SaveChangesAsync();

            return RedirectToActionPermanent("Index");
            

        }

        public IActionResult CreateGoalModal()
        {
            return PartialView();
        }

        public IActionResult CreateStatelessDateModal()
        {
            return PartialView();
        }


    }
}
