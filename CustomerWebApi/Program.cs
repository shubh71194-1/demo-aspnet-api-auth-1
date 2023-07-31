using CustomerWebApi.Context;
using Microsoft.EntityFrameworkCore;

namespace CustomerWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // DB context
            var dbHost = ".\\SQLEXPRESS";
            //var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            var dbName = "db_customer";
            //var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var dbPassword = "";
            //var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");

            var connection = $"Data Source={dbHost};Initial Catalog={dbName};Trusted_Connection=True;TrustServerCertificate=True;";
            builder.Services.AddDbContext<CustomerDbContext>(opt => opt.UseSqlServer(connection));

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}