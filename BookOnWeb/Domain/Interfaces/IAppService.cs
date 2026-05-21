using BookOnWeb.DTO;
using BookOnWeb.Models;

namespace BookOnWeb.Domain.Interfaces
{
    public interface IAppService
    {
        #region Autore
        Task<Autore?> Autori_FindAsync(int id);
        Task<List<Autore>> Autori_GetAsync(int[]? ids = null, string? nome = null, bool ricercaTestoEsatto = false);
        Task<Autore> Autori_SaveAsync(Autore entity);
        Task<Autore> Autori_SaveAsync(AutoreDTO dto);
        #endregion

        #region Libro
        Task<Libro?> Libri_FindAsync(int id);
        Task<List<Libro>> Libri_GetAsync(int[]? libri_ids = null, int? anno = null, string? titolo = null, string? trama = null, string? nomeAutore = null, int? autoreId = null, bool ricercaTestoEsatto = false);
        Task<Libro> Libri_SaveAsync(Libro entity);
        Task<Libro> Libri_SaveAsync(LibroDTO dto);
        #endregion
        
        #region Prestito
        Task<Prestito?> Prestiti_FindAsync(int id);
        Task<List<Prestito>> Prestiti_GetAsync(int[]? ids = null, string? nomeUtente = null, string? email = null, string? cellulare = null, int? libroId = null, bool ricercaTestoEsatto = false);
        Task<Prestito> Prestiti_SaveAsync(Prestito entity);
        Task<Prestito> Prestiti_SaveAsync(PrestitoDTO dto);
        #endregion

        #region Mapping da e verso DTO
        Autore MappaDTOsuEntity(AutoreDTO dto, Autore? entity);
        Libro MappaDTOsuEntity(LibroDTO dto, Libro? entity);
        Prestito MappaDTOsuEntity(PrestitoDTO dto, Prestito? entity);
        AutoreDTO MappaENTITYsuDTO(Autore entity, AutoreDTO? dto);
        LibroDTO MappaENTITYsuDTO(Libro entity, LibroDTO? dto);
        PrestitoDTO MappaENTITYsuDTO(Prestito entity, PrestitoDTO? dto);
        #endregion
    }
}