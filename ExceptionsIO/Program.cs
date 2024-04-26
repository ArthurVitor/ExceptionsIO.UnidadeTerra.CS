using ExceptionsIO.Infrastructure;
using ExceptionsIO.Interfaces;
using ExceptionsIO.Services;

namespace ExceptionsIO;

public class Program
{
    public static void Main(string[] args)
    {
        var appBuilder = WebApplication.CreateBuilder(args);

        appBuilder.Services.AddControllers();

        appBuilder.Services.AddEndpointsApiExplorer();
        appBuilder.Services.AddSwaggerGen();
        appBuilder.Services.AddSingleton<IFileService, FileService>();

        appBuilder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        appBuilder.Services.AddProblemDetails();
        
        var app = appBuilder.Build();

        app.UseHttpsRedirection();

        
        app.UseExceptionHandler();
        
        app.UseSwagger();
        app.UseSwaggerUI();
        
        app.UseDeveloperExceptionPage();

        app.MapControllers();

        app.Run();
    }
}