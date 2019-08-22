using Functions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class FunctionTests
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public void WarmingTimer_Should_log_message()
        {
            var logger = (ListLogger)TestFactory.CreateLogger(LoggerTypes.List);
            TimerWarmingFunction.Run(null, logger);
            var msg = logger.Logs[0];
            Assert.Contains("Called warming function at", msg);
        }

        [Fact]
        public async Task CreateUserTest()
        {
            var testUser = new User { Name = "Test user", DateOfBirth = DateTime.Now };

            Mock<HttpRequest> mockRequest = TestFactory.CreateMockRequest(testUser);

            User result = await CreateUser.Run(mockRequest.Object, null,new Microsoft.Azure.WebJobs.ExecutionContext());

            Assert.Equal(testUser.Name, result.Name);
        }
    }
}
