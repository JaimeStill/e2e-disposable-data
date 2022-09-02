using System.Text.Json;
using System.Text.Json.Serialization;
using Brainstorm.Rig.Hubs;
using Brainstorm.Rig.Services;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddCors(options =>
        options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins(GetConfigArray("CorsOrigins"))
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithExposedHeaders(
                    "Content-Disposition",
                    "Access-Control-Allow-Origin"
                );
        })
    );

builder
    .Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddSingleton<ApiRig>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseRouting();
app.MapControllers();
app.MapHubs();
app.Run();

string[] GetConfigArray(string section) =>
    builder.Configuration
        .GetSection(section)
        .GetChildren()
        .Select(x => x.Value)
        .ToArray();