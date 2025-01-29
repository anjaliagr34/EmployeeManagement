using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Models
{
    //Add by Anjali Agrawal
    public class EmployeeManagementDbContext : DbContext
    {
        public EmployeeManagementDbContext(DbContextOptions<EmployeeManagementDbContext> options)
            : base(options)
        { }

        // This DbSet represents the Employee table in your database
        public DbSet<Employee> Employees { get; set; }
    }
}

