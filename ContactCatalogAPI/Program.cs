using ContactCatalogAPI.Models;
using ContactCatalogAPI.Repositories;
using ContactCatalogAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<Dictionary<int, Contact>>();
builder.Services.AddSingleton<HashSet<string>>();
builder.Services.AddSingleton<IContactRepository, ContactRepository>();
builder.Services.AddSingleton<ContactService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();