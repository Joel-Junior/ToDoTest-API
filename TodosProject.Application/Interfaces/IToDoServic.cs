using TodosProject.Domain.Dto;
using TodosProject.Domain.Entities;
using TodosProject.Domain.Filters;
using TodosProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TodosProject.Application.Interfaces
{
    public interface IToDoService
    {
        Task<ToDoPaged> GetAllPaginate(ToDoFilter filter);
        ResultReturned Add(ToDo toDo);
        ResultReturned Update(long id, ToDo toDo);
        ResultReturned Delete(long id, bool isDeletePhysical = false);
        Task<ToDo> GetById(long id);
        List<ToDo> GetAll();
        ResultReturned ConcludeToDo(long id, long idUser);
    }
}
