// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.Extensions.Logging;
using Moq;

namespace Eawv.Service.Unit.Tests.Extensions;

public static class MockExtensions
{
    public static Mock<ILogger<T>> VerifyLogging<T>(this Mock<ILogger<T>> logger, string expectedMessage, LogLevel expectedLogLevel = LogLevel.Debug, Times? times = null)
    {
        times ??= Times.Once();

        Func<object, Type, bool> state = (v, t) => t != null && string.Equals(v.ToString(), expectedMessage, StringComparison.InvariantCultureIgnoreCase);

        logger.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == expectedLogLevel),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => state(v, t)),
                It.IsAny<Exception>(),
#pragma warning disable RCS1163 // Unused parameter.
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
            (Times)times);
#pragma warning restore RCS1163 // Unused parameter.

        return logger;
    }
}
