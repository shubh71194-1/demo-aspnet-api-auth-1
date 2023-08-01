using JwtAuthManager;
using Microsoft.EntityFrameworkCore;
using ProductWebApi.Context;

namespace ProductWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // DB context
            var dbHost = "localhost";
            //var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            var dbName = "db_product";
            //var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var dbPassword = "root";
            //var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");

            var connection = $"server={dbHost};port=3306;database={dbName};user=root;password={dbPassword};";
            builder.Services.AddDbContext<ProductDbContext>(opt => opt.UseMySQL(connection));

            builder.Services.AddControllers();

            // Authentication Service
            builder.Services.AddCustomJwtAuthentication();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}