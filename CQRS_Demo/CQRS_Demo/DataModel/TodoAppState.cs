using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQRS_Demo.DataModel
{
    public class TodoAppState
    {
        public IEnumerable<TodoItem> TodoItems { get; set; } = Enumerable.Empty<TodoItem>();
    }
}
