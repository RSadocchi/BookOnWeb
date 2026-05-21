namespace BookOnWeb.DTO
{
    public class LibroDTO
    {
        public int Id { get; set; }
        public string Titolo { get; set; } = null!;
        public int AutoreId { get; set; }
        public string? Trama { get; set; }
        public int? AnnoPubblicazione { get; set; }

        public string AutoreNome { get; set; } = null!;
        public string AutoreCognome { get; set; } = null!;
        public int NrCopieInPrestito { get; set; }
    }
}
