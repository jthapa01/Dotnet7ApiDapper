using Shared.DataTransferObjects;

namespace Service.Contracts;

public interface ICompanyService
{
    Task<IEnumerable<CompanyDto>> GetAllCompanies();

    Task<CompanyDto> GetCompany(Guid id);

    Task<IEnumerable<CompanyWithEmployeesDto>> GetAllCompaniesWithEmployees();

    Task<CompanyDto> CreateCompany(CompanyForCreationDto company);

    Task<IEnumerable<CompanyDto>> GetByIds(IEnumerable<Guid> ids);

    Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection);

    Task DeleteCompany(Guid companyId);

    Task UpdateCompany(Guid companyId, CompanyForUpdateDto companyForUpdate);

    Task<CompanyDto> GetCompanyByEmployeeId(Guid employeeId);
}
