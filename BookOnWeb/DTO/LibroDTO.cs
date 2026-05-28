namespace BookOnWeb.DTO
{
    public class LibroDTO
    {
        public int Id { get; set; }
        public string Titolo { get; set; } = null!;
        public int AutoreId { get; set; }
        public string? Trama { get; set; }
        public int? AnnoPubblicazione { get; set; }

        public string? AutoreNome { get; set; }
        public string? AutoreCognome { get; set; }
        public int NrCopieInPrestito { get; set; }
    }
}
