using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Todolist
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
    }
}