using BookOnWeb.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookOnWeb.Models
{
    public class Prestito : Entity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "date")] // specifica che la colonna nel database deve essere di tipo data (senza l'orario)
        public DateOnly DataPrestito { get; set; }

        public int LibroId { get; set; }

        [Required, MaxLength(100)]
        public string NomeUtente { get; set; } = null!;

        [MaxLength(100), EmailAddress] // indica che la stringa deve essere un indirizzo email valido
        public string? Email { get; set; }

        [MaxLength(30)]
        public string Cellulare { get; set; } = null!;

        [Column(TypeName = "datetime")] // specifica che la colonna nel database deve essere di tipo datetime (con data e orario)
        public DateTime? DataRestituzione { get; set; }


        public virtual Libro Libro { get; set; } = null!;
    }
}
