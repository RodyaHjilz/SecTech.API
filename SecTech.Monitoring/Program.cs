using HealthChecks.UI.Core;
using HealthChecks.UI.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services
    .AddHealthChecksUI(setupSettings: setup =>
    {
        setup.SetEvaluationTimeInSeconds(5);
        setup.MaximumHistoryEntriesPerEndpoint(60);
    }).AddInMemoryStorage();
    


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.MapHealthChecksUI();
app.Run();


