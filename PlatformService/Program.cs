using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataServices.Grpc;
using PlatformService.SyncDataServices.Http;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        if (builder.Environment.IsProduction())
        {
            Console.WriteLine("--> Using SqlServer Db");
            builder.Services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn")));
        }
        else
        {
            Console.WriteLine("--> Using InMem Db");
            builder.Services.AddDbContext<AppDbContext>(opt =>
                opt.UseInMemoryDatabase("InMem"));
        }

        builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddControllers();
        builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
        builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
        builder.Services.AddGrpc();

        Console.WriteLine($"--> CommandService Endpoint {builder.Configuration.GetValue<string>("CommandService")}");

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //app.UseHttpsRedirection();

        app.MapControllers();
        app.MapGrpcService<GrpcPlatformService>();

        //optional
        app.MapGet("/protos/platforms/proto", async context =>
        {
            await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
        });

        Console.WriteLine(builder.Environment.IsProduction());
        PrepDb.PrepPopulation(app, builder.Environment.IsProduction());

        app.Run();
    }
}