using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Dtos.TodoList.Delete
{
    public class DeleteTodoListDtos
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
    }
}