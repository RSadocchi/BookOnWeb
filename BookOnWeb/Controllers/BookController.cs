using BookOnWeb.Domain.Interfaces;
using BookOnWeb.DTO;
using BookOnWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookOnWeb.Controllers
{
    [Route("[controller]")]
    public class BookController(
        ILogger<BookController> _logger,
        IAppService _appService) : Controller
    {
        [HttpGet("")]
        public async Task<IActionResult> List()
        {
            var data = await _appService.Libri_GetAsync();
            List<LibroDTO> dtos = [];
            foreach (var d in data)
                dtos.Add(_appService.MappaENTITYsuDTO(entity: d, dto: null));

            return View(dtos);
        }


        [HttpGet("edit"), HttpGet("edit/{id}")]
        public async Task<IActionResult> EditPartial(int? id)
        {
            LibroDTO dto = new();
            if (id.GetValueOrDefault() > 0)
            {
                var entity = await _appService.Libri_FindAsync(id: id.GetValueOrDefault());
                ArgumentNullException.ThrowIfNull(entity, nameof(Libro));
                dto = _appService.MappaENTITYsuDTO(entity: entity, dto: dto);
            }
            return PartialView("_BookEditPartial", dto);
        }

        [HttpPost("edit"), ValidateAntiForgeryToken]
        public async Task<IActionResult> AjaxEdit([FromForm] LibroDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(LibroDTO));
            if (!ModelState.IsValid)
                throw new InvalidOperationException(string.Join(
                    " | ",
                    ModelState
                        .Where(x => x.Value != null && x.Value.Errors != null)
                        .SelectMany(x => x.Value!.Errors.Select(xx => xx.ErrorMessage))));

            try
            {
                await _appService.Libri_SaveAsync(dto: dto);
                return Json(new { succeeded = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Libri AjaxEdit");
                return Json(new { succeeded = false, message = ex.ToString() });
            }
        }
    }
}
