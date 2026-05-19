using BookOnWeb.Data;
using System.ComponentModel.DataAnnotations;

namespace BookOnWeb.Models
{
    public class Autore : Entity
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Nome { get; set; } = null!;
        
        [Required]
        [MaxLength(50)]
        public string Cognome { get; set; } = null!;

        public virtual ICollection<Libro> Libri { get; set; } = []; // inizializza la collezione di libri associati a questo autore
    }
}
