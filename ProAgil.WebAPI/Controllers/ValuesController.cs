﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProAgil.Repository;
using Microsoft.AspNetCore.Authorization;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ProAgilContext _dataContext ;
        public ValuesController(ProAgilContext ctx)
        {
           _dataContext = ctx;
        }

        // GET api/values
        [HttpGet]
        [AllowAnonymous]
        public async Task< IActionResult> Get()
        {
            try
            {
                var results = await _dataContext.Eventos.ToListAsync();
                return Ok(results); 
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,  "Banco de dados falhou");
            } 
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task< IActionResult> Get(int id)
        {
            try
            {
                var result = await _dataContext.Eventos.FirstOrDefaultAsync(x => x.Id == id);
 
                return Ok(result);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
