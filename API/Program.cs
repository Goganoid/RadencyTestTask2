using API.Extensions;
using API.Middlewares;
using Application;
using Application.Books;
using Application.Core;
using FluentValidation;
using Persistence;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

if (builder.Configuration["SecretKey"] == null) throw new ArgumentException("SecretKey is not set");

// Add services to the container.
builder.Services.AddTransient<ExceptionMiddleware>();
builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);
builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(ApplicationAssembly));
builder.Services.AddDbContext<DataContext>();
builder.Services.AddTransient<BookService>();
// Add HTTP logging
const HttpLoggingFields requestLogs = HttpLoggingFields.RequestMethod | HttpLoggingFields.RequestHeaders |
                                      HttpLoggingFields.RequestQuery | HttpLoggingFields.RequestBody;
const HttpLoggingFields responseLogs = HttpLoggingFields.ResponseHeaders | HttpLoggingFields.ResponseBody |
                                       HttpLoggingFields.ResponseStatusCode;
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = requestLogs | responseLogs;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
// seed database
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

app.UseHttpLogging();
app.ConfigureExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();