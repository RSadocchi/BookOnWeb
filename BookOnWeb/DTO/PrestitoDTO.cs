namespace BookOnWeb.DTO
{
    public class PrestitoDTO
    {
        public int Id { get; set; }
        public DateOnly DataPrestito { get; set; }
        public int LibroId { get; set; }
        public string NomeUtente { get; set; } = null!;
        public string? Email { get; set; }
        public string Cellulare { get; set; } = null!;
        public DateTime? DataRestituzione { get; set; }

        public string? TitoloLibro { get; set; }
        public string? NomeCompletoAutore { get; set; }
    }
}
