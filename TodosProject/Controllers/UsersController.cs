using AutoMapper;
using TodosProject.Application.Interfaces;
using TodosProject.Domain.Dto;
using TodosProject.Domain.Entities;
using TodosProject.Domain.Enums;
using TodosProject.Domain.Filters;
using TodosProject.Domain.Models;
using TodosProject.Infra.CrossCutting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IMapper mapper, IGeneralService generalService, IUserService userService) : base(mapper, generalService)
        {
            _userService = userService;
        }

        [HttpGet("v1/GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Task.FromResult(_userService.GetAll()));
        }

        [HttpPost("v1/GetAllPaginate")]
        public async Task<IActionResult> GetAllPaginate([FromBody] UserFilter userFilter)
        {            
            try
            {
                var result = await Task.FromResult(_userService.GetAllPaginate(userFilter));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, $"Falha na busca dos Usuários.");
            }
        }

        [HttpGet("v1/GetById/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            return Ok(await Task.FromResult(_userService.GetById(id)));
        }

        [HttpGet("v1/GetByLogin/{login}")]
        public async Task<IActionResult> GetByLogin(string login)
        {
            return Ok(await Task.FromResult(_userService.GetByLogin(login)));
        }

        [HttpGet("v1/GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await Task.FromResult(_userService.GetUsers()));
        }

        [HttpPost("v1/Add")]
        public async Task<IActionResult> Add([FromBody] UserSendDto userSendDto)
        {
            User user = _mapper.Map<User>(userSendDto);
            ResultReturned result = new ResultReturned();
            user.Password = new ExtensionMethods().BuildPassword(8);
            if (ModelState.IsValid)
            {
                result = await Task.FromResult(_userService.Add(user));
                if (result.Result)
                    return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPut("v1/Update")]
        public async Task<IActionResult> Update(int id, [FromBody] UserSendDto userSendDto)
        {
            User user = _mapper.Map<User>(userSendDto);
            ResultReturned result = new ResultReturned();
            try
            {
                if (ModelState.IsValid)
                {
                    result = await Task.FromResult(_userService.Update(id, user));
                    if (result.Result)
                        return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(result);
            }
            
        }

        [HttpDelete("v1/Delete/{id}/{isDeletePhysical}")]
        public async Task<IActionResult> Delete(int id, bool isDeletePhysical)
        {
            ResultReturned result = await _userService.Delete(id, isDeletePhysical);
            if (result.Result)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("v1/InactivateActivateUser")]
        public async Task<IActionResult> InactivateActivateUser(long userID)
        {
            try
            {
                var result = await _userService.InactivateActivateUser(userID);
                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao atualizar Usuário");
            }
        }

        [HttpGet("v1/VerifyExistsLogin")]
        public async Task<IActionResult> VerifyExistsLogin(string login, long? id)
        {
            try
            {
                var result = await _userService.VerifyExistsLogin(login, id);
                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao verificar login existente!");
            }
        }

        [HttpGet("v1/VerifyExistsEmail")]
        public async Task<IActionResult> VerifyExistsEmail(string email, long? id)
        {
            try
            {
                var result = await _userService.VerifyExistsEmail(email, id);
                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao verificar e-mail existente!");
            }
        }

        [HttpPost("v1/UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] User change)
        {
            try
            {
                var result = await _userService.UpdatePassword(change.Id.Value, change.Password, change.LastPassword);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
