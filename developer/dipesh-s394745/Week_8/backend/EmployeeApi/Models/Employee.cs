namespace EmployeeApi.Models;

// A simple class that represents one employee row.
// No database is used in this project - we just keep a List<Employee> in memory.
public class Employee
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Department { get; set; } = "";
    public string JobTitle { get; set; } = "";
    public decimal Salary { get; set; }
    public DateTime HireDate { get; set; }
    public bool IsActive { get; set; } = true;
}
