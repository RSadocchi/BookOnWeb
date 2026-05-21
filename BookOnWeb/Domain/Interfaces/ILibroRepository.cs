using BookOnWeb.Data;
using BookOnWeb.Models;

namespace BookOnWeb.Domain.Interfaces
{
    public interface ILibroRepository : IRepository<Libro, int>
    {
    }

}
