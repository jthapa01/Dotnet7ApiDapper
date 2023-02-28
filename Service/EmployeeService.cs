using Contracts;
using Entities.Exceptions;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service;

public sealed class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repository;

    private readonly ILoggerManager _logger;

    public EmployeeService(IRepositoryManager repository, ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<(IEnumerable<EmployeeDto> employees, MetaData metaData)> GetEmployees(Guid companyId, EmployeeParameters employeeParameters)
    {
        if (!employeeParameters.ValidAgeRange)
            throw new MaxAgeRangeBadRequestException();

        var company = await _repository.Company.GetCompany(companyId);

        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employeesWithMetaData = await _repository.Employee.GetEmployees(companyId, employeeParameters);

        var employees = employeesWithMetaData.Select(e => new EmployeeDto(e.EmployeeId, e.Name, e.Age, e.Position));
        
        return (employees: employees, metaData: employeesWithMetaData.MetaData);
    }

    public async Task<EmployeeDto> GetEmployee(Guid companyId, Guid id)
    {
        var company = await _repository.Company.GetCompany(companyId);

        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employee = await _repository.Employee.GetEmployee(companyId, id);

        if (employee is null)
            throw new EmployeeNotFoundException(id);

        return employee;
    }

    public async Task<EmployeeDto> CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeDto)
    {
        var company = await _repository.Company.GetCompany(companyId);

        if(company is null)
            throw new CompanyNotFoundException(companyId);

        var employee = await _repository.Employee.CreateEmployeeForCompany(companyId, employeeDto);

        return employee;
    }

    public async Task DeleteEmployeeForCompany(Guid companyId, Guid employeeId)
    {
        var company = await _repository.Company.GetCompany(companyId);

        if(company is null)
            throw new CompanyNotFoundException(companyId);

        var employeeForCompany = _repository.Employee.GetEmployee(companyId, employeeId);

        if (employeeForCompany is null)
            throw new EmployeeNotFoundException(employeeId);

        await _repository.Employee.DeleteEmployee(employeeId);
    }

    public async Task UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employee)
    {
        var company = await _repository.Company.GetCompany(companyId);

        if(company is null)
            throw new CompanyNotFoundException(companyId);

        var employeeDto = await _repository.Employee.GetEmployee(companyId, id);

        if (employeeDto is null)
            throw new EmployeeNotFoundException(id);

        await _repository.Employee.UpdateEmployee(id, employee);
    }
}
