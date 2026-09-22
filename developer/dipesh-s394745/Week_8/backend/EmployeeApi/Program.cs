using EmployeeApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Let the Next.js app (running on port 3000) call this API from the browser.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJs", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors("AllowNextJs");

// ---- "Database" -----------------------------------------------------
// For this practice project we keep everything in a simple List instead
// of a real database. Data resets every time the API restarts.
var employees = new List<Employee>
{
    new() { Id = 1, FullName = "Ava Thompson", Email = "ava.thompson@example.com", Department = "Engineering", JobTitle = "Software Engineer", Salary = 92000, HireDate = new DateTime(2022, 3, 14), IsActive = true },
    new() { Id = 2, FullName = "Liam Chen", Email = "liam.chen@example.com", Department = "Sales", JobTitle = "Account Executive", Salary = 78000, HireDate = new DateTime(2021, 7, 1), IsActive = true },
    new() { Id = 3, FullName = "Sofia Martinez", Email = "sofia.martinez@example.com", Department = "Marketing", JobTitle = "Marketing Manager", Salary = 88000, HireDate = new DateTime(2020, 11, 23), IsActive = true },
    new() { Id = 4, FullName = "Noah Patel", Email = "noah.patel@example.com", Department = "Engineering", JobTitle = "QA Engineer", Salary = 74000, HireDate = new DateTime(2023, 1, 9), IsActive = false },
    new() { Id = 5, FullName = "Emma Wilson", Email = "emma.wilson@example.com", Department = "HR", JobTitle = "HR Specialist", Salary = 65000, HireDate = new DateTime(2019, 5, 30), IsActive = true },
};
var nextId = 6;

// ---- Helper: very simple validation ----------------------------------
List<string> Validate(Employee e)
{
    var errors = new List<string>();
    if (string.IsNullOrWhiteSpace(e.FullName)) errors.Add("Full name is required.");
    if (string.IsNullOrWhiteSpace(e.Email) || !e.Email.Contains('@')) errors.Add("A valid email is required.");
    if (string.IsNullOrWhiteSpace(e.Department)) errors.Add("Department is required.");
    if (string.IsNullOrWhiteSpace(e.JobTitle)) errors.Add("Job title is required.");
    if (e.Salary < 0) errors.Add("Salary cannot be negative.");
    return errors;
}

// ---- CRUD endpoints ----------------------------------------------------

// GET all employees
app.MapGet("/api/employees", () => employees);

// GET one employee by id
app.MapGet("/api/employees/{id:int}", (int id) =>
{
    var employee = employees.FirstOrDefault(e => e.Id == id);
    return employee is null ? Results.NotFound() : Results.Ok(employee);
});

// POST - create a new employee
app.MapPost("/api/employees", (Employee input) =>
{
    var errors = Validate(input);
    if (errors.Count > 0) return Results.BadRequest(new { errors });

    input.Id = nextId++;
    employees.Add(input);
    return Results.Created($"/api/employees/{input.Id}", input);
});

// PUT - update an existing employee
app.MapPut("/api/employees/{id:int}", (int id, Employee input) =>
{
    var errors = Validate(input);
    if (errors.Count > 0) return Results.BadRequest(new { errors });

    var employee = employees.FirstOrDefault(e => e.Id == id);
    if (employee is null) return Results.NotFound();

    employee.FullName = input.FullName;
    employee.Email = input.Email;
    employee.Department = input.Department;
    employee.JobTitle = input.JobTitle;
    employee.Salary = input.Salary;
    employee.HireDate = input.HireDate;
    employee.IsActive = input.IsActive;

    return Results.Ok(employee);
});

// DELETE - remove an employee
app.MapDelete("/api/employees/{id:int}", (int id) =>
{
    var employee = employees.FirstOrDefault(e => e.Id == id);
    if (employee is null) return Results.NotFound();

    employees.Remove(employee);
    return Results.NoContent();
});

app.Run();
