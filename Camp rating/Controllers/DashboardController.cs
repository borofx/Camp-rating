using Camp_rating.Data;
using Camp_rating.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public DashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")] // Само за администратори
    public async Task<IActionResult> Stats()
    {
        var stats = new DashboardViewModel
        {
            TotalUsers = await _userManager.Users.CountAsync(),
            TotalCampsites = await _context.Campsites.CountAsync(),
            TotalReviews = await _context.Reviews.CountAsync(),
            MostReviewedCampsites = await _context.Campsites
                .Include(c => _context.Reviews.Where(r => r.Campsite.Id == c.Id))
                .OrderByDescending(c => _context.Reviews.Count(r => r.Campsite.Id == c.Id))
                .Take(5)
                .Select(c => new CampsiteStatsViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    ReviewCount = _context.Reviews.Count(r => r.Campsite.Id == c.Id)
                })
                .ToListAsync(),
            MostActiveUsers = await _userManager.Users
                .OrderByDescending(u => u.Reviews.Count)
                .Take(5)
                .Select(u => new UserStatsViewModel
                {
                    Id = u.Id,
                    Name = $"{u.FirstName} {u.LastName}",
                    ReviewCount = u.Reviews.Count
                })
                .ToListAsync()
        };

        return View(stats);
    }
}

// Модели за статистиката
public class DashboardViewModel
{
    public int TotalUsers { get; set; }
    public int TotalCampsites { get; set; }
    public int TotalReviews { get; set; }
    public List<CampsiteStatsViewModel> MostReviewedCampsites { get; set; } = new List<CampsiteStatsViewModel>();
    public List<UserStatsViewModel> MostActiveUsers { get; set; } = new List<UserStatsViewModel>();
}

public class CampsiteStatsViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ReviewCount { get; set; }
}

public class UserStatsViewModel
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int ReviewCount { get; set; }
}