using BookOnWeb.Data;
using BookOnWeb.Domain.Interfaces;
using BookOnWeb.Models;

namespace BookOnWeb.Domain.Implementations
{
    public class LibroRepository : Repository<Libro, int>, ILibroRepository
    {
        public LibroRepository(AppDbContext context) : base(context: context)
        {
            
        }
    }
}
