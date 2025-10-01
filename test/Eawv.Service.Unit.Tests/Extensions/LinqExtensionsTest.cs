// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Linq;
using Eawv.Service.Extensions;
using FluentAssertions;
using Xunit;

namespace Eawv.Service.Unit.Tests.Extensions;

public class LinqExtensionsTest
{
    [Fact]
    public void PeekShouldWork()
    {
        var resultList = new List<int>();
        var input = new[] { 1, 2, 3, 4 };

        var output = input
            .Peek(x => resultList.Add(x))
            .ToList();

        output.Should().BeEquivalentTo(resultList);
    }
}
