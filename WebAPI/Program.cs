using ApplicationCore.Interfaces.Repository;
using BackendLab01;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure;
using Infrastructure.Memory.Repository;
using Infrastructure.MongoDB;
using Infrastructure.MongoDB.Entities;
using Infrastructure.Services;
using Microsoft.OpenApi.Models;
using WebAPI.Configuration;
using WebAPI.DTO;
using WebAPI.Validators;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSingleton<JwtSettings>();
            builder.Services.ConfigureIdentity();
            builder.Services.ConfigureJWT(new JwtSettings(builder.Configuration));
            builder.Services.ConfigureCors();
            builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.
              Enter 'Bearer' and then your token in the text input below.
              Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                       new OpenApiSecurityScheme
                       {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                       },
                       new List<string>()
                    }
                });

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Quiz API",
                });
            });
            //builder.Services.AddSingleton<IGenericRepository<Quiz, int>, MemoryGenericRepository<Quiz, int>>();
            //builder.Services.AddSingleton<IGenericRepository<QuizItem, int>, MemoryGenericRepository<QuizItem, int>>();
            //builder.Services.AddSingleton<IGenericRepository<QuizItemUserAnswer, string>, MemoryGenericRepository<QuizItemUserAnswer, string>>();


            //builder.Services.AddScoped<IQuizUserService, QuizUserService>();
            //builder.Services.AddScoped<IQuizAdminService, QuizAdminService>();
            builder.Services.AddTransient<IQuizUserService, QuizUserServiceEF>();
            builder.Services.AddTransient<IQuizAdminService, QuizAdminServiceEF>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddScoped<IValidator<QuizItem>, QuizItemValidator>();
            builder.Services.AddDbContext<QuizDbContext>();

            builder.Services.AddSingleton<QuizUserServiceMongoDB>();


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
            //app.Seed();
            app.AddUsers();
            app.Run();


        }
    }
}
public partial class Program
{

}
