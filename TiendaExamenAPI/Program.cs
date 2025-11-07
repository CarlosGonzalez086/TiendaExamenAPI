using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TiendaExamenAPI.Services.FuncionesGenerales;

var builder = WebApplication.CreateBuilder(args);
Clave clave = new Clave();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevPolicy", policy =>
    {
        policy
          .WithOrigins(
              "http://localhost:4200", 
              "http://localhost:5173"  
          )
          .AllowAnyHeader()
          .AllowAnyMethod();
    });
});

builder.Services.AddSingleton<ImageStorageService>(sp =>
{
    var env = sp.GetRequiredService<IWebHostEnvironment>();
    return new ImageStorageService(env.WebRootPath);
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<TiendaExamenAPI.Services.Utils.Context>();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Token: Bearer {your JWT token}.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clave.getClave())),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    }
    );

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("DevPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();