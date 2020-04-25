using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PalestranteController : ControllerBase
    {
         private readonly IProAgilRepository _repository ;

        public PalestranteController(IProAgilRepository repository)
        {
          _repository = repository;
        }

        [HttpGet]
        [Route("{Id}")]
        public async Task<IActionResult> Get(int Id)
        {
          try
          {
             var result = await _repository.GetPalestranteById(Id);

             if(result == null)
                return NotFound();

                return Ok(result);
          }
          catch (System.Exception)
          {
            return this.StatusCode(StatusCodes.Status500InternalServerError,  "Banco de dados falhou");
          }         
        }

        [HttpGet]
        [Route("getByName")]
        public async Task<IActionResult> Get(string Name)
        {
          try
          {
             var result = await _repository.GetAllPalestranteAsyncByName(Name);

             if(result == null)
                return NotFound();

                return Ok(result);
          }
          catch (System.Exception)
          {
            return this.StatusCode(StatusCodes.Status500InternalServerError,  "Banco de dados falhou");
          }         
        }

        [HttpPost]
        public async Task<IActionResult> Post(Palestrante model)
        {
            try
            {
                 _repository.Add(model);

                 if(await _repository.SaveChangesAsync())
                    return Created($"api/Palestrante/{model.Id}", model);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,  "Banco de dados falhou");
            }   

            return BadRequest();
        }

    }
}