using SISWIN.DataAccess;
using SISWIN.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar la cadena de conexión
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registrar ProductoService y ProductoDAL con la cadena de conexión
builder.Services.AddScoped(_ => new ProductoService(connectionString));
builder.Services.AddScoped(_ => new ProductoDAL(connectionString));

// Add CORS policy (if needed)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

// Use HTTPS redirection for secure communication
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
