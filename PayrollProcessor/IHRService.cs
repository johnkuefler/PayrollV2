using Payroll.Data.Models;
using PayrollProcessor.Models;

namespace PayrollProcessor
{
    public interface IHRService
    {
        InsuranceAmounts GetInsuranceAmounts(Employee employee);
        TaxBracket GetTaxBracket(double salary, Employee employee);
    }
}