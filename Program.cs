using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Paletitas.DataAccess;
using Paletitas.Models;
using Paletitas.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// **1. Configurar servicios en el contenedor**
// Leer configuración de JWT desde appsettings.json
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Registrar servicios de acceso a datos (DAL) y lógica de negocio (Services)
builder.Services.AddSingleton<UsuarioServices>(new UsuarioServices(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<UsuarioDAL>(new UsuarioDAL(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<ProveedorServices>(new ProveedorServices(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<ProveedorDAL>(new ProveedorDAL(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<ClienteServices>(new ClienteServices(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<ClienteDAL>(new ClienteDAL(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<CategoriaServices>(new CategoriaServices(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<CategoriaDAL>(new CategoriaDAL(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<VentasDAL>(new VentasDAL (builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<VentaServices>(new VentaServices(builder.Configuration.GetConnectionString("DefaultConnection")));



// Configurar autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
    });

// Configurar Swagger para soportar autenticación JWT
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor ingrese el token JWT con el prefijo 'Bearer'. Ejemplo: 'Bearer {token}'",
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
            Array.Empty<string>()
        }
    });
});

// Agregar servicios para controladores
builder.Services.AddControllers();

// **2. Construir la aplicación**
var app = builder.Build();

// **3. Configuración del middleware y pipeline**
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Habilitar autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Mapear controladores
app.MapControllers();

// **4. Ejecutar la aplicación**
app.Run();
