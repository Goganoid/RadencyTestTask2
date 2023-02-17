using API.Extensions;
using Application;
using Application.Books;
using Application.Books.DTO;
using Application.Books.DTO.Requests;
using Application.Books.Validation;
using Application.Core;
using FluentValidation;
using FluentValidation.AspNetCore;
using Persistence;
using MediatR;
var builder = WebApplication.CreateBuilder(args);

if(builder.Configuration["SecretKey"]==null) throw new ArgumentException("SecretKey is not set");

// Add services to the container.
builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);
builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(ApplicationAssembly));
// builder.Services.AddScoped<IValidator<SaveBookRequestDTO>, SaveBookRequestValidator>();
// builder.Services.AddFluentValidationAutoValidation();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>();
builder.Services.AddTransient<BookService>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DataContext>();
    Seed.SeedData(context);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.ConfigureExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
