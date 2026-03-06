using BookingSystem.API.Middleware;
using BookingSystem.Application.DependencyInjection;
using BookingSystem.Persistence.DependencyInjection;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(
    opt =>
    {
        opt.Filters.Add<BookingSystem.API.Filters.WrapResponseFilter>();
    });
builder.Services.AddScoped<BookingSystem.API.Filters.WrapResponseFilter>();
// აქ ვრთავთ Application layer-ს (MediatR + Validators)
builder.Services.AddApplication();

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();

//სვაგერი
//builder.Services.AddSwaggerGen(options =>
//{
//    options.OperationFilter<BookingSystem.API.Swagger.ApiResponseOperationFilter>();
//});
builder.Services.AddSwaggerGen();

//ვამატებთ მიდდლვეარს
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

// გავთიშოთი cors
// AllowAnyOrigin() და AllowCredentials() ერთად ნელზიაა CORS სტანდარტით. 
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", p =>
        p.AllowAnyOrigin()
         .AllowAnyHeader()
         .AllowAnyMethod()
    );
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// მიდლვეარი უნდა იყოს სვაგერის მერე და მაპ კონტროლერამდე, არ გაატოკო :დ
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Build-ის მერე:
app.UseCors("DevCors");
app.MapControllers();
app.Run();
