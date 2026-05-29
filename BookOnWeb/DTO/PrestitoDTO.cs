using System.ComponentModel.DataAnnotations;

namespace BookOnWeb.DTO
{
    public class PrestitoDTO
    {
        public int Id { get; set; }
        public DateOnly DataPrestito { get; set; } = DateOnly.FromDateTime(DateTime.Today);
        public int LibroId { get; set; }
        public string NomeUtente { get; set; } = null!;
        [EmailAddress]
        public string? Email { get; set; }
        public string Cellulare { get; set; } = null!;
        public DateOnly? DataRestituzione { get; set; }

        public string? TitoloLibro { get; set; }
        public string? NomeCompletoAutore { get; set; }
    }
}
