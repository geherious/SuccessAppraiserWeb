using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuccessAppraiserWeb.Areas.Goal.DTO;
using SuccessAppraiserWeb.Areas.Goal.models;
using SuccessAppraiserWeb.Data;
using SuccessAppraiserWeb.Data.Goal.Interfaces;
using SuccessAppraiserWeb.Data.Identity;

namespace SuccessAppraiserWeb.Areas.Goal.Controllers;

[Area("Goal")]
public class HomeController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IGoalTemplateRepository _goalTemplateRepository;
    private readonly IMapper _mapper;
    private readonly CustomUserManager _userManager;

    public HomeController(CustomUserManager userManager, ApplicationDbContext dbContext,
        IGoalTemplateRepository goalTemplateRepository,
        IMapper mapper)
    {
        _userManager = userManager;
        _dbContext = dbContext;
        _goalTemplateRepository = goalTemplateRepository;
        _mapper = mapper;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        ViewBag.SystemTemplates = await _goalTemplateRepository.GetSystemTemplatesAsync();
        ViewBag.UserTemplates = await _goalTemplateRepository.GetUserTemplatesAsync(HttpContext.User);

        return View();
    }

    public IActionResult CreateGoalModal()
    {
        return PartialView();
    }

    public IActionResult CreateStatelessDateModal()
    {
        return PartialView();
    }

    public IActionResult Toasts()
    {
        return PartialView();
    }
}