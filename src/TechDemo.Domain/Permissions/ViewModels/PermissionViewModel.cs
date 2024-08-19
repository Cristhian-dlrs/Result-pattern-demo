namespace TechDemo.Domain.Permissions.ViewModels;

public class PermissionViewModel
{
    public PermissionViewModel(
        string employeeForename,
        string employeeSurname,
        string permissionType,
        DateTime permissionDate)
    {
        EmployeeForename = employeeForename;
        EmployeeSurname = employeeSurname;
        PermissionType = permissionType;
        PermissionDate = permissionDate;
    }

    public string EmployeeForename { get; }
    public string EmployeeSurname { get; }
    public string PermissionType { get; }
    public DateTime PermissionDate { get; }
}