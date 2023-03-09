using Microsoft.EntityFrameworkCore;

namespace employeeWebApp
{
    public class EmployeeDbContext:DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees => Set<Employee>();
    }
}
