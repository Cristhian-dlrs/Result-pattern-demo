namespace TechDemo.Domain.Permissions.ViewModels;

public class PermissionViewModel
{
    public PermissionViewModel(
        Guid id,
        string employeeForename,
        string employeeSurname,
        string permissionType,
        DateTime permissionDate)
    {
        Id = id;
        EmployeeForename = employeeForename;
        EmployeeSurname = employeeSurname;
        PermissionType = permissionType;
        PermissionDate = permissionDate;
    }

    public Guid Id { get; set; }
    public string EmployeeForename { get; }
    public string EmployeeSurname { get; }
    public string PermissionType { get; }
    public DateTime PermissionDate { get; }
}