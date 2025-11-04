
// http://localhost:5263/swagger/index.html

using MyApp.Namespace;
using LaunchDarkly.Sdk.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Store client reference for cleanup
LdClient? ldClient = null;

builder.Services.AddSingleton( _=> {
    var sdkKey = builder.Configuration["LaunchDarkly:SdkKey"];
    Console.WriteLine("** creating LdClient **");
    if (string.IsNullOrEmpty(sdkKey))
    {
        Console.WriteLine("*** Please set LAUNCHDARKLY_SDK_KEY environment variable to your LaunchDarkly SDK key first\n");
        Environment.Exit(1);
    }
    ldClient = LaunchDarklyService.CreateLdClient(sdkKey);
    return ldClient;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// LaunchDarkly client cleanup on application shutdown
app.Lifetime.ApplicationStopped.Register(() => {
    Console.WriteLine("** Shutting down LaunchDarkly client **");
    ldClient?.Dispose(); // Properly closes connections and flushes pending events
    Console.WriteLine("** LaunchDarkly client shutdown complete **");
});

app.Run();
