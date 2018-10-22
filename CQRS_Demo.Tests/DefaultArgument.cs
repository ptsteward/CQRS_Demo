using System;
using System.Linq;

namespace Tests
{
    public class DefaultArgument
    {
        public DefaultArgument(string propertyName, object argument, bool skipNullCheck = false)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            Argument = argument ?? throw new ArgumentNullException(nameof(argument));
            SkipNullCheck = skipNullCheck;
        }

        public string PropertyName { get; }
        public object Argument { get; }
        public bool SkipNullCheck { get; }
    }
}
