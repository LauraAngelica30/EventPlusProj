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
    public class EventoController : ControllerBase
    {
        private readonly IEvento _evento;

        private readonly ICloudinaryService _cloudinaryService;

        public EventoController(IEvento evento, ICloudinaryService cloudinaryService)
        {
            _evento = evento;

            _cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var eventos = await _evento.Listar();

                return Ok(eventos);
            }
            catch (Exception erro)
            {

                return BadRequest(erro.Message);
            }
        }

        [HttpGet("{id:guid}/ListarPorInscrito")]
        public async Task<IActionResult> ListarPorInscrito(Guid id)
        {
            try
            {
                var inscritos = await _evento.ListarPorInscrito(id);

                return Ok(inscritos);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet("{idInstituicao:guid}/ListarPorInstituicao")]
        public async Task<IActionResult> ListarPorInstituicao(Guid idInstituicao)
        {
            try
            {
                var intituicoes = await _evento.ListarPorInstituicao(idInstituicao);

                return Ok(intituicoes);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet("{idProximos:guid}/ListarProximosEventos")]
        public async Task<IActionResult> ListarProximosEventos(Guid idProximos)
        {
            try
            {
                var proximosEventos = await _evento.ListarProximosEventos(idProximos);

                return Ok(proximosEventos);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }


        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Cadastrar([FromForm] EventoDTO dto)
        {
            try
            {
                string? imagemUrl = null;

                if (dto.ArquivoImagem is not null)
                {
                    imagemUrl = await _cloudinaryService.UploadImagem(dto.ArquivoImagem);
                }

                var evento = new Evento
                {
                    NomeEvento = dto.NomeEvento,
                    DataEvento = dto.DataEvento,
                    Descricao = dto.Descricao,
                    ImagemUrl = imagemUrl, // Url vindo do Cloudinary (ou null)
                    IdTipoEvento = dto.IdTipoEvento,
                    IdInstituicao = dto.IdInstituicao
                };

                    await _evento.Cadastrar(evento);

                    return StatusCode(201, evento);
            }
            catch (Exception error)
            {

                return BadRequest(error.Message);
            }

        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var eventoBuscado = await _evento.BuscarPorId(id);

            if (eventoBuscado == null)
            {
                return NotFound("Evento não encontrado");
            }

            return Ok(eventoBuscado);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] EventoDTO dto)
        {
            var evento = new Evento
            {
                NomeEvento = dto.NomeEvento,
                DataEvento = dto.DataEvento,
                Descricao = dto.Descricao,
                ImagemUrl = dto.ImagemUrl,
                IdTipoEvento = dto.IdTipoEvento,
                IdInstituicao = dto.IdInstituicao
            };

            await _evento.Atualizar(id, evento);

            return Ok(evento);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            await _evento.Deletar(id);

            return NoContent();
        }
    }
}
