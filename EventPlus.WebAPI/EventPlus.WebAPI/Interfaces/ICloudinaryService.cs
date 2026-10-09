namespace EventPlus.WebAPI.Interfaces
{
    public interface ICloudinaryService
    {
        //IFormFile: arquivo binário que chega no multipart/form-data
        //É a imagem
        Task<string> UploadImagem(IFormFile arquivo);
    }
}
