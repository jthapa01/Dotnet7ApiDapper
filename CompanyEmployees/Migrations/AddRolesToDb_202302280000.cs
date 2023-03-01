using AspNetCore.Identity.Dapper.Models;
using FluentMigrator;

namespace CompanyEmployees.Migrations;

[Migration(202302280000)]
public class AddRolesToDb_202302280000 : Migration
{
    public override void Down()
    {
        Delete.FromTable("AspNetRoles")
            .Row(new ApplicationRole
            {
                Name = "Manager",
                NormalizedName = "MANAGER"
            })
            .Row(new ApplicationRole
            {
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            });
    }

    public override void Up()
    {
        Insert.IntoTable("AspNetRoles")
            .Row(new ApplicationRole
            {
                Name = "Manager",
                NormalizedName = "MANAGER"
            })
            .Row(new ApplicationRole
            {
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            });
    }
}
