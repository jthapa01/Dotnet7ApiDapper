using Entities.Models;
using FluentMigrator;

namespace CompanyEmployees.Migrations;

[Migration(202302200000)]
public class InitialSeed_202302200000 : Migration
{
    public override void Down()
    {
        Delete.FromTable("Employees")
            .Row(new Employee
            {
                EmployeeId = new Guid("80abbca8-664d-4b20-b5de-024705497d4a"),
                Name = "Sam Raiden",
                Age = 26,
                Position = "Software developer",
                CompanyId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870")
            })
            .Row(new Employee 
            {
                EmployeeId = new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"),
                Name = "Jana McLeaf",
                Age = 30,
                Position = "Software developer",
                CompanyId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870")
            })
            .Row(new Employee 
            { 
                EmployeeId = new Guid("021ca3c1-0deb-4afd-ae94-2159a8479811"),
                Name = "Kane Miller", 
                Age = 35, 
                Position = "Administrator", 
                CompanyId = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3") 
            });
        Delete.FromTable("Companies")
            .Row(new Company
            {
                CompanyId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                Address = "10213 Main St. Gwynn Oak, MD 21234",
                Country = "USA",
                Name = "IT Solutions Ltd"
            })
            .Row(new Company
            {
                CompanyId = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"),
                Name = "Admin_Solutions Ltd",
                Address = "312 Forest Avenue, BF 923",
                Country = "USA"
            });
    }

    public override void Up()
    {
        Insert.IntoTable("Companies")
            .Row(new Company
            {
                CompanyId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                Address = "10213 Main St. Gwynn Oak, MD 21234",
                Country = "USA",
                Name = "IT Solutions Ltd"
            })
            .Row(new Company
            {
                CompanyId = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"),
                Name = "Admin_Solutions Ltd",
                Address = "312 Forest Avenue, BF 923",
                Country = "USA"
            });

        Insert.IntoTable("Employees")
            .Row(new Employee
            {
                EmployeeId = new Guid("80abbca8-664d-4b20-b5de-024705497d4a"),
                Name = "Sam Raiden",
                Age = 26,
                Position = "Software developer",
                CompanyId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870")
            })
            .Row(new Employee
            {
                EmployeeId = new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"),
                Name = "Jana McLeaf",
                Age = 30,
                Position = "Software developer",
                CompanyId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870")
            })
            .Row(new Employee
            {
                EmployeeId = new Guid("021ca3c1-0deb-4afd-ae94-2159a8479811"),
                Name = "Kane Miller",
                Age = 35,
                Position = "Administrator",
                CompanyId = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3")
            });
    }
}
