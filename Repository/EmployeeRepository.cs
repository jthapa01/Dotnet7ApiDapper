using Contracts;
using Dapper;
using Entities;
using Repository.Queries;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Data;

namespace Repository;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly DapperContext _context;
    
    public EmployeeRepository(DapperContext context) => _context = context;

    public async Task<PagedList<EmployeeDto>> GetEmployees(Guid companyId, EmployeeParameters employeeParameters)
    {
        var skip = (employeeParameters.PageNumber - 1) * employeeParameters.PageSize;

        var searchTerm = !string.IsNullOrEmpty(employeeParameters.SearchTerm) ? 
            employeeParameters.SearchTerm.Trim().ToLower() : string.Empty;

        var orderBy = OrderQueryBuilder.CreateOrderQuery<EmployeeDto>(employeeParameters.OrderBy!, 'e');

        var query = EmployeeQuery.SelectEmployeesQuery(orderBy);

        var param = new DynamicParameters();
        param.Add("companyId", companyId, DbType.Guid);
        param.Add("skip", skip, DbType.Int32);
        param.Add("take", employeeParameters.PageSize, DbType.Int32);
        param.Add("minAge", employeeParameters.MinAge, DbType.Int32);
        param.Add("maxAge", employeeParameters.MaxAge, DbType.Int32);
        param.Add("searchTerm", searchTerm, DbType.String);

        using(var connection = _context.CreateConnection())

        using(var multi = await connection.QueryMultipleAsync(query, param))
        {
            var count = await multi.ReadSingleAsync<int>();

            var employees = (await multi.ReadAsync<EmployeeDto>()).ToList();

            return new PagedList<EmployeeDto>(employees, count, employeeParameters.PageNumber, employeeParameters.PageSize);
        }
    }

    public async Task<EmployeeDto> GetEmployee(Guid companyId, Guid id)
    {
        var query = EmployeeQuery.SelectEmployeeByIdAndCompanyIdQuery;

        using (var connection = _context.CreateConnection())
        {
            var param = new { companyId, id };

            var employee = await connection.QuerySingleOrDefaultAsync<EmployeeDto>(query, param);

            return employee;
        }
    }

    public async Task<EmployeeDto> CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeDto)
    {
        var query = EmployeeQuery.InsertEmployeeWithOutputQuery;

        var param = new DynamicParameters(employeeDto);

        param.Add("id", companyId, DbType.Guid);

        using(var connection = _context.CreateConnection())
        {
            var id = await connection.QuerySingleAsync<Guid>(query, param);

            return new EmployeeDto(id, employeeDto.Name, employeeDto.Age, employeeDto.Position);
        }
    }

    public async Task DeleteEmployee(Guid employeeId)
    {
        var query = EmployeeQuery.DeleteEmployeeQuery;

        using(var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(query, new { employeeId });
        }
    }

    public async Task UpdateEmployee(Guid employeeId, EmployeeForUpdateDto employee)
    {
        var query = EmployeeQuery.UpdateEmployeeQuery;

        var param = new DynamicParameters(employee);

        param.Add("employeeId", employeeId, DbType.Guid);

        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(query, param);
        }
    }
}