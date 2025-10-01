// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Eawv.Service.Utils;
using FluentAssertions;
using Xunit;

namespace Eawv.Service.Unit.Tests.Utils;

public class GuidUtilsTest
{
    [Theory]
    [InlineData("test", "74736574-0000-0000-0000-000000000000")]
    [InlineData("very-long input string to test", "d7a65a0a-8920-49d5-72aa-f024ee5ebf28")]
    [InlineData("", "00000000-0000-0000-0000-000000000000")]
    public void ShouldGenerateGuid(string input, string expected)
    {
        GuidUtils
            .GuidFromString(input)
            .ToString()
            .Should()
            .Be(expected);
    }
}
