using Camp_rating.Data;
using Camp_rating.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class ReviewController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ReviewController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Create(int campsiteId)
    {
        var campsite = _context.Campsites.Find(campsiteId);
        if (campsite == null)
        {
            return NotFound();
        }

        ViewBag.CampsiteId = campsiteId;
        ViewBag.CampsiteName = campsite.Name;
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(int campsiteId, Review review)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            var campsite = await _context.Campsites.FindAsync(campsiteId);

            if (campsite == null)
            {
                return NotFound();
            }

            review.UserId = user.Id;
            review.User = user;
            review.Campsite = campsite;

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Campsite", new { id = campsiteId });
        }

        ViewBag.CampsiteId = campsiteId;
        ViewBag.CampsiteName = _context.Campsites.Find(campsiteId)?.Name;
        return View(review);
    }
}