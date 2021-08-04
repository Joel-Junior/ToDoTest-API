using AutoMapper;
using TodosProject.Application.Interfaces;
using TodosProject.Domain.Dto;
using TodosProject.Domain.Entities;
using TodosProject.Domain.Enums;
using TodosProject.Domain.Filters;
using TodosProject.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodosProject.Controllers.Base
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize("Bearer")]
    //[AllowAnonymous]
    public class ToDosController : BaseController
    {
        private readonly IToDoService _toDoService;
        private readonly IUserService _userService;
        public ToDosController(IMapper mapper, IGeneralService generalService, IToDoService toDoService, IUserService userService) : base(mapper, generalService)
        {
            _toDoService = toDoService;
            _userService = userService;
        }

        [HttpGet("v1/GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Task.FromResult(_toDoService.GetAll()));
        }

        [HttpPost("v1/GetAllPaginate")]
        public async Task<IActionResult> GetAllPaginate([FromBody] ToDoFilter tdFilter)
        {
            try
            {
                var result = await _toDoService.GetAllPaginate(tdFilter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, $"Falha na busca das Tarefas.");
            }
        }

        [HttpPost("v1/Add")]
        public async Task<IActionResult> Add([FromBody] ToDoDto tdDto)
        {
            try
            {
                ToDo org = _mapper.Map<ToDo>(tdDto);
                ResultReturned result = new ResultReturned();

                if (ModelState.IsValid)
                {
                    var user = _userService.GetById(tdDto.IdUser);
                    if (user == null)
                    {
                        return BadRequest(result);
                    }

                    result = await Task.FromResult(_toDoService.Add(org));
                    if (result.Result)
                    {
                        return Ok(result);
                    }
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, $"Falha ao salvar uma Tarefa.");
            }
        }

        [HttpPut("v1/Update")]
        public async Task<IActionResult> Update(int id, [FromBody] ToDoDto tdDto)
        {
            ToDo org = _mapper.Map<ToDo>(tdDto);
            ResultReturned result = new ResultReturned();
            try
            {
                if (ModelState.IsValid)
                {
                    var tdOrig = _toDoService.GetById(id).Result;

                    if (tdOrig.ConcludeDate != null)
                        throw new Exception("Já foi concluída e não pode ser alterada");

                    result = await Task.FromResult(_toDoService.Update(id, org));
                    if (result.Result)
                        return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultReturned() { Result = false, Message = ex.Message });
            }

        }

        [HttpDelete("v1/Delete/{id}/{isDeletePhysical}")]
        public async Task<IActionResult> Delete(int id, bool isDeletePhysical)
        {
            ResultReturned result = new ResultReturned();
            result = await Task.FromResult(_toDoService.Delete(id, isDeletePhysical));
            if (result.Result)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("v1/Conclude")]
        public async Task<IActionResult> ConcludeToDo(long id, long idUser)
        {
            ResultReturned result = new ResultReturned();
            try
            {
                var user = _userService.GetById(idUser);
                if (user == null)
                {
                    return BadRequest(result);
                }
                var tdOrig = _toDoService.GetById(id).Result;

                result = await Task.FromResult(_toDoService.ConcludeToDo(id, user.Id.Value));
                if (result.Result)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultReturned() { Result = false, Message = ex.Message });
            }
        }

    }
}
