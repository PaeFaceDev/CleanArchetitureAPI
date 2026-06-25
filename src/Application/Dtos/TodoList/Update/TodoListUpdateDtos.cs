using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Dtos.TodoList.Update
{
    public class TodoListUpdateDtos
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}