using System;
using Microsoft.EntityFrameworkCore;
using Payroll.Data.Models;

namespace Payroll.Data
{
    public class PayrollDbContext : DbContext
    {
        private readonly string _connectionString;

        public PayrollDbContext()
        {
        }

        public PayrollDbContext(DbContextOptions<PayrollDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<TimeCard> TimeCards { get; set; }
    }
}
