using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Context;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class DCandidatesController : ControllerBase
    {
        private readonly appDbContext _dbContext;

        public DCandidatesController(appDbContext context)
        {
            _dbContext = context;
        }

        //Recupera um usuário no banco de dados.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DCandidates>>> GetDCandidates()
        {
            return await _dbContext.DCandidates.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DCandidates>> GetDCandidates(int id)
        {
            var dCandidate = await _dbContext.DCandidates.FindAsync(id); 

            if(dCandidate == null)
                return NotFound();

            return Ok(dCandidate);  
        }

        //Recebe como parâmetro do objeto e atualiza-o no banco de dados.
        [HttpPut("{id}")]
        public async Task<ActionResult> PutDCandidate(int id, DCandidates dCandidates)
        {
            dCandidates.id = id; 
            _dbContext.Entry(dCandidates).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DCandidatesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //Recebe o objeto como parâmetro e insere no banco 
        [HttpPost]
        public async Task<ActionResult<DCandidates>> PostDCandidate(DCandidates dCandidates)
        {
            _dbContext.DCandidates.Add(dCandidates);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("Novo candidato", new {id = dCandidates.id}, dCandidates);
        }

        //Busca o usuário pelo id e o remove do banco de dados
        [HttpDelete("{id}")]
        public async Task<ActionResult<DCandidates>> DeleteCandidate(int id)
        {
            var dCandidate = await _dbContext.DCandidates.FindAsync(id);

            if(dCandidate == null)
            {
                return NotFound();
            }

            _dbContext.DCandidates.Remove(dCandidate);
            await _dbContext.SaveChangesAsync();

            return dCandidate;
        }

        //Verifica se existe um candidato e retorna um valor booleano
        private bool DCandidatesExists(int id)
        {
            return _dbContext.DCandidates.Any(a => a.id == id);
        }
    }
}
