using System;
using System.Collections.Generic;
using Payroll.Data.Models;

namespace PayrollProcessor
{
    public interface IPayrollService
    {
        Employee GetEmployeeByTimeCard(TimeCard timeCard);
        List<TimeCard> GetTimeCardsByDate(DateTime startDateTime, DateTime endDateTime);
    }
}