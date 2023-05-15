using CRUDAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CRUDAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        
        public void ConfigureServices(IServiceCollection services)
        {
		   services.AddDbContext<Contexto>(options =>options.UseSqlite(Configuration.GetConnectionString("ConexaoDB")));		
		   services.AddEndpointsApiExplorer();
		   services.AddSwaggerGen();
		   services.AddAuthorization();
		   services.AddCors ();
		   services.AddHttpClient();

           services.AddControllers ();
		}

       
        public void Configure(WebApplication app, IWebHostEnvironment environment)
        {
            // Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			
			app.UseRouting ();
			
			app.UseCors(opcoes => opcoes.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());


			app.UseAuthorization();

			app.MapControllers();
        }
    }
}
