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
    public class ProfilesController : BaseController
    {
        private readonly IProfileService _profileService;

        public ProfilesController(IMapper mapper, IGeneralService generalService, IProfileService profileService) : base(mapper, generalService)
        {
            _profileService = profileService;
        }

        [HttpGet("v1/GetProfilesAvailable")]
        public async Task<IActionResult> GetProfilesAvailable(bool includeAdmin = false)
        {
            try
            {
                var result = await _profileService.GetProfilesAvailable(includeAdmin);
                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, $"Falha ao consultar perfil.");
            }
        }
    }
}
