using BookOnWeb.Data;
using BookOnWeb.Models;

namespace BookOnWeb.Domain.Interfaces
{
    /// <summary>
    /// Questa dichiarazione è equivalente a quella di <see cref="LibroRepository"/>, viene usato quello che si chiama costruttore primario
    /// </summary>
    public class AutoreRepository(AppDbContext context) : Repository<Autore, int>(context: context), IAutoreRepository
    {

    }
}
