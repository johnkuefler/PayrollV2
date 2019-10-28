using System;
using System.Collections.Generic;
using System.Text;
using Payroll.Data.Models;
using PayrollProcessor.Models;

namespace PayrollProcessor
{
    public interface IHrService
    {
        MealDeduction GetDiningDeductionForEmployee(string employeeId);
    }
}
