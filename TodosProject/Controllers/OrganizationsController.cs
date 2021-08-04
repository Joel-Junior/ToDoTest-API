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
    public class OrganizationsController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IOrganizationService _orgService;
        public OrganizationsController(IMapper mapper, IGeneralService generalService, IOrganizationService orgService,IUserService userService) : base(mapper, generalService)
        {
            _userService = userService;
            _orgService = orgService;
        }

        [HttpGet("v1/GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await Task.FromResult(_orgService.GetAll());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, $"Falha na busca das Organizações.");
            }
        }

        [HttpPost("v1/GetAllPaginate")]
        public async Task<IActionResult> GetAllPaginate([FromBody] OrganizationFilter orgFilter)
        {
            try
            {
                var result = await _orgService.GetAllPaginate(orgFilter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, $"Falha na busca das Organizações.");
            }
        }

        [HttpPost("v1/Add")]
        public async Task<IActionResult> Add([FromBody] OrganizationDto orgDto)
        {
            try
            {
                Organization org = _mapper.Map<Organization>(orgDto);
                ResultReturned result = new ResultReturned();

                if (ModelState.IsValid)
                {
                    var user = _userService.GetById(orgDto.IdUser.Value);
                    if (user == null)
                    {
                        return BadRequest(result);
                    }
                    if (org.Logo != null)
                    {
                        var extension = Path.GetExtension(org.Logo);
                        var imagemNome = Guid.NewGuid() + extension;
                        if (!UploadArquivo(orgDto.LogoUpload, imagemNome))
                        {
                            return BadRequest(result);
                        }

                        org.Logo = imagemNome;
                    }
                    var orgUsers = new OrganizationUser() { IdUser = user.Id.Value, IdProfile = 3 };//idProfile todo que cadastra organizacao é manager dele

                    org.OrganizationUsers.Add(orgUsers);

                    result = await Task.FromResult(_orgService.Add(org));
                    if (result.Result)
                    {
                        return Ok(result);
                    }
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, $"Falha ao salvar uma Organização.");
            }
        }

        [HttpPut("v1/Update")]
        public async Task<IActionResult> Update(int id, [FromBody] OrganizationDto orgDto)
        {
            Organization org = _mapper.Map<Organization>(orgDto);
            ResultReturned result = new ResultReturned();
            try
            {
                if (ModelState.IsValid)
                {
                    var orgOrig = _orgService.GetById(id).Result;
                    if (orgDto.LogoUpload != null)                    
                    {
                        
                        string imageName = orgOrig.Logo;
                        if(string.IsNullOrEmpty(imageName))
                        {
                            var extension = Path.GetExtension(orgDto.Logo);
                            imageName = Guid.NewGuid().ToString() + extension;
                        }

                        if (!UploadArquivo(orgDto.LogoUpload, imageName))
                        {
                            result.Message = "Falha Salvar Logo";
                            return BadRequest(result);
                        }

                        org.Logo = imageName;
                    }
                    else
                    {
                        org.Logo = orgOrig.Logo;
                    }
                    result = await Task.FromResult(_orgService.Update(id, org));
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
            result = await Task.FromResult(_orgService.Delete(id, isDeletePhysical));
            if (result.Result)
                return Ok(result);

            return BadRequest(result);
        }
        
        [HttpGet("v1/GetOrganizationsAvailable")]
        public async Task<IActionResult> GetOrganizationsAvailable()
        {
            try
            {
                var result = await _orgService.GetOrganizationsAvailable();
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
                var result =  _mapper.Map<OrganizationDto>(await _orgService.GetById(id));
                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao retornar dados da empresa");
            }
        }

        private bool UploadArquivo(string arquivo, string imgNome)
        {
            if (string.IsNullOrEmpty(arquivo))
            {                
                return false;
            }

            var imageDataByteArray = Convert.FromBase64String(arquivo);

            var dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "organizations");
            var filePath = Path.Combine(dirPath, imgNome);

            if(!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);                
            }

            System.IO.File.WriteAllBytes(filePath, imageDataByteArray);

            return true;
        }

        [HttpPost("v1/AddMemberUser")]
        public async Task<IActionResult> AddMemberUser([FromBody] UserFilter userFilter)
        {
            try
            {
                if(userFilter.Organizations == null || userFilter.Organizations.Count <= 0)
                    return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao retornar dados da empresa");

                var result = await _orgService.AddMemberUser(userFilter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao retornar dados da empresa");
            }
        }

        [HttpPost("v1/GetMemberUsers")]
        public async Task<IActionResult> GetMemberUsers([FromBody] UserFilter userFilter)
        {
            try
            {
                if (userFilter.Organizations == null || userFilter.Organizations.Count <= 0)
                    return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao retornar dados de usuários associados");

                var result = await _orgService.GetMemberUsers(userFilter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao retornar dados de usuários associados");
            }
        }

        [HttpDelete("v1/DeleteMemberUsers/{idUser}/{idOrganization}")]       
        public async Task<IActionResult> DeleteMemberUsers(long idUser,long idOrganization)
        {
            try
            {
                if (idUser<=0 || idOrganization <= 0)
                    return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao retornar dados de usuários associados");

                var result = await _orgService.DeleteMemberUsers(idUser, idOrganization);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao retornar dados de usuários associados");
            }
        }

        [HttpPost("v1/GetAccessGroups")]
        public async Task<IActionResult> GetAccessGroups([FromBody] AccessGroupFilter accessFilter)
        {
            try
            {
                if (!accessFilter.IdOrganization.HasValue || accessFilter.IdOrganization <= 0)
                    return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao retornar dados de grupos de acesso associados");

                var result = await _orgService.GetAccessGroups(accessFilter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao retornar dados de grupos de acesso associados");
            }
        }

        [HttpPost("v1/AddAccessGroupUser")]
        public async Task<IActionResult> AddAccessGroupUser([FromBody] UserFilter userFilter)
        {
            try
            {
                if (userFilter.Organizations == null || userFilter.Organizations.Count <= 0)
                    return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao retornar dados da empresa");

                var result = await _orgService.AddMemberUser(userFilter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao retornar dados da empresa");
            }
        }
        [HttpGet("v1/GetAllNotPaginate")]
        public async Task<IActionResult> GetAllNotPaginate()
        {
            try
            {
                var result = await Task.FromResult(_orgService.GetAll());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha na busca das empresa.");
            }
        }

        [HttpGet("v1/CheckExistingOneManager")]
        public async Task<IActionResult> CheckExistingOneManager([FromQuery]long idUser, long idOrganization)
        {
            try
            {
                if (idUser <= 0 || idOrganization <= 0)
                    return this.StatusCode(StatusCodes.Status500InternalServerError, $"Parâmetros não definidos.");

                var result = await _orgService.CheckExistingOneManager(idUser, idOrganization);
                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao verificar e-mail existente!");
            }
        }
    }
}
