using Contracts;
using Dapper;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Repository.Queries;
using Shared.DataTransferObjects;
using System.Data;

namespace Repository;

public class CompanyRepository : ICompanyRepository
{
    private readonly DapperContext _context;

    public CompanyRepository(DapperContext context) => _context = context;

    public async Task<IEnumerable<CompanyDto>> GetAllCompanies()
    {
        var query = CompanyQuery.SelectCompanyQuery;

        using(var connection = _context.CreateConnection())
        {
            var companies = await connection.QueryAsync<CompanyDto>(query);

            return companies.ToList();
        }
    }

    public async Task<CompanyDto> GetCompany(Guid id)
    {
        var query = CompanyQuery.SelectCompanyByIdQuery;

        using(var connection = _context.CreateConnection())
        {
            var company = await connection.QuerySingleOrDefaultAsync<CompanyDto>(query, new {companyId = id});

            return company;
        }
    }

    public async Task<IEnumerable<CompanyWithEmployeesDto>> GetCompaniesWithEmployees()
    {
        var query = CompanyQuery.SelectCompaniesWithEmployeesQuery;

        using(var connection = _context.CreateConnection())
        {
            var companyDict = new Dictionary<Guid, CompanyWithEmployeesDto>();

            var companies = await connection.QueryAsync<CompanyWithEmployeesDto, EmployeeDto, CompanyWithEmployeesDto>
                (query, (company, employee) =>
            {
                if (!companyDict.TryGetValue(company.CompanyId, out var currentCompany))
                {
                    currentCompany = company;
                    companyDict.Add(currentCompany.CompanyId, currentCompany);
                }

                currentCompany.Employees.Add(employee);

                return currentCompany;
            }, splitOn: "CompanyId, EmployeeId");

            return companies.Distinct().ToList();
        };
    }

    public async Task<CompanyDto> CreateCompany(CompanyForCreationDto company)
    {
        var query = CompanyQuery.InsertCompanyQuery;

        var param = new DynamicParameters(company);

        using(var conection = _context.CreateConnection())
        {
            var id = await conection.QuerySingleAsync<Guid>(query, param);

            return new CompanyDto(id, company.Name, string.Join(", ", company.Address, company.Country));
        }
    }

    public async Task<CompanyDto> CreateCompanyWithEmployees(CompanyForCreationDto company)
    {
        var query = CompanyQuery.InsertCompanyQuery;

        var param = new DynamicParameters(company);

        using(var connection = _context.CreateConnection())
        {
            connection.Open();

            using(var trans = connection.BeginTransaction())
            {
                var id = await connection.QuerySingleAsync<Guid>(query, param, transaction: trans);

                var queryEmp = EmployeeQuery.InsertEmployeeNoOutputQuery;

                var empList = company.Employees!.Select(e => new {e.Name, e.Age, e.Position, id});

                await connection.ExecuteAsync(queryEmp, empList, transaction: trans);

                trans.Commit();

                return new CompanyDto(id, company.Name, string.Join(", ", company.Address, company.Country));
            }
        }
    }

    public async Task<IEnumerable<CompanyDto>> GetByIds(IEnumerable<Guid> ids)
    {
        var query = CompanyQuery.SelectCompaniesForMultipleIdsQuery;

        using (var connection = _context.CreateConnection())
        {
            var companies = await connection.QueryAsync<CompanyDto>(query, new {ids});

            return companies.ToList();
        }
    }

    public async Task<IEnumerable<CompanyDto>> CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companies)
    {
        var companyList = new List<CompanyDto>();

        using(var connection = _context.CreateConnection()) 
        {
            connection.Open();

            using(var trans = connection.BeginTransaction())
            {
                foreach(var company in companies) 
                {
                    var query = CompanyQuery.InsertCompanyQuery;

                    var param = new DynamicParameters(company);

                    var id = await connection.QuerySingleAsync<Guid>(query, param, transaction: trans);

                    companyList.Add(new CompanyDto(id, company.Name, string.Join(", ", company.Address, company.Country)));
                }
                
                trans.Commit();

                return companyList;
            }
        }
    }

    public async Task DeleteCompany(Guid id)
    {
        var query = CompanyQuery.DeleteCompanyQuery;

        using(var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(query, new {id});
        }
    }

    public async Task UpsertCompany(Guid id, CompanyForUpdateDto company)
    {
        var query = CompanyQuery.UpdateCompanyQuery;

        var param = new DynamicParameters(company);

        param.Add("id", id, DbType.Guid);

        using(var connection = _context.CreateConnection())
        {
            connection.Open();

            using(var trans = connection.BeginTransaction())
            {
                await connection.ExecuteAsync(query, param, transaction: trans);

                var queryEmp = EmployeeQuery.InsertEmployeeNoOutputQuery;

                var empList = company.Employees.Select(e => new { e.Name, e.Age, e.Position, id });

                await connection.ExecuteAsync(queryEmp, empList, transaction: trans);

                trans.Commit();
            }
        }
    }

    public async Task<CompanyDto> GetCompanyByEmployeeId(Guid employeeId)
    {
        var procName = "ShowCompanyByEmployeeId";

        using(var connection = _context.CreateConnection())
        {
            var company = await connection.QueryFirstOrDefaultAsync<CompanyDto>(procName, new { id = employeeId }, commandType: CommandType.StoredProcedure);

            return company;
        }
    }
}
