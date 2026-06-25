using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities.Update
{
    public class TodoListUpdate
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}