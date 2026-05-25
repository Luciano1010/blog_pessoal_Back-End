using blogpessoal.Configuration;
using blogpessoal.Data;
using blogpessoal.Model;
using blogpessoal.Model.Security;
using blogpessoal.Model.Security.Implements;
using blogpessoal.Service;
using blogpessoal.Service.Implements;
using blogpessoal.Validator;
using FluentValidation;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace blogpessoal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Controllers
            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling =
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore;

                    options.SerializerSettings.NullValueHandling =
                        Newtonsoft.Json.NullValueHandling.Ignore;
                });

            // Porta do Render
            var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
            builder.WebHost.UseUrls($"http://*:{port}");

            // Conexão PostgreSQL
            var connectionString = builder.Configuration
                .GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            // Validators
            builder.Services.AddTransient<IValidator<Postagem>, PostagemValidator>();
            builder.Services.AddTransient<IValidator<Tema>, TemaValidator>();
            builder.Services.AddTransient<IValidator<User>, UserValidator>();

            // Services
            builder.Services.AddScoped<IPostagemService, PostagemService>();
            builder.Services.AddScoped<ItemaService, TemasService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            // JWT
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                var key = Encoding.UTF8.GetBytes(Settings.Secret);

                options.SaveToken = true;

                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(key)
                    };
            });

            // Swagger
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Projeto Blog Pessoal",
                    Description = "Projeto Blog Pessoal - ASP.NET CORE 7.0",

                    Contact = new OpenApiContact
                    {
                        Name = "Luciano Simões",
                        Email = "luciano_lopesdealmeida@hotmail.com",
                        Url = new Uri("https://github.com/Luciano1010")
                    },

                    License = new OpenApiLicense
                    {
                        Name = "Github",
                        Url = new Uri("https://github.com/Luciano1010")
                    }
                });

                options.AddSecurityDefinition("JWT",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Digite um Token JWT valido",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "Bearer"
                    });

                options.OperationFilter<AuthResponsesOperationFilter>();
            });

            builder.Services.AddFluentValidationRulesToSwagger();

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "MyPolicy",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
            });

            var app = builder.Build();

            // Criar banco automaticamente
            using (var scope = app.Services.CreateScope())
            {
                var dbContext =
                    scope.ServiceProvider.GetRequiredService<AppDbContext>();

                dbContext.Database.Migrate();
            }

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(
                    "/swagger/v1/swagger.json",
                    "Blog Pessoal - v1");

                options.RoutePrefix = string.Empty;
            });

            app.UseCors("MyPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
} 