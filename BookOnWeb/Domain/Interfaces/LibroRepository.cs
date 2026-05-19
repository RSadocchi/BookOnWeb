using BookOnWeb.Data;
using BookOnWeb.Models;

namespace BookOnWeb.Domain.Interfaces
{
    public class LibroRepository : Repository<Libro, int>, ILibroRepository
    {
        public LibroRepository(AppDbContext context) : base(context: context)
        {
            
        }


    }
}
