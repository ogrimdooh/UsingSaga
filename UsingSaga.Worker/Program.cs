using UsingSaga.Infra.Bus;
using UsingSaga.Domain;
using Serilog;

namespace UsingSaga.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var _env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local";

            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("appsettings.json", false, true);
            builder.Configuration.AddJsonFile($"appsettings.{_env}.json", false, true);
            var config = builder.Configuration.AddEnvironmentVariables().Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level} {CorrelationToken} {Message} {Exeption} {NewLine}")
                .CreateLogger();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddListenerBus(config);

            builder.Services.AddDomainServices();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
