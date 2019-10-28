using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture;
using DotnetTestUtils;
using Xunit;

namespace Payroll.Tests
{
    public  class PayrollProcessorTests
    {
        private Fixture _fixture;

        public PayrollProcessorTests()
        {
            _fixture = new FixtureFactory().WithDefaults().Create();
        }

        [Fact]
        public void ProcessPayroll_InvalidDates_ThrowsException()
        {
            // arrange
            var sut = _fixture.Create<PayrollProcessor.PayrollProcessor>();

            // act and assert

            Assert.Throws<Exception>(() => sut.ProcessPayroll(DateTime.Now, DateTime.Now.AddDays(-1)));

        }


        [Fact]
        public void ProcessPayroll_ValidInput_CompletesSuccessfully()
        {
            // arrange
            var sut = _fixture.Create<PayrollProcessor.PayrollProcessor>();


            // act 
            sut.ProcessPayroll(DateTime.Now, DateTime.Now.AddDays(14));

            // assert
            Assert.True(true);
        }
    }
}
