using System;
using System.Collections.Generic;
using System.Linq;
using Payroll.Data;
using Payroll.Data.Models;

namespace PayrollProcessor
{
    public class SqlPayrollService : IPayrollService
    {
        private PayrollDbContext _context;
        public SqlPayrollService(PayrollDbContext context)
        {
            _context = context;
        }

        public Employee GetEmployeeByTimeCard(TimeCard timeCard)
        {
            Employee employee = _context.Employees.FirstOrDefault(x => x.Id == timeCard.EmployeeId);
            return employee;
        }

        public List<TimeCard> GetTimeCardsByDate(DateTime startDateTime, DateTime endDateTime)
        {
            List<TimeCard> timeCards = new List<TimeCard>();
            timeCards = _context.TimeCards
                .Where(x => x.StartDateTime >= startDateTime && x.EndDateDateTime <= endDateTime).ToList();
            return timeCards;
        }
    }
}