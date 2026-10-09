using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Repositories;
using EventPlus.WebAPI.Services;
using EventPlus.WebAPI.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//Adicionando Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Insira um token válido para ter acesso aos endpoints da API"
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });

});

//Configuração do EFCore - Banco de Dados
builder.Services.AddDbContext<EventContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        //Corta o ciclo Usuario -> TipoUsuario -> Usuario -> ......
        //Colocando um null no ponto onde a referencia se repete
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

//Injeção de dependência
//AddScoped significa que uma instância nova é criada por requisição http 
//Isso garante que cada requisição tenha seu próprio contexto isolado
builder.Services.AddScoped<ITipoUsuario, TipoUsuarioRepository>();
builder.Services.AddScoped<ITipoEvento, TipoEventoRepository>();
builder.Services.AddScoped<IUsuario, UsuarioRepository>();
builder.Services.AddScoped<IInstituicao, InstituicaoRepository>();
builder.Services.AddScoped<IEvento, EventoRepository>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.AddScoped<IComentario, ComentarioRepository>();
builder.Services.AddScoped<IPresenca, PresencaRepository>();

//AUTENTICAÇÃO JWT
//Configura como a API vai validar os tokens recebidos nas requisições
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        //valida quem emitiu o token
        ValidateIssuer = true,
        ValidIssuer = "EventPlus.WebAPI",

        //valida para quem o token foi emitido
        ValidateAudience = true,
        ValidAudience = "EventPlus.WebAPI",

        //valida se o token ainda está dentro do prazo de validade
        ValidateLifetime = true,

        //define a tolerancia de clock entre servidores
        ClockSkew = TimeSpan.FromMinutes(5),

        //chave secreta utilizada para validar a assinatura do token
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        )
    };
});

//Configuração do cloudinary
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));

// --- Sightengine (plano Free, sem cartão) ---
builder.Services.Configure<SightengineSettings>(builder.Configuration.GetSection("Sightengine"));

builder.Services.AddHttpClient<IModerationService, SightengineModerationService>(client =>
{
    client.BaseAddress = new Uri("https://api.sightengine.com/1.0/");
});

//Registra o serviço de autorização (necessário para [Authorize] funcionar)
builder.Services.AddAuthorization();

//Registra o serviço de controllers(mapeia automaticamente os controllers da pasta /Controllers)
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Redireciona Http para Https automaticamente
app.UseHttpsRedirection();

//Ativa a autenticação
app.UseAuthentication();

//Ativa a autorização
app.UseAuthorization();

//Mapeia as rotas definidas nos Controllers com os atributos [Route]: api/[controller]
app.MapControllers();

app.Run();
