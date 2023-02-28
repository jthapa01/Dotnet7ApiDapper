using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service.Contracts;

public interface IEmployeeService
{
    Task<(IEnumerable<EmployeeDto> employees, MetaData metaData)> GetEmployees(Guid companyId, EmployeeParameters employee);

    Task<EmployeeDto> GetEmployee(Guid companyId, Guid id);

    Task<EmployeeDto> CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeDto);

    Task DeleteEmployeeForCompany(Guid companyId, Guid employeeId);

    Task UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employee);
}
