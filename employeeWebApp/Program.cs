
using employeeWebApp;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<EmployeeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeDbContext")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

async Task<List<Employee>> GetAllEmployee(EmployeeDbContext context) => await context.Employees.ToListAsync();
app.MapGet("/employees", async (EmployeeDbContext context) => await context.Employees.ToListAsync());
app.MapGet("/employees/{id}", async (EmployeeDbContext context, int id) => await context.Employees.FindAsync(id) is Employee Item ? Results.Ok(Item) : Results.NotFound("Employee not found"));
app.MapPost("Add/Employee", async (EmployeeDbContext context, Employee Item) =>
{
    context.Employees.Add(Item);
    await context.SaveChangesAsync();
    return Results.Ok(await GetAllEmployee(context));
});

app.MapPut("/employees/{id}", async (EmployeeDbContext context, Employee Item, int id) =>
{
    var emp = await context.Employees.FindAsync(id); ;
    if (emp == null) return Results.NotFound("Employee not found");
    emp.Address = Item.Address;
    emp.empName = Item.empName;
    await context.SaveChangesAsync();
    return Results.Ok(await GetAllEmployee(context));
});

app.MapDelete("/Delete/{id}", async (EmployeeDbContext context, int id) =>
{
    var emp = await context.Employees.FindAsync(id); ;
    if (emp == null) return Results.NotFound("Employee not found");
    context.Remove(emp);
    await context.SaveChangesAsync();
    return Results.Ok(await GetAllEmployee(context));
});
app.Run();


app.Run();
