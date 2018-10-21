using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS_Demo.DataModel
{
    public class TodoItem
    {
        public string Description { get; set; }
        public bool Done { get; set; }
    }
}
