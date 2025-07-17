using Microsoft.AspNetCore.Mvc;
using TSZHApp2.Data;
using TSZHApp2.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace TSZHApp2.Controllers
{
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public NewsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpPost]
        public async Task<IActionResult> AddNews(string title, string text, IFormFile image)
        {
            if (string.IsNullOrWhiteSpace(title))
                return BadRequest("Заголовок обязателен");

            string ?imagePath = null;

            if (image != null)
            {
                var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                imagePath = "/uploads/" + fileName;
            }

            var news = new News
            {
                Title = title,
                Text = text,
                ImagePath = imagePath
            };

            _context.News.Add(news);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
