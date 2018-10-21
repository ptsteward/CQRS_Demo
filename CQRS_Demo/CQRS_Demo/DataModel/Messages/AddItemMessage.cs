using CQRS_Demo.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS_Demo.DataModel.Messages
{
    public class AddItemMessage : IAppCommand
    {
        public static AddItemMessage Default { get; } = new AddItemMessage();
    }
}
