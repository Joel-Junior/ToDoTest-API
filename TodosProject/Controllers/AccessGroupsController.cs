using AutoMapper;
using TodosProject.Application.Interfaces;
using TodosProject.Controllers.Base;
using TodosProject.Domain.Dto;
using TodosProject.Domain.Entities;
using TodosProject.Domain.Filters;
using TodosProject.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TodosProject.Controllers.Base
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize("Bearer")]
    public class AccessGroupsController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IAccessGroupService _acGpService;
        public AccessGroupsController(IMapper mapper, IGeneralService generalService, IAccessGroupService acGpService,IUserService userService) : base(mapper, generalService)
        {
            _userService = userService;
            _acGpService = acGpService;
        }

        [HttpPost("v1/GetAll")]
        public async Task<IActionResult> GetAll([FromBody] AccessGroupFilter orgFilter)
        {
            try
            {
                var result = await _acGpService.GetAllPaginate(orgFilter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, $"Falha na busca das Groupos de Acesso.");
            }
        }

        [HttpPost("v1/GetAllPaginate")]
        public async Task<IActionResult> GetAllPaginate([FromBody] AccessGroupFilter orgFilter)
        {
            try
            {
                var result = await Task.FromResult(_acGpService.GetAllPaginate(orgFilter));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, $"Falha na busca das Groupos de Acesso.");
            }
        }

        [HttpPost("v1/Add")]
        public async Task<IActionResult> Add([FromBody] AccessGroupDto acGpDto)
        {
            try
            {
                AccessGroup access = _mapper.Map<AccessGroup>(acGpDto);
                ResultReturned result = new ResultReturned();

                if (ModelState.IsValid)
                {
                    
                    result = await Task.FromResult(_acGpService.Add(access));
                    if (result.Result)
                    {
                        return Ok(result);
                    }
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, $"Falha ao salvar uma Grupo de Acesso.");
            }
        }

        [HttpPut("v1/Update")]
        public async Task<IActionResult> Update(int id, [FromBody] AccessGroupDto acGpDto)
        {
            AccessGroup org = _mapper.Map<AccessGroup>(acGpDto);
            ResultReturned result = new ResultReturned();
            try
            {               
                if (ModelState.IsValid)
                {
                    result = await Task.FromResult(_acGpService.Update(id, org));
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
            ResultReturned result = new ResultReturned();
            result = await Task.FromResult(_acGpService.Delete(id, isDeletePhysical));
            if (result.Result)
                return Ok(result);

            return BadRequest(result);
        }
        
        [HttpGet("v1/GetAccessGroupsAvailable")]
        public async Task<IActionResult> GetAccessGroupsAvailable()
        {
            try
            {
                var result = await _acGpService.GetAccessGroupsAvailable();
                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao consultar perfil.");
            }
        }

        [HttpGet("v1/GetById/{Id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var result =  _mapper.Map<AccessGroupDto> (await _acGpService.GetById(id));
                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao retornar dados da empresa");
            }
        }
             

        [HttpPost("v1/AddMemberUser")]
        public async Task<IActionResult> AddMemberUser([FromBody] UserFilter userFilter)
        {
            try
            {
                if(userFilter == null || userFilter.AccessGroups.Count <= 0)
                    return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao deletar dados de associação");

                var result = await _acGpService.AddMemberUser(userFilter);

                if(result.Result)
                    return Ok(result);

                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao deletar os dados de associação");


            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao deletar dados de associação");
            }
        }

        [HttpPost("v1/GetMemberUsers")]
        public async Task<IActionResult> GetMemberUsers([FromBody] UserFilter userFilter)
        {
            try
            {
                if (userFilter.AccessGroups == null || userFilter.AccessGroups.Count <= 0)
                    return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao retornar dados de usuários associados");

                var result = await _acGpService.GetMemberUsers(userFilter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao retornar dados de usuários associados");
            }
        }

        [HttpDelete("v1/DeleteMemberUsers/{idUser}/{idAccessGroup}")]       
        public async Task<IActionResult> DeleteMemberUsers(long idUser,long idAccessGroup)
        {
            try
            {
                if (idUser<=0 || idAccessGroup <= 0)
                    return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao retornar dados de usuários associados");

                var result = await _acGpService.DeleteMemberUsers(idUser, idAccessGroup);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao retornar dados de usuários associados");
            }
        }
    }
}
