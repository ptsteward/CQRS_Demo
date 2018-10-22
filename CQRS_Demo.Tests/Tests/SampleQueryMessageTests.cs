using CQRS_Demo.DataModel.Messages;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Tests
{
    [TestFixture]
    public class SampleQueryMessageTests : AppQueryTests<SampleQueryMessage, int>
    {
        protected override IEnumerable<DefaultArgument> DefaultArguments => new[]
        {
            new DefaultArgument(nameof(SampleQueryMessage.Arg1), "abc"),
            new DefaultArgument(nameof(SampleQueryMessage.Arg2), "def"),
            new DefaultArgument(nameof(SampleQueryMessage.Arg3), 23, skipNullCheck: true)
        };
    }
}
