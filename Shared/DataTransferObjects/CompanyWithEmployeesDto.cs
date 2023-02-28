namespace Shared.DataTransferObjects;

public record CompanyWithEmployeesDto
{
    public Guid CompanyId { get; init; }

    public string? Name { get; init; }

    public string? FullAddress { get; init; }

    public List<EmployeeDto> Employees { get; init; } = new();
}
