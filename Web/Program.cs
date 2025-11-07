using AdventOfCode.Services;
using AdventOfCode.SharedUI;
using AdventOfCode.Web;
using AdventOfCode.Web.Components;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddRazorComponents()
	.AddInteractiveServerComponents(options =>
	{
		options.DetailedErrors = builder.Environment.IsDevelopment();
		options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(3);
		options.JSInteropDefaultCallTimeout = TimeSpan.FromMinutes(1);
		options.MaxBufferedUnacknowledgedRenderBatches = 10;
	});

// Settings
builder.Services.Configure<AocSettings>(builder.Configuration.GetSection(nameof(AocSettings)));
builder.Services.AddMemoryCache();

builder.Services.AddHttpClient<AocHttpClient>(httpClient => httpClient.DefaultRequestHeaders.Add("Cookie", $"session={builder.Configuration["AocSettings:HttpClientSettings:SessionCookie"]};"));
builder.Services.AddHttpClient<GithubHttpClient>();

// Add output caching for improved performance
builder.Services.AddOutputCache();

// Add response compression for static assets
builder.Services.AddResponseCompression(options => options.EnableForHttps = true);

builder.Services.AddScoped<FileSystemInputData>();
builder.Services.AddScoped<SessionState>();
builder.Services.AddScoped<AocJsInterop>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
	_ = app.UseDeveloperExceptionPage();
} else {
	_ = app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	_ = app.UseHsts();
}

app.UseResponseCompression();
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.MapStaticAssets();

app.UseAntiforgery();

app.UseOutputCache();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode()
	.AddAdditionalAssemblies(typeof(AdventOfCode.SharedUI.Component1).Assembly);

app.Run();
