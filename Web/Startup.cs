using System;
using System.Net.Http;

using AdventOfCode.Services;
using AdventOfCode.SharedUI;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AdventOfCode.Web {
	public class Startup {
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services) {
			services.AddRazorPages();
			services.AddServerSideBlazor();

			// Settings
			services.Configure<AocSettings>(Configuration.GetSection(nameof(AocSettings)));

			services.AddHttpClient<AocHttpClient>(httpClient => {
				httpClient.DefaultRequestHeaders.Add("Cookie", $"session={Configuration["AocSettings:HttpClientSettings:SessionCookie"]};");
			});
			services.AddHttpClient<GithubHttpClient>();

			services.AddScoped<FileSystemInputData>();
			services.AddScoped<SessionState>();
			services.AddScoped<AocJsInterop>();

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			} else {
				app.UseExceptionHandler(" / Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseEndpoints(endpoints => {
				endpoints.MapBlazorHub();
				endpoints.MapFallbackToPage("/_Host");
			});
		}
	}
}
