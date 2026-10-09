namespace EventPlus.WebAPI.Utils
{
    public class CloudinarySettings
    {
        //Nome da conta no Cloudinary
        public string CloudName { get; set; } = string.Empty;

        //Chave pública de identificação da API
        public string ApiKey { get; set; } = string.Empty;

        //Chave secreta que assina/autentica as requisições
        public string ApiSecret { get; set; } = string.Empty;

    }
}
