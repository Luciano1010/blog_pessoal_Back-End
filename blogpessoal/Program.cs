
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

            // Add services to the container.

            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; // evita ficar no loop infinito
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore; // ela ignora os objetos null no json
                }); // ele fornece todos os recursos para cria��o das classes controladoras
                 




            // Conex�o com o banco de Dados
            // Conexão com o banco de Dados na nuvem e o else pra conexão local

            if (builder.Configuration["Enviroment:Start"] == "PROD")
            {
                builder.Configuration.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("secrets.json");
               
                var connecetionString = builder.Configuration
                .GetConnectionString("ProdConnection");
                builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(connecetionString));

            }
            else
            {
                var connecetionString = builder.Configuration
                .GetConnectionString("DefaultConnection");
                builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(connecetionString));
            }
            


            // registrar  a Valida��o das Entidades 
            builder.Services.AddTransient<IValidator<Postagem>, PostagemValidator>(); 
            builder.Services.AddTransient<IValidator<Tema>,TemaValidator>(); 
            builder.Services.AddTransient<IValidator<User>, UserValidator>();
           
            // registrar as classes de servi�o (service)
            builder.Services.AddScoped<IPostagemService, PostagemService>(); 
            builder.Services.AddScoped<ItemaService, TemasService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, AuthService>();


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var key = Encoding.UTF8.GetBytes(Settings.Secret);
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)

                };
            });




            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            
            // customizando o swagger
            builder.Services.AddSwaggerGen(options =>
            {
                // informa��es do projeto e do desenvolvedor
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Projeto Blog Pessoal",
                    Description = "Projeto Blog Pessoal - ASP.NET CORE 7.0",
                    Contact = new OpenApiContact
                    {
                        Name = "Luciano Sim�es",
                        Email = "luciano_lopesdealmeida@hotmail.com",
                        Url = new Uri("https://github.com/Luciano1010")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Github",
                        Url = new Uri("https://github.com/Luciano1010")
                    }

                });

                // configuara�ao de seguran�a no swwagger
                options.AddSecurityDefinition("JWT", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Digite um Token JWT valido",
                    Name = "Suthrization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                // adicionar a indica�ao de endpoint protegido
                options.OperationFilter<AuthResponsesOperationFilter>();

            });

            // adicionado o fluentvalidation no swagger
            builder.Services.AddFluentValidationRulesToSwagger();    


            // Configura��o do CORS onde fa�o a configura��o onde habilito que o front converse com o backend
            builder.Services.AddCors(options => 
            {
                options.AddPolicy(name:"MyPolicy",
                    policy => 
                    {
                        policy.AllowAnyOrigin() 
                              .AllowAnyMethod() 
                              .AllowAnyHeader(); 
                               
                    });
                
            }); 

            var app = builder.Build();
            
           

            // Criar o banco de dados e as tabelas automaticamente
            using(var scope = app.Services.CreateAsyncScope()) // CreateasyScope cria o banco de dados e as tabelas e consulta os contextos
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureCreated();
            }

            

           
               app.UseSwagger();


            // swagger como pagina inicial na nuvem
            if (app.Environment.IsProduction()) 
            {
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog Pessoal - v1");
                    options.RoutePrefix = string.Empty;
                });

            }
              
         

            
            app.UseCors("MyPolicy");// ele inicializa o CORS

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
   
    }
}
