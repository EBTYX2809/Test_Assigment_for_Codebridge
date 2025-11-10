using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading.RateLimiting;
using Test_Assigment_for_Codebridge.DataBase;
using Test_Assigment_for_Codebridge.Middleware;
using Test_Assigment_for_Codebridge.Services;

namespace Test_Assigment_for_Codebridge;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // DI
        builder.Services.AddDbContext<AppDbContext>(options =>      
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddScoped<DogService>();
        
        builder.Services.AddControllers();
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        builder.Services.AddEndpointsApiExplorer();        
        builder.Services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });
        var rateLimiterOptions = builder.Configuration.GetSection("RateLimiterOptions");
        builder.Services.AddRateLimiter(limiterOptions =>
        {
            limiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            limiterOptions.AddFixedWindowLimiter("fixed", options =>
            {
                options.Window = TimeSpan.Parse(rateLimiterOptions["Window"]);
                options.PermitLimit = int.Parse(rateLimiterOptions["PermitLimit"]);
                options.QueueLimit = int.Parse(rateLimiterOptions["QueueLimit"]);
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            }); 
        });

        var app = builder.Build();

        // Development config
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            DataSeeder.ClearDb(app.Services).Wait();
            DataSeeder.Seed(app.Services).Wait();
        }

        // Middleware
        app.UseRouting();
        app.UseMiddleware<ExceptionsHandler>();
        app.UseHttpsRedirection();
        app.UseRateLimiter();
        app.MapControllers().RequireRateLimiting("fixed");

        app.Run();
    }
}
