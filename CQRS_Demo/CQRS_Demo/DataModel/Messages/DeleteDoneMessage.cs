using CQRS_Demo.Framework;
using System;

namespace CQRS_Demo.DataModel.Messages
{
    public class DeleteDoneMessage : IAppCommand
    {
        public static DeleteDoneMessage Default { get; } = new DeleteDoneMessage();
    }
}
