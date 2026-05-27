using BookOnWeb.Domain.Interfaces;
using BookOnWeb.DTO;
using BookOnWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookOnWeb.Controllers
{
    [Route("[controller]")]
    public class AuthorController(
        ILogger<AuthorController> _logger,
        IAppService _appService) : Controller
    {
        [HttpGet("")]
        public async Task<IActionResult> List()
        {
            var data = await _appService.Autori_GetAsync();
            List<AutoreDTO> dtos = [];
            foreach (var d in data)
                dtos.Add(_appService.MappaENTITYsuDTO(entity: d, dto: null));

            return View(dtos);
        }


        [HttpGet("edit"), HttpGet("edit/{id}")]
        public async Task<IActionResult> EditPartial(int? id)
        {
            AutoreDTO dto = new();
            if (id.GetValueOrDefault() > 0)
            {
                var entity = await _appService.Autori_FindAsync(id: id.GetValueOrDefault());
                ArgumentNullException.ThrowIfNull(entity, nameof(Autore));
                dto = _appService.MappaENTITYsuDTO(entity:entity, dto: dto);
            }
            return PartialView("_AuthorEditPartial", dto);
        }

        [HttpPost("edit"), ValidateAntiForgeryToken]
        public async Task<IActionResult> AjaxEdit([FromForm] AutoreDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(AutoreDTO));
            if (!ModelState.IsValid)
                throw new InvalidOperationException(string.Join(
                    " | ",
                    ModelState
                        .Where(x => x.Value != null && x.Value.Errors != null)
                        .SelectMany(x => x.Value!.Errors.Select(xx => xx.ErrorMessage))));

            try
            {
                await _appService.Autori_SaveAsync(dto: dto);
                return Json(new { succeeded = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Autore AjaxEdit");
                return Json(new { succeeded = false, message = ex.ToString() });
            }
        }
    }
}
