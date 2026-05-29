using BookOnWeb.Domain.Interfaces;
using BookOnWeb.DTO;
using BookOnWeb.Models;
using BookOnWeb.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BookOnWeb.Domain.Implementations
{
    public class AppService(
        ILogger<AppService> _logger,
        ILibroRepository _libroRepository,
        IAutoreRepository _autoreRepository) : IAppService
    {
        /*
         * **************************************************
         * GESTIONE LIBRI
         * **************************************************
         */

        /// <summary>
        /// Questa funzione restituisce sempre una lista, indipendentemente che i record recuperati siano 0, 1 o molti
        /// </summary>
        /// <param name="libri_ids">Se valorizzato cerca per <see cref="BookOnWeb.Models.Libro.Id"/></param>
        /// <param name="anno">Se valorizzato cerca per <see cref="BookOnWeb.Models.Libro.AnnoPubblicazione"/></param>
        /// <param name="titolo">Se valorizzato cerca per <see cref="BookOnWeb.Models.Libro.Titolo"/></param>
        /// <param name="trama">Se valorizzato cerca per <see cref="BookOnWeb.Models.Libro.Trama"/></param>
        /// <param name="nomeAutore">Se valorizzato cerca per <see cref="BookOnWeb.Models.Autore.Id"/></param>
        /// <param name="autoreId">Se valorizzato cerca per <see cref="BookOnWeb.Models.Autore.Nome"/> o <see cref="BookOnWeb.Models.Autore.Cognome"/></param>
        /// <param name="ricercaTestoEsatto">Se valorizzato cerca e se titolo o nomeAutore sono valorizzati, indica se cercare il testo esatto o una parte di esso</param>
        /// <returns>Una collezione di Libri</returns>
        public async Task<List<Libro>> Libri_GetAsync(
            int[]? libri_ids = null,
            int? anno = null,
            string? titolo = null,
            string? trama = null,
            string? nomeAutore = null,
            int? autoreId = null,
            bool ricercaTestoEsatto = false)
        {
            var query = await _libroRepository.GetAllAsync();

            //carico le propietà di navigazione
            query = query
                .Include(x => x.Autore)
                .Include(x => x.Prestiti);

            if (libri_ids?.Length > 0)
                query = query.Where(x => libri_ids.Contains(x.Id));

            if (anno.HasValue)
                query = query.Where(x => x.AnnoPubblicazione.HasValue && x.AnnoPubblicazione.Value == anno.Value);

            if (!string.IsNullOrWhiteSpace(titolo))
            {
                if (ricercaTestoEsatto)
                    query = query.Where(x => x.Titolo.ToLower().Trim() == titolo.ToLower().Trim());
                else
                    query = query.Where(x => x.Titolo.ToLower().Trim().Contains(titolo.ToLower().Trim()));
            }

            if (!string.IsNullOrWhiteSpace(trama))
                query = query.Where(x => !string.IsNullOrWhiteSpace(x.Trama) && x.Trama.ToLower().Trim().Contains(trama.ToLower().Trim()));

            if (!string.IsNullOrWhiteSpace(nomeAutore))
            {
                if (ricercaTestoEsatto)
                    query = query.Where(x =>
                        //la prima ricerca la fa solo nel nome
                        x.Autore.Nome.ToLower().Trim() == nomeAutore.ToLower().Trim()
                        //se non ci sono risultati cerca nel cognome
                        || x.Autore.Nome.ToLower().Trim() == nomeAutore.ToLower().Trim()
                        //se non ci sono risultati cerca nelle combinazioni di nome e cognome
                        || (x.Autore.Nome.ToLower().Trim() + " " + x.Autore.Cognome.ToLower().Trim()) == nomeAutore.ToLower().Trim()
                        || (x.Autore.Cognome.ToLower().Trim() + " " + x.Autore.Nome.ToLower().Trim()) == nomeAutore.ToLower().Trim()
                    );
                else
                    query = query.Where(x =>
                        //la prima ricerca la fa solo nel nome
                        x.Autore.Nome.ToLower().Trim().Contains(nomeAutore.ToLower().Trim())
                        //se non ci sono risultati cerca nel cognome
                        || x.Autore.Nome.ToLower().Trim().Contains(nomeAutore.ToLower().Trim())
                        //se non ci sono risultati cerca nelle combinazioni di nome e cognome
                        || (x.Autore.Nome.ToLower().Trim() + " " + x.Autore.Cognome.ToLower().Trim()).Contains(nomeAutore.ToLower().Trim())
                        || (x.Autore.Cognome.ToLower().Trim() + " " + x.Autore.Nome.ToLower().Trim()).Contains(nomeAutore.ToLower().Trim())
                    );
            }

            if (autoreId.HasValue)
                query = query.Where(x => x.AutoreId == autoreId.Value);

            return query.ToList();
        }

        /// <summary>
        /// Questa funzione restituisce 1 solo risultato (se trovato), null se non trova nulla, ma soprattutto da errore se per i parametri passati ci sono più risultati
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un oggetto di tipo Libro, se trovato</returns>
        public async Task<Libro?> Libri_FindAsync(int id)
            => (await Libri_GetAsync(libri_ids: [id])).SingleOrDefault();
        /*
         * SingleOrDefault => restituisce 1 elemento oppure null se la collezione è vuota, se ci sono più elementi nella lista da errore 
         *          (questo ci aiuta a evitare di prendere un elemnto che non è quello che ci aspettiamo)
         * 
         * FirstOrDefault => ci restituisce il primo elemento della lista, null se non ce ne sono
         * 
         * LastOrDefault => ci restituisce l'ultimo elemento della lista, null se non ce ne sono
         */

        public Libro MappaDTOsuEntity(LibroDTO dto, Libro? entity)
        {
            // l'oggetto DTO non può essere nullo, per completezza andrebbero fatte anche delle altre validazioni
            //  come ad esempio che AutoreId non sia <= 0
            ArgumentNullException.ThrowIfNull(dto, nameof(LibroDTO));
            entity ??= new();
            /*
             * queste if con assegnazione sono equivalenti:
             *  entity ??= new();
             *  if (entity is null) entity = new();
             *  if (entity == null) entity = new Libro();
             */

            // controllo che non stiamo cercando di salvare un dto con, ad esempio, id = 2 su una entity con id = 5
            if (dto.Id != entity.Id) throw new InvalidOperationException();

            //mappo i campi della datella Libro con quelli del DTO
            entity.Titolo = dto.Titolo;
            entity.AutoreId = dto.AutoreId;
            entity.Trama = dto.Trama;
            entity.AnnoPubblicazione = dto.AnnoPubblicazione;

            return entity;
        }

        public LibroDTO MappaENTITYsuDTO(Libro entity, LibroDTO? dto)
        {
            dto ??= new();

            dto.Id = entity.Id;
            dto.Titolo = entity.Titolo;
            dto.Trama = entity.Trama;
            dto.AutoreId = entity.AutoreId;
            dto.AutoreNome = entity.Autore.Nome;
            dto.AutoreCognome = entity.Autore.Cognome;
            dto.NrCopieInPrestito = entity.Prestiti
                ?.Where(x => x.DataRestituzione.HasValue == false) //considero solo quelli che non sono stati restituiti
                ?.Count() ?? 0;

            return dto;
        }

        /// <summary>
        /// Aggiunge o aggiorna un libro a database, partendo dal DTO
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">se dto è null o se non recupera il record da aggiornare dal database</exception>
        public async Task<Libro> Libri_SaveAsync(LibroDTO dto)
        {
            // l'oggetto DTO non può essere nullo, per completezza andrebbero fatte anche delle altre validazioni
            //  come ad esempio che AutoreId non sia <= 0
            ArgumentNullException.ThrowIfNull(dto, nameof(LibroDTO));

            // creo l'oggetto Libro nullo
            Libro? entity = null;
            // se ho un Id nel dto recupero dal database quel record, se non lo trovo do errore, vuol dire che l'id è sbagliato
            if (dto.Id > 0) entity = await Libri_FindAsync(id: dto.Id) ?? throw new ArgumentNullException(nameof(Libro));
            // mappo il DTO nella classe del database
            entity = MappaDTOsuEntity(dto: dto, entity: entity);

            return await Libri_SaveAsync(entity: entity);
        }

        /// <summary>
        /// Aggiunge o aggiorna un libro a database, partendo da una classe Libro del db
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">se il parametro entity è null o se non recupera il record da aggiornare dal database</exception>
        public async Task<Libro> Libri_SaveAsync(Libro entity)
        {
            // l'oggetto DTO non può essere nullo, per completezza andrebbero fatte anche delle altre validazioni
            //  come ad esempio che AutoreId non sia <= 0
            ArgumentNullException.ThrowIfNull(entity, nameof(Libro));

            try
            {
                // se l'id è maggiore di 0 devo aggiornare, altrimenti aggiungere
                if (entity.Id > 0)
                    entity = await _libroRepository.UpdateAsync(entity);
                else
                    entity = await _libroRepository.AddAsync(entity);

                await _libroRepository.UnitOfWork.SaveChangesAsync();

                _logger.LogInformation("Libro salvato con id {id}", entity.Id);

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il salvataggio del libro");
                throw;
            }
        }

        /*
         * **************************************************
         * GESTIONE AUTORI
         * **************************************************
         */

        public async Task<List<Autore>> Autori_GetAsync(
            int[]? ids = null,
            string? nome = null,
            bool ricercaTestoEsatto = false)
        {
            var query = await _autoreRepository.GetAllAsync();

            query = query
                .Include(x => x.Libri);

            if (ids?.Length > 0)
                query = query.Where(x => ids.Contains(x.Id));

            if (!string.IsNullOrWhiteSpace(nome))
            {
                if (ricercaTestoEsatto)
                    query = query.Where(x =>
                        //la prima ricerca la fa solo nel nome
                        x.Nome.ToLower().Trim() == nome.ToLower().Trim()
                        //se non ci sono risultati cerca nel cognome
                        || x.Nome.ToLower().Trim() == nome.ToLower().Trim()
                        //se non ci sono risultati cerca nelle combinazioni di nome e cognome
                        || (x.Nome.ToLower().Trim() + " " + x.Cognome.ToLower().Trim()) == nome.ToLower().Trim()
                        || (x.Cognome.ToLower().Trim() + " " + x.Nome.ToLower().Trim()) == nome.ToLower().Trim()
                    );
                else
                    query = query.Where(x =>
                        //la prima ricerca la fa solo nel nome
                        x.Nome.ToLower().Trim().Contains(nome.ToLower().Trim())
                        //se non ci sono risultati cerca nel cognome
                        || x.Nome.ToLower().Trim().Contains(nome.ToLower().Trim())
                        //se non ci sono risultati cerca nelle combinazioni di nome e cognome
                        || (x.Nome.ToLower().Trim() + " " + x.Cognome.ToLower().Trim()).Contains(nome.ToLower().Trim())
                        || (x.Cognome.ToLower().Trim() + " " + x.Nome.ToLower().Trim()).Contains(nome.ToLower().Trim())
                    );
            }

            return query.ToList();
        }

        public async Task<Autore?> Autori_FindAsync(int id)
            => (await Autori_GetAsync(ids: [id])).SingleOrDefault();

        public Autore MappaDTOsuEntity(AutoreDTO dto, Autore? entity)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(LibroDTO));
            entity ??= new();

            if (dto.Id != entity.Id) throw new InvalidOperationException();

            entity.Nome = dto.Nome;
            entity.Cognome = dto.Cognome;

            return entity;
        }

        public AutoreDTO MappaENTITYsuDTO(Autore entity, AutoreDTO? dto)
        {
            dto ??= new();

            dto.Id = entity.Id;
            dto.Nome = entity.Nome;
            dto.Cognome = entity.Cognome;
            dto.NrLibri = entity.Libri?.Count() ?? 0;

            return dto;
        }

        public async Task<Autore> Autori_SaveAsync(AutoreDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(AutoreDTO));

            Autore? entity = null;
            if (dto.Id > 0) entity = await Autori_FindAsync(id: dto.Id) ?? throw new ArgumentNullException(nameof(Autore));
            entity = MappaDTOsuEntity(dto: dto, entity: entity);

            return await Autori_SaveAsync(entity: entity);
        }

        public async Task<Autore> Autori_SaveAsync(Autore entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(Autore));

            try
            {
                // se l'id è maggiore di 0 devo aggiornare, altrimenti aggiungere
                if (entity.Id > 0)
                    entity = await _autoreRepository.UpdateAsync(entity);
                else
                    entity = await _autoreRepository.AddAsync(entity);

                await _autoreRepository.UnitOfWork.SaveChangesAsync();

                _logger.LogInformation("Autore salvato con id {id}", entity.Id);

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il salvataggio dell'autore");
                throw;
            }
        }

        #region GESTIONE PRESTITI
        public async Task<List<Prestito>> Prestiti_GetAsync(
            int[]? ids = null,
            string? nomeUtente = null,
            string? email = null,
            string? cellulare = null,
            int? libroId = null,
            bool ricercaTestoEsatto = false,
            bool? aperti = null)
        {
            var query = _libroRepository.UnitOfWork.Prestiti.AsQueryable();

            query = query
                .Include(x => x.Libro).ThenInclude(x => x.Autore);

            if (ids?.Length > 0)
                query = query.Where(x => ids.Contains(x.Id));

            if (!string.IsNullOrWhiteSpace(nomeUtente))
            {
                if (ricercaTestoEsatto)
                    query = query.Where(x => x.NomeUtente.ToLower().Trim() == nomeUtente.ToLower().Trim());
                else
                    query = query.Where(x => x.NomeUtente.ToLower().Trim().Contains(nomeUtente.ToLower().Trim()));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                if (ricercaTestoEsatto)
                    query = query.Where(x => !string.IsNullOrWhiteSpace(x.Email) && x.Email.ToLower().Trim() == email.ToLower().Trim());
                else
                    query = query.Where(x => !string.IsNullOrWhiteSpace(x.Email) && x.Email.ToLower().Trim().Contains(email.ToLower().Trim()));
            }

            if (!string.IsNullOrWhiteSpace(cellulare))
            {
                if (ricercaTestoEsatto)
                    query = query.Where(x => x.Cellulare.ToLower().Trim() == cellulare.ToLower().Trim());
                else
                    query = query.Where(x => x.Cellulare.ToLower().Trim().StartsWith(cellulare.ToLower().Trim()));
            }

            if (libroId.HasValue)
                query = query.Where(x => x.LibroId == libroId.Value);

            if (aperti.HasValue)
                query = query.Where(x => x.DataRestituzione.HasValue != aperti.GetValueOrDefault());

            return query.ToList();
        }

        public async Task<Prestito?> Prestiti_FindAsync(int id)
            => (await Prestiti_GetAsync(ids: [id])).SingleOrDefault();

        public Prestito MappaDTOsuEntity(PrestitoDTO dto, Prestito? entity)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(PrestitoDTO));
            entity ??= new();

            if (dto.Id != entity.Id) throw new InvalidOperationException();

            entity.LibroId = dto.LibroId;
            entity.DataPrestito = dto.DataPrestito;
            entity.NomeUtente = dto.NomeUtente;
            entity.Email = dto.Email;
            entity.Cellulare = dto.Cellulare;
            entity.DataRestituzione ??= dto.DataRestituzione; // la data di restituzione la aggiorno solo se è nulla

            return entity;
        }

        public PrestitoDTO MappaENTITYsuDTO(Prestito entity, PrestitoDTO? dto)
        {
            dto ??= new();

            dto.Id = entity.Id;
            dto.NomeUtente = entity.NomeUtente;
            dto.Email = entity.Email;
            dto.Cellulare = entity.Cellulare;

            dto.LibroId = entity.LibroId;
            dto.DataPrestito = entity.DataPrestito;
            dto.DataRestituzione = entity.DataRestituzione;

            dto.TitoloLibro = entity.Libro?.Titolo;
            dto.NomeCompletoAutore = entity.Libro?.Autore is not null
                ? $"{entity.Libro.Autore.Nome} {entity.Libro.Autore.Cognome}"
                : null;

            return dto;
        }

        public async Task<Prestito> Prestiti_SaveAsync(PrestitoDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(PrestitoDTO));

            Prestito? entity = null;
            if (dto.Id > 0) entity = await Prestiti_FindAsync(id: dto.Id) ?? throw new ArgumentNullException(nameof(Prestito));
            entity = MappaDTOsuEntity(dto: dto, entity: entity);

            return await Prestiti_SaveAsync(entity: entity);
        }

        public async Task<Prestito> Prestiti_SaveAsync(Prestito entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(Prestito));

            try
            {
                // se l'id è maggiore di 0 devo aggiornare, altrimenti aggiungere
                // questo è un approccio diverso, non avendo il repository dei prestiti
                if (entity.Id > 0)
                    _libroRepository.UnitOfWork.Entry<Prestito>(entity).State = EntityState.Modified;
                else
                    entity = _libroRepository.UnitOfWork.Prestiti.Add(entity).Entity;

                await _libroRepository.UnitOfWork.SaveChangesAsync();

                _logger.LogInformation("Prestito salvato con id {id}", entity.Id);

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il salvataggio del prestito");
                throw;
            }
        }
        #endregion
    }
}
