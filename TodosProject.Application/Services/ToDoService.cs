using AutoMapper;
using TodosProject.Application.Interfaces;
using TodosProject.Domain;
using TodosProject.Domain.Dto;
using TodosProject.Domain.Entities;
using TodosProject.Domain.Enums;
using TodosProject.Domain.Filters;
using TodosProject.Domain.Models;
using TodosProject.Infra.CrossCutting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Data.Entity.SqlServer;

namespace TodosProject.Application.Services
{
    public class ToDoService : IToDoService
    {
        public readonly IRepository<ToDo> _toDoRepository;
        protected readonly IMapper _mapper;
        public ToDoService(IRepository<ToDo> toDoRepository, IMapper mapper)
        {
            _toDoRepository = toDoRepository;
            _mapper = mapper;
        }
        public ResultReturned Add(ToDo todo)
        {
            try
            {
                if (todo.DueDate == DateTime.MinValue)
                    return new ResultReturned() { Result = false, Message = "Para realizar o cadastro, o campo data de Vencimento deve ser preenchido" };

               
                    todo.CreatedTime = DateTime.Now;
                    todo.IsActive = true;
                    _toDoRepository.Add(todo);
                    _toDoRepository.SaveChanges();
                    return new ResultReturned() { Result = true, Message = Constants.SuccessInAdd };
            }
            catch (Exception ex)
            {
                return new ResultReturned() { Result = false, Message = Constants.ErrorInAdd };
            }
        }

        public ResultReturned Update(long id, ToDo toDo)
        {
            ToDo tdDb = _toDoRepository.GetById(id);

            if (tdDb is not null)
            {
                if (tdDb.ConcludeDate != null)
                    return new ResultReturned() { Result = false, Message = "Tarefa não pode ser alterada pois já está concluida." };

                tdDb.DueDate = toDo.DueDate;
                tdDb.Description = toDo.Description;
                tdDb.IsActive = toDo.IsActive;
                tdDb.UpdateTime = DateTime.Now;
                _toDoRepository.Update(tdDb);
                _toDoRepository.SaveChanges();
                return new ResultReturned() { Result = true, Message = Constants.SuccessInUpdate };
            }

            return new ResultReturned() { Result = true, Message = Constants.ErrorInUpdate };
        }

        public ResultReturned Delete(long id, bool isDeletePhysical = false)
        {
            ToDo td = _toDoRepository.GetById(id);

            if (isDeletePhysical && td is not null)
                _toDoRepository.Remove(td);

            else if (!isDeletePhysical && td is not null)
            {
                td.UpdateTime = DateTime.Now;
                td.IsActive = td.IsActive ? false : true;
                _toDoRepository.Update(td);
                _toDoRepository.SaveChanges();
                return new ResultReturned() { Result = true, Message = Constants.SuccessInDelete };
            }

            return new ResultReturned() { Result = false, Message = Constants.ErrorInDelete };
        }

        public async Task<ToDoPaged> GetAllPaginate(ToDoFilter filter)
        {

            List<ToDo> resultado = null;
            var query = _toDoRepository.GetAllTracking()
                   .Include(x => x.User)
                   .Where(x => (!filter.Status.HasValue || filter.Status.Value == x.IsActive)
                   && (filter.User == null || (filter.User != null && !string.IsNullOrEmpty(filter.User.Login) && x.User.Login == filter.User.Login))
                   ).AsQueryable();

            if (!string.IsNullOrEmpty(filter.Search))
            {
                resultado = await query.AsQueryable().AsNoTracking()
                    .Where(x =>
                    (x.DueDate == null || x.DueDate.ToString("dd/MM/yyyy").StartsWith(filter.Search.ToString())) ||
                  
                    (x.ConcludeDate != null ? x.ConcludeDate.Value.ToString("dd/MM/yyyy").Contains(filter.Search):true) ||
                   
                    x.User.Name.ToLower().Contains(filter.Search.ToLower())
                ).ToListAsync();
            }
            else
            {
                resultado = query.AsQueryable().AsNoTracking().OrderByDescending(p => p.DueDate).ToList();
            }           
 
            var queryResult = _mapper.Map<List<ToDoDto>>(resultado);

            return new ToDoPaged
            {
                ToDoReturnedSet = queryResult.Count() > (filter.pageIndex) * filter.pageSize ? queryResult.Skip((filter.pageIndex) * filter.pageSize).Take(filter.pageSize).ToList() : queryResult.ToList(),
                NextPage = (filter.pageSize * filter.pageIndex) >= queryResult.Count() ? null : (int?)filter.pageIndex + 1,
                Page = filter.pageIndex,
                Total = queryResult.Count()
            };
        }

        public async Task<ToDo> GetById(long id)
        {
            try
            {
                return await _toDoRepository.FindBy(x => x.Id == id)
                    .AsNoTracking()
                    .Include(x => x.User)
                    .FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ResultReturned ConcludeToDo(long id,long idUser)
        {
            try
            {
                ToDo tdDb = _toDoRepository.FindBy(x => x.Id==id && x.IdUser == idUser).FirstOrDefault();

                if (tdDb is not null)
                {
                    if (tdDb.ConcludeDate != null)
                        return new ResultReturned() { Result = false, Message = "Tarefa não pode ser alterada pois já está concluida." };

                    tdDb.ConcludeDate = DateTime.Now;
                    tdDb.UpdateTime = DateTime.Now;
                    tdDb.IdUser = idUser;
                    _toDoRepository.Update(tdDb);
                    _toDoRepository.SaveChanges();
                    return new ResultReturned() { Result = true, Message = Constants.SuccessInUpdate };
                }
                return new ResultReturned() { Result = false, Message = Constants.ErrorInUpdate };
            }
            catch (Exception ex)
            {
                return new ResultReturned() { Result = false, Message = Constants.ErrorInUpdate };
            }
        }
        public List<ToDo> GetAll()
        {
            return _toDoRepository.GetAll().ToList();
        }
    }
}
