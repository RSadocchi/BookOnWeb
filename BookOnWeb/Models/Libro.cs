using BookOnWeb.Data;
using System.ComponentModel.DataAnnotations;

namespace BookOnWeb.Models
{
    public class Libro : Entity
    {
        [Key] // indica che questa proprietà è la chiave primaria della tabella
        public int Id { get; set; }

        [Required] // indica che questa proprietà è obbligatoria e non può essere null
        [MaxLength(100)] // indica che la lunghezza massima della stringa è 100 caratteri
        public string Titolo { get; set; } = null!; // null! da un valore di defailt alla proprietà, ma al tempo stesso indica che non deve essere null
        
        public int AutoreId { get; set; }
        
        public string? Trama { get; set; } // string? invece indica che la proprietà può essere null

        public int? AnnoPubblicazione { get; set; }

        public virtual Autore Autore { get; set; } = null!;
        public virtual ICollection<Prestito> Prestiti { get; set; } = []; // inizializza la collezione di prestiti associati a questo libro
    }
}
