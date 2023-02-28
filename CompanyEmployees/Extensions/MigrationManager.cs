using CompanyEmployees.Migrations;
using Contracts;
using FluentMigrator.Runner;

namespace CompanyEmployees.Extensions;

public static class MigrationManager
{
    public static WebApplication MigrateDatabase(this WebApplication app, ILoggerManager logger)
    {
        using(var scope = app.Services.CreateScope())
        {
            var databaseService = scope.ServiceProvider.GetRequiredService<Database>();

            var migrationService = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

            try
            {
                databaseService.CreateDatabase("CompanyEmployeeDapper");

                migrationService.ListMigrations();

                migrationService.MigrateUp();
            }
            catch(Exception ex)
            {
                logger.LogError($"Exception occured during the database creation: {ex}");
                throw;
            }
        }
        return app;
    }
}
