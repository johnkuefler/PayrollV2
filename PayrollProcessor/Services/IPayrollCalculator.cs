using Payroll.Data.Models;
using PayrollProcessor.Models;

namespace PayrollProcessor
{
    public interface IPayrollCalculator
    {
        EmployeePayroll Calculate(Employee employee, TaxBracket taxBracket, InsuranceAmounts insuranceAmounts, TimeCard timeCard);
    }
}