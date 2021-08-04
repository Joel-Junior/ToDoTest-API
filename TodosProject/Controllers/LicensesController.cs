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
using System.Linq;
using System.Threading.Tasks;


namespace TodosProject.Controllers.Base
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize("Bearer")]
    public class LicensesController : BaseController
    {
        
        private readonly ILicenseService _licService;
        public LicensesController(IMapper mapper, IGeneralService generalService, ILicenseService licService) : base(mapper, generalService)
        {                       
            _licService = licService;
        }

        [HttpGet("v1/GetLicencesAvailableOrganization")]
        public async Task<IActionResult> GetLicencesAvailableOrganization([FromBody] long id)
        {            
            try
            {
                if(id<=0)
                   return this.StatusCode(StatusCodes.Status500InternalServerError, $"Organização não encontrada.");

                var result = await Task.FromResult(_licService.GetLicencesAvailableOrganization(id));
                return Ok(result);                

            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao consultar licenças.");
            }
        }

        [HttpPost("v1/GetAllPaginate")]
        public async Task<IActionResult> GetAllPaginate([FromBody] LicenseFilter liFilter)
        {
            try
            {
                var result = await Task.FromResult(_licService.GetAllPaginate(liFilter));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha na busca das Organizações.");
            }
        }

        [HttpPost("v1/Add")]
        public async Task<IActionResult> Add([FromBody] LicenseDto liDto)
        {
            License li = _mapper.Map<License>(liDto);
            ResultReturned result = new ResultReturned();
            
            if (ModelState.IsValid)
            {
                li.CodeID = Guid.NewGuid();
                result = await Task.FromResult(_licService.Add(li));
                if (result.Result)
                    return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPut("v1/Update")]
        public async Task<IActionResult> Update(int id, [FromBody] LicenseDto liDto)
        {
            License li = _mapper.Map<License>(liDto);
            ResultReturned result = new ResultReturned();
            try
            {
                if (ModelState.IsValid)
                {
                    result = await Task.FromResult(_licService.Update(id, li));
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
            ResultReturned result = await Task.FromResult(_licService.Delete(id, isDeletePhysical));
            if (result.Result)
                return Ok(result);

            return BadRequest(result);
        }
        
        [HttpGet("v1/GetById/{Id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var result = _mapper.Map<LicenseDto>(await _licService.GetById(id));
                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao retornar dados da licença");
            }
        }

        [HttpPost("v1/AddMinutesMultiplayer")]
        public async Task<IActionResult> AddMinutesMultiplayer([FromBody] string chaveLicense, long multiplayerMinutes)
        {
            ResultReturned result = new ResultReturned();

            if (ModelState.IsValid)
            {
                result = await Task.FromResult(_licService.AddMinutesMultiplayer(chaveLicense, multiplayerMinutes));
                if (result.Result)
                    return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("v1/GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await Task.FromResult(_licService.GetAll());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha na busca das Organizações.");
            }
        }
    }
}
