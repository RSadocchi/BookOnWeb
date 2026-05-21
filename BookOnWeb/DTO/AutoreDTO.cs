namespace BookOnWeb.DTO
{
    public class AutoreDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Cognome { get; set; } = null!;
        public int NrLibri { get; set; }
    }
}
