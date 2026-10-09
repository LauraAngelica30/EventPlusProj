using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Utils;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace EventPlus.WebAPI.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> options)
        {
            //Desempacota as configurações (CloudName, ApiKey e ApiSecret)
            var credenciais = options.Value;

            //Account = "carteira" com as três credenciais que autenticam na conta do Cloudinary
            var account = new Account(credenciais.CloudName, credenciais.ApiKey, credenciais.ApiSecret);

            //Cria o cliente de fato, já autenticado com as credencias  
            _cloudinary = new Cloudinary(account);

            //Definindo que as urls geradas venha como https 
            _cloudinary.Api.Secure = true;
        }

        public async Task<string> UploadImagem(IFormFile arquivo)
        {
            //Abre um fluxo de leitura do arquivo
            // using: garante que o stream será fechado após o uso(libera a memória mesmo se der erro)
            using var stream = arquivo.OpenReadStream();

            //Monta os parâmetros do upload
            var uloadParams = new ImageUploadParams
            {
                //O arquivo em si: nome original + o fluxo de bytes a enviar
                File = new FileDescription(arquivo.FileName, stream),

                //Pasta de destino dentro do Cloudinary
                Folder = "eventplus/eventos"
            };

            //Envia a imagem para o Cloudinary e aguarda a resposta com os dados do upload
            var resultado = await _cloudinary.UploadAsync(uloadParams);

            //Retorna só o que interessa para a aplicação (URL segura da imagem)
            //Que depois será salva no campo ImagemUrl(em Eventos)
            return resultado.SecureUrl.AbsoluteUri;
        }
    }
}
