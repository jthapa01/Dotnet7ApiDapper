namespace Repository.Queries;

public static class EmployeeQuery
{
    public static string SelectEmployeesQuery(string orderBy) =>
        @$"SELECT COUNT(e.EmployeeId)
          FROM Employees AS e
          WHERE e.CompanyId = @companyId AND (e.Age >= @minAge AND e.Age <= @maxAge)
          AND ((@searchTerm LIKE N'') OR (CHARINDEX(@searchTerm, LOWER(e.[Name])) > 0));

          SELECT e.EmployeeId, e.[Name], e.[Age], e.Position 
          FROM Employees AS e WHERE e.CompanyId = @companyId AND (e.Age >= @minAge AND e.Age <= @maxAge) 
          AND ((@searchTerm LIKE N'') OR (CHARINDEX(@searchTerm, LOWER(e.[Name])) > 0)) 
          ORDER BY {orderBy} 
          OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";

    public const string SelectEmployeeByIdAndCompanyIdQuery =
        @"SELECT EmployeeId, [Name], Age, Position
          FROM Employees
          WHERE EmployeeId = @id AND CompanyId = @companyId";
    
    public const string InsertEmployeeWithOutputQuery =
        @"INSERT INTO Employees (EmployeeId, Name, Age, Position, CompanyId)
          OUTPUT inserted.EmployeeId
          VALUES (default, @name, @age, @position, @id)";

    public const string InsertEmployeeNoOutputQuery =
        @"INSERT INTO Employees (EmployeeId, Name, Age, Position, CompanyId)
          VALUES (default, @name, @age, @position, @id)";

    public const string DeleteEmployeeQuery =
        @"DELETE FROM Employees
          WHERE EmployeeId = @employeeId";

    public const string UpdateEmployeeQuery =
        @"UPDATE Employees
          SET [Name] = @name, Age = @age, Position = @position
          WHERE EmployeeId = @employeeId";
}
