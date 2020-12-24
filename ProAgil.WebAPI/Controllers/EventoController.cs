using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;
using ProAgil.WebAPI.Dtos;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IProAgilRepository _repository ;

        private readonly IMapper _mapper ;

        public EventoController(IProAgilRepository repository, IMapper mapper)
        {

          _repository = repository;
          _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await _repository.GetAllEventoAsync(true);
                var results = _mapper.Map<List<EventoDto>>(eventos);

                return Ok(results); 
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,  "Banco de dados falhou: "+ ex.Message);
            } 
        }

        [HttpGet("{EventoId}")]
        public async Task<IActionResult> Get(int EventoId)
        {
            try
            {
                var evento = await _repository.GetEventoById(EventoId, true);
                var result = _mapper.Map<EventoDto>(evento);
                return Ok(result);
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpGet("getByTema/{tema}")]
        public async Task<IActionResult> Get(string tema)
        {
            try
            {
                var eventos = await _repository.GetAllEventoAsyncByTema(tema, true);
                var results = _mapper.Map<List<EventoDto>>(eventos);
 
                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou: "+ ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        { 
            try
            {
                var evento = _mapper.Map<Evento>(model);
              _repository.Add(evento);

              if(await _repository.SaveChangesAsync())
                return Created( $"/api/evento/{evento.Id}", _mapper.Map<EventoDto>(evento));

            }
            catch (System.Exception ex)
            {
              return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou: "+ $"{ex.Message}");
            }   

            return BadRequest();
        }

        [HttpPut("{EventoId}")]
        public async Task<IActionResult> Put(int EventoId, EventoDto model)
        {
            try
            {
                var evento = await _repository.GetEventoById(EventoId, false);
                if(evento == null) return NotFound();

              _mapper.Map(model, evento);
              _repository.Update(evento);

              if(await _repository.SaveChangesAsync())
                return Created( $"/api/evento/{model.Id}", _mapper.Map<EventoDto>(evento));

            }
            catch (System.Exception)
            {
              return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }   

            return BadRequest();
        }

        [HttpDelete("{EventoId}")]
        public async Task<IActionResult> Delete(int EventoId)
        {
            try
            {
                var evento = _repository.GetEventoById(EventoId, false);
                if(evento.Result == null) return NotFound();

              _repository.Delete(evento.Result);

              if(await _repository.SaveChangesAsync())
                return Ok();
            }
            catch
            {
              return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }   

            return BadRequest();
        }

         [HttpPost("upload")]
        public async Task<IActionResult> upload()
        {
            try
            {
               var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources","Imagens");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if(file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                    var fullPath = Path.Combine(pathToSave, fileName.Replace("\"", "").Trim());

                    FileStream fileStream = new FileStream(fullPath, FileMode.Create);
                     file.CopyTo(fileStream);
 
                }
                return  Ok(); 
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,  "Banco de dados falhou: "+ ex.Message);
            } 
        }
    }
}