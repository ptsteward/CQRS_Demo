using CQRS_Demo.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS_Demo.DataModel.Messages
{
    public class SampleQueryMessage : IAppQuery<int>
    {
        public SampleQueryMessage(string arg1, string arg2, int arg3)
        {
            Arg1 = arg1 ?? throw new ArgumentNullException(nameof(arg1));
            Arg2 = arg2 ?? throw new ArgumentNullException(nameof(arg2));
            Arg3 = arg3;
        }

        public string Arg1 { get; }
        public string Arg2 { get; }
        public int Arg3 { get; }

        public static SampleQueryMessage Default { get; } = new SampleQueryMessage(string.Empty, string.Empty, default(int));
    }
}
