using ClinicService.Data;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ClinicService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connection = builder.Configuration.GetConnectionString("DefaultConnection");

            // Add services to the container.

            builder.WebHost.ConfigureKestrel(op =>
            {
                op.Listen(IPAddress.Any, 5001, lisop =>
                {
                    lisop.Protocols = HttpProtocols.Http2;
                }); 
            });

            builder.Services.AddGrpc();

            builder.Services.AddControllers();

            builder.Services.AddDbContext<ClinicServiceDbContext>(options => options.UseSqlServer(connection));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGrpcService<ClinicService.Services.Imp.ClinicService>();
            //});

            app.MapGrpcService<ClinicService.Services.Imp.ClinicService>();

            app.Run();
        }
    }
}