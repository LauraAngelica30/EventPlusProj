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
    public class InstituicaoController : ControllerBase
    {
        private readonly IInstituicao _instituicao;

        public InstituicaoController(IInstituicao instituicao)
        {
            _instituicao = instituicao;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var instituicoes = await _instituicao.Listar();

                return Ok(instituicoes);
            }
            catch (Exception erro)
            {
                return BadRequest(erro.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Cadastrar([FromBody] InstituicaoDTO dto)
        {
            var instituicao = new Instituicao
            {
                NomeFantasia = dto.NomeFantasia,
                Cnpj = dto.CNPJ,
                Endereco = dto.Endereco

            };

            await _instituicao.Cadastrar(instituicao);

            return StatusCode(201, instituicao);
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var InstituicaoBuscada = await _instituicao.BuscarPorId(id);

            if (InstituicaoBuscada == null)
            {
                return NotFound("Instituição não encontrada");
            }

            return Ok(InstituicaoBuscada);
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] InstituicaoDTO dto)
        {
            var intituicao = new Instituicao
            {
                NomeFantasia = dto.NomeFantasia,
                Cnpj = dto.CNPJ,
                Endereco = dto.Endereco
            };

            await _instituicao.Atualizar(id, intituicao);

            return Ok(intituicao);
        }

        [HttpDelete("{id:guid}")]
        [Authorize]

        public async Task<IActionResult> Deletar(Guid id)
        {
            await _instituicao.Deletar(id);
            return NoContent();
        }
    }
}
