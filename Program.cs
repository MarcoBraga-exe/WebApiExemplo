// Ponto de composição da aplicação: registra dependências e o pipeline HTTP.
using WebApiExemplo.Application.Interfaces;
using WebApiExemplo.Application.Services;
using WebApiExemplo.Infrastructure.Messaging;
using WebApiExemplo.Infrastructure.Repositories;
using WebApiExemplo.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Controllers expõem a API; Swagger documenta e permite testar seus endpoints.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<AzureQueueOptions>(builder.Configuration.GetSection(AzureQueueOptions.SectionName));
// Repositórios em memória são singletons para manter os dados durante a execução da API.
builder.Services.AddSingleton<IProprietarioRepository, InMemoryProprietarioRepository>();
builder.Services.AddSingleton<IVeiculoRepository, InMemoryVeiculoRepository>();
builder.Services.AddScoped<IProprietarioService, ProprietarioService>();
builder.Services.AddScoped<IVeiculoService, VeiculoService>();
builder.Services.AddSingleton<IQueuePublisher, AzureQueuePublisher>();
builder.Services.AddHostedService<AzureQueueWorker>();

var app = builder.Build();

// O middleware converte exceções de domínio em respostas HTTP padronizadas.
app.UseMiddleware<ExceptionMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
