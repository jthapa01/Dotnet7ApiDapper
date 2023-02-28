using Contracts;
using Entities.Exceptions;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

public sealed class CompanyService : ICompanyService
{
    private readonly IRepositoryManager _repository;

    private readonly ILoggerManager _logger;

    public CompanyService(IRepositoryManager repository, ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<CompanyDto>> GetAllCompanies()
    {
        var companies = await _repository.Company.GetAllCompanies();

        return companies;
    }

    public async Task<CompanyDto> GetCompany(Guid id)
    {
        var company = await _repository.Company.GetCompany(id);

        if (company is null)
            throw new CompanyNotFoundException(id);

        return company;
    }

    public async Task<IEnumerable<CompanyWithEmployeesDto>> GetAllCompaniesWithEmployees()
    {
        var companies = await _repository.Company.GetCompaniesWithEmployees();

        return companies;
    }

    public async Task<CompanyDto> CreateCompany(CompanyForCreationDto company)
    {
        if (company.Employees is not null && company.Employees.Any())
            return await _repository.Company.CreateCompanyWithEmployees(company);
        else
            return await _repository.Company.CreateCompany(company);
    }

    public async Task<IEnumerable<CompanyDto>> GetByIds(IEnumerable<Guid> ids)
    {
        if (ids is null)
            throw new IdParameterBadRequestException();

        var companies = await _repository.Company.GetByIds(ids);

        if (ids.Count() != companies.Count())
            throw new CollectionByIdsBadRequestException();

        return companies;
    }

    public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection)
    {
        if (companyCollection is null)
            throw new CompanyCollectionBadRequestException();

        var companies = await _repository.Company.CreateCompanyCollection(companyCollection);

        var ids = string.Join(",", companies.Select(c => c.CompanyId));

        return (companies, ids);
    }

    public async Task DeleteCompany(Guid companyId)
    {
        var company = await _repository.Company.GetCompany(companyId);

        if (company is null)
            throw new CompanyNotFoundException(companyId);

        await _repository.Company.DeleteCompany(companyId);
    }

    public async Task UpdateCompany(Guid companyId, CompanyForUpdateDto companyForUpdate)
    {
        var companyEntity = await _repository.Company.GetCompany(companyId);

        if(companyEntity is null)
            throw new CompanyNotFoundException(companyId);

        if(companyForUpdate.Employees is not null && companyForUpdate.Employees.Any())
        {
            await _repository.Company.UpsertCompany(companyId, companyForUpdate);
        }
    }

    public async Task<CompanyDto> GetCompanyByEmployeeId(Guid employeeId)
    {
        var company = await _repository.Company.GetCompanyByEmployeeId(employeeId);

        return company;
    }
}
