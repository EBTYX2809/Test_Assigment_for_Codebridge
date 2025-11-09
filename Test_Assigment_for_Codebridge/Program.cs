using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
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
        app.MapControllers();

        app.Run();
    }
}
