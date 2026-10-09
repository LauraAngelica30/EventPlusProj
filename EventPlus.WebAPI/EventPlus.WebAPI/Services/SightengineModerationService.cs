using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Utils;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace EventPlus.WebAPI.Services
{
    public class SightengineModerationService : IModerationService
    {
        //Para sexta - implementar aqui a lógica de moderar texto 

        //Pendências:
        // 1. Cadastro na plataforma
        // 2. Usar Secrets das credenciais (ApiUser e ApiSecret)
        // 3. Cadastrar uma presença para o usuário (Cadastrar)
        // 4. Cadastrar um comentário (Cadastrar)
        // 5. Terminar todos os CRUDS

        private readonly HttpClient _http;
        private readonly string _apiUser;
        private readonly string _apiSecret;

        //Acima de limiar a categoria é considerada violação
        private const double Limiar = 0.5;

        public SightengineModerationService(HttpClient http, IOptions<SightengineSettings> options)
        {
            _http = http;
            _apiUser = options.Value.ApiUser;
            _apiSecret = options.Value.ApiSecret;
        }

        public async Task<bool> ModerarTexto(string texto)
        {
            //FormUrlEncodedContent: definir que as credencias vão no corpo da requisição
            var form = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["text"] = texto,
                ["lang"] = "pt",
                ["mode"] = "ml",
                ["api_user"] = _apiUser,
                ["api_secret"] = _apiSecret
            });

            //"text/check.json": endpoint da api externa
            //form: dados que serão enviados junto a requisição(texto a ser moderado etc...)
            var resposta = await _http.PostAsync("text/check.json", form);

            //Verifica se a resposta(http post) foi bem sucedida
            //Se o status for um erro, lança um exception
            resposta.EnsureSuccessStatusCode();

            using var doc = JsonDocument.Parse(
                 await resposta.Content.ReadAsStringAsync()
            );

            //Obtém o elemento raiz do json
            //Acesso as propriedades do json(array e etc...)
            var root =  doc.RootElement;

            //Obtém a propriedade status do json e verifica se o valor é diferente de "sucess"
            if (root.GetProperty("status").GetString() != "success")
            {
                //Tenta obter a propriedade "error" e dentro dela a mensagem de erro
                //Caso não retorne, utilizamos uma mensagem de erro desconhecido
                var msg = root.TryGetProperty("error", out var err) && err.TryGetProperty("message", out var m) ? m.GetString() : "erro desconhecido";

                throw new Exception($"Sightengine: {msg}");
            }

            var classes = root.GetProperty("moderation_classes");

            foreach (var prop in classes.EnumerateObject())
            {
                if (prop.Name == "available") continue;

                if (prop.Value.ValueKind == JsonValueKind.Number && prop.Value.GetDouble() >= Limiar) return true;//reprovado, passou do limiar
            }

            return false;//aprovado, não passou do limiar
        }
    }
}

