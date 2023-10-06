  
using blogpessoal.Data;
using blogpessoal.Model;
using blogpessoal.Model.Security;
using blogpessoal.Model.Security.Implements;
using blogpessoal.Service;
using blogpessoal.Service.Implements;
using blogpessoal.Validator;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
                }); // ele fornece todos os recursos para criação das classes controladoras




            // Conexão com o banco de Dados
            var connecetionString = builder.Configuration
                .GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connecetionString));


            // registrar  a Validação das Entidades 
            builder.Services.AddTransient<IValidator<Postagem>, PostagemValidator>(); 
            builder.Services.AddTransient<IValidator<Tema>,TemaValidator>(); 
            builder.Services.AddTransient<IValidator<User>, UserValidator>();
           
            // registrar as classes de serviço (service)
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
            builder.Services.AddSwaggerGen();

            // Configuração do CORS onde faço a configuração onde habilito que o front converse com o backend
            builder.Services.AddCors(options => 
            {
                options.AddPolicy(name: "My policy",
                    policy => 
                    {
                        policy.AllowAnyOrigin() 
                              .AllowAnyMethod() 
                              .AllowAnyHeader(); 
                    
                    });
                
            }); ;

            var app = builder.Build();

            // Criar o banco de dados e as tabelas automaticamente
            using(var scope = app.Services.CreateAsyncScope()) // CreateasyScope cria o banco de dados e as tabelas e consulta os contextos
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureCreated();
            }

            

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            
            app.UseCors("Mypolicy");// ele inicializa o CORS

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
   
    }
}