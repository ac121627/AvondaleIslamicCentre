using System.Diagnostics;
using AvondaleIslamicCentre.Models;
using Microsoft.AspNetCore.Mvc;

namespace AvondaleIslamicCentre.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Gallery()
        {
            var images = new List<string>();
            try
            {
                var imgDir = Path.Combine(_env.WebRootPath ?? "", "Images");
                if (Directory.Exists(imgDir))
                {
                    var files = Directory.GetFiles(imgDir)
                        .Where(f => {
                            var ext = Path.GetExtension(f).ToLowerInvariant();
                            return ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif" || ext == ".webp";
                        })
                        .OrderBy(f => f);

                    foreach (var file in files)
                    {
                        var fileName = Path.GetFileName(file);
                        images.Add(Url.Content("~/Images/" + fileName));
                    }
                }
            }
            catch
            {
                // ignore and return empty list
            }

            return View(images);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
