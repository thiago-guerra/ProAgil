using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {
        private readonly ProAgilContext _context;
        public ProAgilRepository(ProAgilContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        #region GERAL

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

         public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        #endregion
       
        #region EVENTOS

        public async Task<List<Evento>> GetAllEventoAsync(bool pIncludePalestrantes = false)
        {
           IQueryable<Evento> query = _context.Eventos.Include(x => x.Lotes).Include(x => x.RedesSociais);

           if(pIncludePalestrantes)
            query = query.Include(x => x.PalestranteEventos).ThenInclude(x => x.Palestrante);

            query = query.OrderByDescending(x => x.DataEvento);

            return await query.ToListAsync();
        }

        public async Task<List<Evento>> GetAllEventoAsyncByTema(string pTema, bool pIncludePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos.Include(x => x.Lotes).Include(x => x.RedesSociais);

            if(pIncludePalestrantes)
                query = query.Include(x => x.PalestranteEventos).ThenInclude(x => x.Palestrante);

            query = query.Where(x => x.Tema.ToLower().Contains(pTema.ToLower()));

            query = query.OrderByDescending(x => x.DataEvento);

            return await query.ToListAsync();
        }

        public async Task<Evento> GetEventoById(int pEventoId, bool pIncludePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos.Include(x => x.Lotes).Include(x => x.RedesSociais);

            if(pIncludePalestrantes)
                query = query.Include(x => x.PalestranteEventos).ThenInclude(x => x.Palestrante);

            query = query.Where(x => x.Id == pEventoId);

            query = query.OrderByDescending(x => x.DataEvento);

            return await query.FirstOrDefaultAsync();        
        }
        #endregion
        
       #region PALESTRANTE
        public async Task<List<Palestrante>> GetAllPalestranteAsyncByName(string pName, bool pIncludeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(x => x.RedesSociais);

            if(pIncludeEventos)
                query = query.Include(x => x.PalestranteEventos)
                        .ThenInclude(x => x.Evento);

            query = query.OrderBy(x => x.Nome)
                    .Where(x => x.Nome.ToLower().Contains(pName.ToLower()));

            return await query.ToListAsync();
        }

        public async Task<Palestrante> GetPalestranteById(int pId, bool pIncludeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(x => x.RedesSociais);

            if(pIncludeEventos)
                query = query.Include(x => x.PalestranteEventos)
                        .ThenInclude(x => x.Evento);

            query = query.OrderBy(x => x.Nome)
                    .Where(x => x.Id == pId);

            return await query.FirstOrDefaultAsync();          
        }
       #endregion
    }
}