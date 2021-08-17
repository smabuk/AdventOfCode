using AdventOfCode.Services;
using AdventOfCode.SharedUI;
using AdventOfCode.Web;
using Microsoft.Extensions.Configuration;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Settings
builder.Services.Configure<AocSettings>(builder.Configuration.GetSection(nameof(AocSettings)));

builder.Services.AddHttpClient<AocHttpClient>(httpClient => {
	httpClient.DefaultRequestHeaders.Add("Cookie", $"session={builder.Configuration["AocSettings:HttpClientSettings:SessionCookie"]};");
});
builder.Services.AddHttpClient<GithubHttpClient>();

builder.Services.AddScoped<FileSystemInputData>();
builder.Services.AddScoped<SessionState>();
builder.Services.AddScoped<AocJsInterop>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
	app.UseDeveloperExceptionPage();
} else {
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();


app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
