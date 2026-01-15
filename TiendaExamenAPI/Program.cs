using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TiendaExamenAPI.DbData;
using TiendaExamenAPI.DbData.Repository;
using TiendaExamenAPI.DbData.Repository.ArticuloCliente;
using TiendaExamenAPI.DbData.Repository.Articulos;
using TiendaExamenAPI.DbData.Repository.Cliente;
using TiendaExamenAPI.DbData.Repository.Tienda;
using TiendaExamenAPI.Services;
using TiendaExamenAPI.Services.ArticuloCliente;
using TiendaExamenAPI.Services.Articulos;
using TiendaExamenAPI.Services.Cliente;
using TiendaExamenAPI.Services.FuncionesGenerales;
using TiendaExamenAPI.Services.Tienda;
using TiendaExamenAPI.Services.Utils;

var builder = WebApplication.CreateBuilder(args);


Clave clave = new Clave();


builder.Services.AddDbContext<MiTiendaDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);


builder.Services.AddScoped<ArticulosServicios>();
builder.Services.AddScoped<ClienteServicio>();
builder.Services.AddScoped<ClienteAccesoServicio>();
builder.Services.AddScoped<TiendaServicios>();
builder.Services.AddScoped<ArticuloClienteServicios>();

builder.Services.AddScoped<ArticulosRepositorio>();
builder.Services.AddScoped<ClienteRepositorio>();
builder.Services.AddScoped<TiendaRepositorio>();
builder.Services.AddScoped<ArticuloClienteRepositorio>();
builder.Services.AddScoped<ServiciosGenerales>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<AdminRepository>();
builder.Services.AddScoped<ServiciosGenerales>();


builder.Services.AddScoped<Context>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<ImageStorageService>(sp =>
{
    var env = sp.GetRequiredService<IWebHostEnvironment>();
    return new ImageStorageService(env.WebRootPath);
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MiTienda API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Ejemplo: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(clave.getClave())
            ),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("DevPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
