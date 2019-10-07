using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Data.Models
{
    public class Employee
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Seniority { get; set; }
        public double HourlyRate { get; set; }
        public string ZipCode { get; set; }
        public bool InsuranceBeforeTaxes { get; set; }
        public virtual ICollection<TimeCard> TimeCards { get; set; }
    }
}
