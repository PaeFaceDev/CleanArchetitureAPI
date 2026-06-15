using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories.TodoList;

namespace Infrastructure.Persistence.Repositories.TodoList
{
    public class GetTodoListRepositories : ITodoListRepositories
    {
        public async Task<List<Todolist>> GetTodolistMany()
        {
            return new List<Todolist>();
        }
    }
}