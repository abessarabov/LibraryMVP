
using Library.Domain.Cache;
using Library.Domain.ConnectionFactory;
using Library.Domain.Repositories;
using Library.Server.Background;
using Library.Server.Cache.Redis;
using Library.Server.Connections;
using Library.Server.External.NoSql.ElasticSearch;
using Library.Server.Repositories;
using Library.Server.Services;
using Library.Web.Middleware;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();

builder.Services.AddSingleton<ISectionElasticReadRepository, SectionElasticRepository>();
builder.Services.AddSingleton<ISectionElasticWriteRepository, SectionElasticRepository>();

builder.Services.AddSingleton<ISectionRepository, SectionRepository>();
builder.Services.AddSingleton<IArticleEventRepository, ArticleEventRepository>();
builder.Services.AddSingleton<ISectionWriteService, SectionWriteService>();

builder.Services.AddScoped<INormalizationService, ArticleRepository>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<ISectionReadService, SectionReadService>();

builder.Services.AddSingleton<IConnectionFactory<SqlConnection>, SqlConnectionFactory>();

builder.Services.Configure<RedisOptions>(builder.Configuration.GetSection("Redis"));
builder.Services.AddSingleton<ICache, RedisCache>();
builder.Services.AddSingleton<IElasticClientFactory, ElasticClientFactory>();


builder.Services.AddHostedService<ArticleEventWorker>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ResponseInterceptor>();

app.MapControllers();

app.Run();
