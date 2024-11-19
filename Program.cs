using SISWIN.DataAccess;
using SISWIN.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar la cadena de conexi�n
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registrar CompraService y CompraDAL con la cadena de conexi�n
builder.Services.AddScoped(_ => new CompraService(connectionString));
builder.Services.AddScoped(_ => new CompraDAL(connectionString));

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
