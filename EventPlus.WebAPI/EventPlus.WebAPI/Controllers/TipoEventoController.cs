using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoEventoController : ControllerBase
    {
        private readonly ITipoEvento _tipoEvento;

        public TipoEventoController(ITipoEvento tipoEvento)
        {
            _tipoEvento = tipoEvento;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListarTipoEvento() // IActionResult: retorna uma ação
        {
            try // O que esperamos que dê certo
            {
                var tiposEventos = await _tipoEvento.ListarTipoEvento();

                return Ok(tiposEventos);
            }
            catch (Exception erro) // Se der errado, não vai quebrar nosso código, vai ter um tratamento de erro
            {

                return BadRequest(erro.Message);
            }
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> BuscarTipoEventoPorId(Guid id) 
        {
            var tipoEventoBuscado = await
                _tipoEvento.BuscarTipoEventoPorId(id);
            if (tipoEventoBuscado == null)
            {
                return NotFound("Tipo de evento não encontrado.");
            }

            return Ok(tipoEventoBuscado);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CadastrarTipoEvento([FromBody] TipoEventoDTO dtoEvento)  
        {
            var tipoEvento = new TipoEvento
            {
                TituloTipoEvento = dtoEvento.TituloTipoEvento
            };

            await _tipoEvento.CadastrarTipoEvento(tipoEvento);

            return StatusCode(201, tipoEvento);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dtoEvento"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> AtualizarTipoEvento(Guid id, [FromBody] TipoEventoDTO dtoEvento)
        {
            var tipoEvento = new TipoEvento
            {
                TituloTipoEvento = dtoEvento.TituloTipoEvento
            };

            await _tipoEvento.AtualizarTipoEvento(id, tipoEvento);

            return Ok(tipoEvento);
        }

        /// <summary>
        /// Remove uma categoria de evento
        /// </summary>
        /// <param name="id">Id do objeto a ser excluído</param>
        /// <returns>Status Code NoContent se der certo e 400 caso haja exceção</returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletarTipoEvento(Guid id)
        {
            await _tipoEvento.DeletarTipoEvento(id);
            return NoContent();
        }
    }
}
