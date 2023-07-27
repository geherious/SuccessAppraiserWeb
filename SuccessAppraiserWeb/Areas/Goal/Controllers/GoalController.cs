﻿using Microsoft.AspNetCore.Authorization;
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
        public IActionResult UserGoals()
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            return new JsonResult(_userManager.GetUserGoals(HttpContext.User), options);
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteGoal(int Id)
        {
            Console.WriteLine(Id);
            var filtered = _userManager.GetUserGoals(HttpContext.User).Where(x => x.Id == Id).ToList();
            if (filtered.Count == 1)
            {
                _goalRepository.Delete(Id);
                return Ok();
            }
            return NotFound();
        }
    }
}