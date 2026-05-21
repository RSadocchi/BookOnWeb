using BookOnWeb.Domain.Interfaces;
using BookOnWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookOnWeb.Controllers
{
    public class HomeController(
        ILogger<HomeController> _logger,
        IAppService _appService) : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/error"), ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("/ajax/books-count")]
        public async Task<IActionResult> Ajax_GetBooksCount()
        {
            var count = await _appService.Libri_GetAsync();
            return Json(count.Count);
        }

        [HttpGet("/ajax/authors-count")]
        public async Task<IActionResult> Ajax_GetAuthorsCount()
        {
            var count = await _appService.Autori_GetAsync();
            return Json(count.Count);
        }

        [HttpGet("/ajax/shared-count")]
        public async Task<IActionResult> Ajax_GetSharedCount()
        {
            var count = await _appService.Prestiti_GetAsync();
            return Json(new
            {
                active = count.Where(x => x.DataRestituzione.HasValue == false).Count(),
                done = count.Where(x => x.DataRestituzione.HasValue).Count()
            });
        }
    }
}
