using BookOnWeb.Domain.Interfaces;
using BookOnWeb.DTO;
using BookOnWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookOnWeb.Controllers
{
    [Route("[controller]")]
    public class SharingController(
        ILogger<SharingController> _logger,
        IAppService _appService) : Controller
    {
        [HttpGet("")]
        public async Task<IActionResult> List() => View();

        [HttpGet("table")]
        public async Task<IActionResult> ListTable([FromQuery] bool? aperti = true)
        {
            var data = await _appService.Prestiti_GetAsync(aperti: aperti);
            List<PrestitoDTO> dtos = [];
            foreach (var d in data)
                dtos.Add(_appService.MappaENTITYsuDTO(entity: d, dto: null));

            return PartialView("ListaTablePartial", dtos.OrderBy(x => x.TitoloLibro).ToList());
        }


        [HttpGet("edit"), HttpGet("edit/{id}")]
        public async Task<IActionResult> EditPartial(int? id)
        {
            PrestitoDTO dto = new();
            if (id.GetValueOrDefault() > 0)
            {
                var entity = await _appService.Prestiti_FindAsync(id: id.GetValueOrDefault());
                ArgumentNullException.ThrowIfNull(entity, nameof(Prestito));
                dto = _appService.MappaENTITYsuDTO(entity: entity, dto: dto);
            }
            return PartialView("_SharingEditPartial", dto);
        }

        [HttpPost("edit"), ValidateAntiForgeryToken]
        public async Task<IActionResult> AjaxEdit([FromForm] PrestitoDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(PrestitoDTO));
            if (!ModelState.IsValid)
                throw new InvalidOperationException(string.Join(
                    " | ",
                    ModelState
                        .Where(x => x.Value != null && x.Value.Errors != null)
                        .SelectMany(x => x.Value!.Errors.Select(xx => xx.ErrorMessage))));

            try
            {
                await _appService.Prestiti_SaveAsync(dto: dto);
                return Json(new { succeeded = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Prestiti AjaxEdit");
                return Json(new { succeeded = false, message = ex.ToString() });
            }
        }
    }
}
