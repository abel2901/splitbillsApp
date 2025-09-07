using Microsoft.EntityFrameworkCore;
using SplitBillsApi.Data;
using SplitBillsApi.Hubs;
using SplitBillsApi.Service;
using System.Runtime.Intrinsics.X86;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// --- Configurar porta din�mica (Railway) ---
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://*:{port}");

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<SaldoService>();
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    }); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

// --- CORS ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();

app.MapHub<GastosHub>("/gastosHub");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SplitBills API V1");
    });
}
else
{
    // Configura��es para produ��o
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SplitBills API V1");
    });
    app.UseExceptionHandler("/error"); // P�gina ou endpoint de erro
    app.UseHsts(); // Seguran�a HTTPS
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();


app.MapControllers();

app.Run();
