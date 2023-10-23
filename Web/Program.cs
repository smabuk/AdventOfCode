using AdventOfCode.Services;
using AdventOfCode.SharedUI;
using AdventOfCode.Web;
using AdventOfCode.Web.Components;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddRazorComponents()
	.AddInteractiveServerComponents();

// Settings
builder.Services.Configure<AocSettings>(builder.Configuration.GetSection(nameof(AocSettings)));
builder.Services.AddMemoryCache();

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
	_ = app.UseDeveloperExceptionPage();
} else {
	_ = app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	_ = app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAntiforgery();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode()
	.AddAdditionalAssemblies(typeof(AdventOfCode.SharedUI.Component1).Assembly);

app.Run();
