using API.DialogRecommendation.Interfaces;
using API.DialogRecommendation.Services;
using API.DialogRecommendation.Services.Decorator;
using CAG.Utils.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCAG(builder.Configuration);
builder.Services
    .AddScoped<IDialogRecommender, DialogRecommender>()
    .Decorate<IDialogRecommender, DialogRecommenderCacheDecorator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
