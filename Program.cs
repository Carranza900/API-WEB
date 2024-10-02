using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProyectoIV.DataAccess;
using ProyectoIV.Services;
using Integrador.Services;
using System.Text;
using Integrador.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios
builder.Services.AddScoped<ClienteDAL>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<ProveedorService>();
builder.Services.AddScoped<UsuarioDAL>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<ProductoDAL>();
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<CategoriaDAL>();
builder.Services.AddScoped<CategoriaService>();


builder.Services.AddControllers();

// Configuración autenticación JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "AuthService",
        ValidAudience = "MyApi",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("S3gur1dad#2024!@#SecretSuperSegura12345"))
    };
});

// Configuración Swagger para aceptar JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor ingrese el token JWT con el prefijo 'Bearer '",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configuración de middlewares en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseHttpsRedirection();


// Habilita autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Mapeo de controladores
app.MapControllers();

app.Run();
















/*using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ProyectoIV.DataAccess;
using ProyectoIV.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Agregar controladores
builder.Services.AddControllers();

// Inyeta servicios y capas de datos
builder.Services.AddScoped<UsuarioDAL>();
builder.Services.AddScoped<UsuarioService>();

builder.Services.AddScoped<ProductoDAL>();
builder.Services.AddScoped<ProductoService>();

builder.Services.AddScoped<CategoriaDAL>();
builder.Services.AddScoped<CategoriaService>(); 

// Configurar autenticación JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "AuthService", // Emisor del token
        ValidAudience = "MyApi", // Audiencia del token
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("S3gur1dad#2024!@#SecretSuperSegura12345"))
    };
});

// Agregar Swagger para la documentación de API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuración de middlewares en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Habilitar autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Mapeo de controladores
app.MapControllers();

app.Run();*/
