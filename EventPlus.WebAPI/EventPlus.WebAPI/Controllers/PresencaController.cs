using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PresencaController : ControllerBase
    {
        private readonly IPresenca _presenca;

        public PresencaController(IPresenca presenca)
        {
            _presenca = presenca;
        }

        [HttpPost]
        public async Task<IActionResult> Inscrever([FromBody] PresencaDTO dto)
        {
            try
            {
                var presenca = new Presenca
                {
                    IdEvento = dto.IdEvento,
                    IdUsuario = dto.IdUsuario,
                    Situacao = dto.situacao
                };

                await _presenca.Increver(presenca);

                return StatusCode(201, presenca);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var presencas = await _presenca.Listar();

                return Ok(presencas);
            }
            catch (Exception erro)
            {

                return BadRequest(erro.Message);
            }
        }

        [HttpGet("{idUsuario:guid}/ListarMinhas")]
        public async Task<IActionResult> ListarMinhas(Guid idUsuario)
        {
            try
            {
                var minhaspresencas = await _presenca.ListarMinhas(idUsuario);

                return Ok(minhaspresencas);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> BucarPorId(Guid id)
        {
            var presencabuscada = await _presenca.BuscarPorId(id);

            if (presencabuscada == null)
            {
                return NotFound("Presenças não encontradas");
            }

            return Ok(presencabuscada);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            await _presenca.Deletar(id);
            return NoContent();
     
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> AtualizarSituacao(Guid id, [FromBody] PresencaDTO dto)
        {

            try
            {
            var presenca = new Presenca
            {
                Situacao = dto.situacao
            };
            await _presenca.AtualizarSituacao(id, presenca);
            return NoContent();
            }
            catch (Exception)
            {

                throw;
            }

        }






    }
}
