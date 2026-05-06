using BDD_LOGIN;
using LIB_DAL_LOGIN;
using LIB_GUITARE_CUSTOM;
using Scalar.AspNetCore;

namespace WEBAPI_GUITARE_CUSTOM;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddSingleton<BDD_GUITARE>();
        builder.Services.AddSingleton<BDD_LOGIN_CLASS>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
