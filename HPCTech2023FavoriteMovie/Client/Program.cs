using HPCTech2023FavoriteMovie.Client;
using HPCTech2023FavoriteMovie.Client.HttpRepository;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("HPCTech2023FavoriteMovie.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("HPCTech2023FavoriteMovie.ServerAPI"));
builder.Services.AddScoped<IUserMoviesHttpRepository, UserMoviesHttpRespository>();
builder.Services.AddApiAuthorization();
builder.Services.AddSyncfusionBlazor();
await builder.Build().RunAsync();
