using DbUp;
using Microsoft.Extensions.Configuration;
using Simple.Data;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

if (!string.IsNullOrEmpty(environment))
{
    environment = $".{environment}";
}
var appSettings = $"appsettings{environment}.json";

var config = new ConfigurationBuilder()
  .SetBasePath(Directory.GetCurrentDirectory())
  .AddJsonFile(appSettings, optional: true, reloadOnChange: true)
  .AddEnvironmentVariables()
.Build();

var connStr = config.GetConnectionString(nameof(SimpleDataDbContext));

EnsureDatabase.For.SqlDatabase(connStr);

var dbUpgradeEngineBuilder = DeployChanges.To.SqlDatabase(connStr)
    .WithScriptsAndCodeEmbeddedInAssembly(typeof(Program).Assembly)
    .WithTransaction();

var dbUpgradeEngine = dbUpgradeEngineBuilder.Build();

if (dbUpgradeEngine.IsUpgradeRequired())
{
    _ = dbUpgradeEngine.PerformUpgrade();
}