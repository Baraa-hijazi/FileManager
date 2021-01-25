using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MyFileSystem.Persistence;
using MyFileSystem.Persistence.UnitOfWork;
using MyFileSystem.Services.File;
using MyFileSystem.Services.Folder;
using MyFileSystem.Services.Interfaces.File;
using MyFileSystem.Services.Interfaces.Folder;
using Scrutor;
using System.IO;
using System.Text;

namespace MyFileSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        { Configuration = configuration; }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                     ValidateAudience = true,
                    ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = Configuration["Jwt:Issuer"],
                     ValidAudience = Configuration["Jwt:Issuer"],
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
               };
            });

            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            
            services.AddControllers();
            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            services.AddDbContext<FileSystemDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddAutoMapper(typeof(Startup));
            services.AddMvc().AddFluentValidation();
            services.AddScoped<IFileManager, OSFileManager>();
            services.AddSwaggerGen(options =>
            {
                services.AddSwaggerGen();
            });

            //services.AddScoped<IFolderService, FolderService>();
            //services.AddScoped<IFileService, FileService>();
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.Scan(scan => scan
            .FromCallingAssembly()
            .AddClasses()
            .AsMatchingInterface());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            { app.UseDeveloperExceptionPage(); }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers();});

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseAuthentication();
        }
    }
}
