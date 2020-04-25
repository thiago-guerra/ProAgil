using System.Collections.Generic;
using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public interface IProAgilRepository
    {
        //GERAL 
        void Add<T>(T entity) where T : class; 
        void Update<T>(T entity) where T : class; 
        void Delete<T>(T entity) where T : class; 
        Task<bool> SaveChangesAsync();

        //EVENTOS
        Task<List<Evento>> GetAllEventoAsyncByTema(string pTema, bool pIncludePalestrantes = false);
        Task<List<Evento>> GetAllEventoAsync(bool pIncludePalestrantes = false);
        Task<Evento> GetEventoById(int pEventoId, bool pIncludePalestrantes = false);

        //PALESTRANTE
        Task<List<Palestrante>> GetAllPalestranteAsyncByName(string pName, bool pIncludeEventos = false);
        Task<Palestrante> GetPalestranteById(int pId ,bool pIncludeEventos = false);

        

    }
}