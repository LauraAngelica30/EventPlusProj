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
    public class TipoUsuarioController : ControllerBase
    {
        private readonly ITipoUsuario _tipoUsuario;

        public TipoUsuarioController(ITipoUsuario tipoUsuario)
        {
            _tipoUsuario = tipoUsuario;
        }
        /// <summary>
        /// Listas todos os perfis de usuário
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var tipos = await _tipoUsuario.Listar();

                return Ok(tipos);
            }
            catch (Exception erro)
            {

                return BadRequest(erro.Message);
            }
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var tipoUsuarioBuscado = await
                _tipoUsuario.BuscarPorId(id);
            if (tipoUsuarioBuscado == null)
            {
                return NotFound("Tipo de usuário não encontrado.");
            }

            return Ok(tipoUsuarioBuscado);
        }

        /// <summary>
        /// Cadastra um novo perfil de usuário
        /// </summary>
        /// <param name="tipoUsuario">Perfil do usuário a ser cadastrado</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Cadastrar([FromBody] TipoUsuarioDTO dto)
        {
            var tipoUsuario = new TipoUsuario
            {
                Titulo = dto.Titulo
            };

            await _tipoUsuario.Cadastrar(tipoUsuario);

            return StatusCode(201, tipoUsuario);
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] TipoUsuarioDTO dto)
        {
            var tipoUsuario = new TipoUsuario
            {
                Titulo = dto.Titulo
            };

            await _tipoUsuario.Atualizar(id, tipoUsuario);

            return Ok(tipoUsuario);
        }

        /// <summary>
        /// Remove um perfil de usuário pelo id
        /// </summary>
        /// <param name="id">Id do perfil a ser removido</param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Deletar(Guid id)
        {
            await _tipoUsuario.Deletar(id);
            return NoContent();
        }
    }
}
