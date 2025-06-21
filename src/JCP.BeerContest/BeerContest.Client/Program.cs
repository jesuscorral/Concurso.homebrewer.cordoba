using BeerContest.Client;
using BeerContest.Client.Services;
using Blazored.Toast;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure base address for API calls
var apiBaseAddress = builder.Configuration["ApiBaseAddress"] ?? "https://localhost:7001/";
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseAddress) });

// Register API service
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<BeerService>();

// Add Blazored Toast
builder.Services.AddBlazoredToast();

// Add Blazorise
builder.Services
    .AddBlazorise()
    .AddBootstrapProviders()
    .AddFontAwesomeIcons();

// Add authentication state provider (for now, we'll create a simple one)
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
