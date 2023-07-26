﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuccessAppraiserWeb.Areas.Goal.models;
using SuccessAppraiserWeb.Areas.Identity.models;
using SuccessAppraiserWeb.Data;
using SuccessAppraiserWeb.Data.Identity;
using System.Diagnostics;

namespace SuccessAppraiserWeb.Areas.Goal.Controllers
{
    [Area("Goal")]
    public class HomeController : Controller
    {
        private readonly CustomUserManager _userManager;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(CustomUserManager userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            var goals = _userManager.GetUserGoals(HttpContext.User);

            ViewBag.Goals = goals;
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
            _dbContext.SaveChanges();


            var goals = _userManager.GetUserGoals(HttpContext.User);
            ViewBag.Goals = goals;

            return View("Index");
            

        }


    }
}
