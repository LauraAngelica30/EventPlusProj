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
    [Authorize]
    public class ComentarioController : ControllerBase
    {
        private readonly IComentario _comentario;

        private readonly IModerationService _moderationService;

        public ComentarioController(IComentario comentario, IModerationService moderationService)
        {
            _comentario = comentario;
            _moderationService = moderationService;
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] ComentarioDTO dto)
        {
            try
            {
                bool reprovado = await _moderationService.ModerarTexto(dto.Descricao);

                var comentario = new Comentario
                {
                    DataComentario = DateTime.Now,
                    Descricao = dto.Descricao,
                    IdEvento = dto.IdEvento,
                    IdUsuario = dto.IdUsuario,
                    Exibe = !reprovado
                };

                await _comentario.Cadastrar(comentario);

                return StatusCode(201, comentario);
            }
            catch (Exception e)
            { 
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var comentarios = await _comentario.Listar();

                return Ok(comentarios);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet("{idEvento:guid}/ListarPorEvento")]
        public async Task<IActionResult> ListarPorEvento(Guid idEvento)                              
        {
            try
            {
                var eventos = await _comentario.ListarPorEvento(idEvento);

                return Ok(eventos);

            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var comentariobuscado = await _comentario.BuscarPorId(id);

            if (comentariobuscado == null)
            {
                return NotFound("Comentario não encontrado");
            }

            return Ok(comentariobuscado);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            await _comentario.Deletar(id);
            return NoContent();
        }
    } 
}
