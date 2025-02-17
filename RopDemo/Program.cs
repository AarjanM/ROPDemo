using RopDemo.Domain;
using RopDemo.Services.Data;
using Scalar.AspNetCore;

namespace RopDemo;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi();
        
        builder.Services.AddSingleton(TimeProvider.System);
        builder.Services.AddSingleton<IAccountRepository, InMemoryAccountRepository>();
        
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new AccountIdConverter());
        });
        
        builder.Services.AddEndpoints(typeof(Program).Assembly);

        var app = builder.Build();

        app.MapOpenApi();
        app.MapScalarApiReference();

        app.MapEndpoints();

        app.Run();
    }
}