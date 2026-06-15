using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repositories.TodoList
{
    public interface ITodoListRepositories
    {
        Task<List<Todolist>> GetTodolistMany();
    }
}