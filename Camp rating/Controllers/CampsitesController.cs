using Camp_rating.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CampsitesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public CampsitesController(ApplicationDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    [HttpGet]
    public IActionResult Upload(int id)
    {
        var campsite = _context.Campsites.Find(id);
        if (campsite == null)
        {
            return NotFound();
        }
        return View(campsite);
    }

    [HttpPost]
    public async Task<IActionResult> Upload(int id, IFormFile image)
    {
        var campsite = await _context.Campsites.FindAsync(id);
        if (campsite == null)
        {
            return NotFound();
        }

        if (image != null && image.Length > 0)
        {
            // Създаваме уникално име за файла
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "campsites");

            // Създаваме директорията, ако не съществува
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, fileName);

            // Запазваме файла
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            // Ако има стара снимка, изтриваме я
            if (!string.IsNullOrEmpty(campsite.ImagePath))
            {
                var oldImagePath = Path.Combine(_environment.WebRootPath, campsite.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // Обновяваме пътя до изображението
            campsite.ImagePath = "/images/campsites/" + fileName;
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = campsite.Id });
        }

        return View(campsite);
    }
    // Метод за търсене
    [HttpGet]
    public async Task<IActionResult> Search(string searchTerm, double? lat, double? lng, double? distance)
    {
        var query = _context.Campsites.AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(c => c.Name.Contains(searchTerm) || c.Description.Contains(searchTerm));
        }

        // Ако са подадени координати и разстояние, филтрираме по местоположение
        if (lat.HasValue && lng.HasValue && distance.HasValue)
        {
            // Това е опростен пример за изчисляване на разстояние
            var results = await query.ToListAsync();

            results = results.Where(c =>
            1.0 <= distance.Value).ToList();

            return View(results);
        }

        return View(await query.ToListAsync());
    }

    

    private double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }
}