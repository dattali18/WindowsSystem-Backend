﻿using Microsoft.EntityFrameworkCore;
using WindowsSystem_Backend.Controllers;
using WindowsSystem_Backend.DAL;

namespace WindowsSystem_Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // WARNING:
            // Connecting to DB
            // FIXME: Change the connection string to your own

            // INFO: for macos use this
            builder.Services.AddDbContext<DataContext>(options =>
               options.UseSqlite(builder.Configuration.GetConnectionString("localDb")));

            // INFO: for windows use this
            // builder.Services.AddDbContext<DataContext>(options =>
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("AppContext")));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.ToString());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            };

            //app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
