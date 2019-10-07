using System;

namespace Payroll.Data.Models
{
    public  class TimeCard
    {
        public int Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateDateTime { get; set; }
        public int TotalHours { get; set; }
        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
