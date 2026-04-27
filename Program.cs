using FrontBlazor_AppiGenericaCsharp.Components;
using FrontBlazor_AppiGenericaCsharp.Services;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios de Blazor Server
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configurar HttpClient para conectarse a la API
// La URL base apunta a la API ApiGenericaCsharp que corre en el puerto 5034
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5034")
});

// Registrar los servicios de la API
// AuthService se registra PRIMERO porque ApiService lo necesita para el token JWT
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<FrontBlazor_AppiGenericaCsharp.Services.ApiService>(sp =>
    new ApiService(sp.GetRequiredService<HttpClient>(), sp.GetRequiredService<AuthService>()));
builder.Services.AddScoped<FrontBlazor_AppiGenericaCsharp.Services.SpService>();

var app = builder.Build();

// Configurar el pipeline HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
