using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities.Create
{
    public class TodoListCreate
    {
        public string? Name { get; set; }
        public string? Category { get; set; }
    }
}