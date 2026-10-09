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
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuario _usuario;

        public UsuarioController(IUsuario usuario)
        {
            _usuario = usuario;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Listar() 
        {
            try
            {
                var usuarios = await _usuario.Listar();

                return Ok(usuarios);
            }
            catch (Exception erro) 
            {
                return BadRequest(erro.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Cadastrar([FromBody] UsuarioDTO dto) 
        {
            try
            {
                var usuario = new Usuario 
                {
                    Nome = dto.Nome,
                    Email = dto.Email,
                    Senha = dto.Senha, // obs: a criptografia ocorre dentro do repositorio
                    IdTipoUsuario = dto.IdTipoUsuario
                };

                await _usuario.Cadastrar(usuario);

                return StatusCode(201, usuario);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var usuarioBuscado = await
                _usuario.BuscarPorId(id);
            if (usuarioBuscado == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            return Ok(usuarioBuscado);
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] UsuarioDTO dto)
        {
                var usuario = new Usuario
                {
                    Nome = dto.Nome,
                    Email = dto.Email,
                    Senha = dto.Senha,
                    IdTipoUsuario = dto.IdTipoUsuario
                };

                await _usuario.Atualizar(id, usuario);

                return Ok(usuario);
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Deletar(Guid id)
        {
            await _usuario.Deletar(id);
            return NoContent();
        }
    }
}
