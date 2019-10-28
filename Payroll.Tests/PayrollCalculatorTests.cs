using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture;
using DotnetTestUtils;
using Payroll.Data.Models;
using PayrollProcessor;
using PayrollProcessor.Models;
using Xunit;

namespace Payroll.Tests
{
    public class PayrollCalculatorTests
    {
        private Fixture _fixture;
        public PayrollCalculatorTests()
        {
            _fixture = new FixtureFactory()
                .WithDefaults()
                .Create();
        }

        [Theory]
        [InlineData(25, true, false, 0.2f, 50, 50, 80, 1520,false)]
        [InlineData(25, true, true, 0.2f, 50, 50, 80, 1600, false)]
        [InlineData(25, false, true, 0.2f, 50, 50, 80, 1580, false)]
        [InlineData(25, true, true, 0.2f, 50, 50, 80, 1584, true)]
        public void Calculate_ValidInput_ReturnsCorrectPayroll(double hourlyRate, bool insuranceBeforeTaxes, 
            bool seniority, double taxRate, double health, double life, int hours, double expectedNetPay, bool hasMealDeduction)
        {
            // arrange
            Employee employee = new Employee
            {
                HourlyRate = hourlyRate,
                InsuranceBeforeTaxes = insuranceBeforeTaxes,
                Seniority = seniority
            };

            TaxBracket taxBracket = new TaxBracket
            {
                TaxRate = taxRate
            };

            InsuranceAmounts insAmounts = new InsuranceAmounts
            {
                Health = health,
                Life = life
            };

            TimeCard timeCard = new TimeCard
            {
                TotalHours = hours
            };

            MealDeduction mealDeduction = new MealDeduction
            {
                Deduction = hasMealDeduction
            };

            // act
            var sut = _fixture.Create<PayrollCalculator>();
            var employeePay = sut.Calculate(employee, taxBracket, insAmounts, timeCard, mealDeduction);

            // assert
            Assert.Equal(expectedNetPay, employeePay.NetPay);
        }
    }
}
