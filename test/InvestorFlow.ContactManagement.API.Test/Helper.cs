using Microsoft.Extensions.Logging;

namespace InvestorFlow.ContactManagement.API.Test;

public static class Helper
{
    public static void VerifyLogging<T>(this Mock<ILogger<T>> logger, string expectedMessage = "",
        LogLevel expectedLogLevel = LogLevel.Information, Times? times = null)
    {
        times ??= Times.Once();

        logger.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == expectedLogLevel),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) =>
                    string.IsNullOrEmpty(expectedMessage) || string.Equals(expectedMessage, o.ToString())),
                It.IsAny<Exception>(),
                ((Func<It.IsAnyType, Exception, string>)It.IsAny<object>())!),
            (Times)times);
    }
}
