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

namespace TodosProject.Application.Services
{
    public class ProfileService : IProfileService
    {
        public readonly IRepository<Profile> _profileRepository;

        public ProfileService(IRepository<Profile> profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<List<DropDownList>> GetProfilesAvailable(bool includeAdmin = false)
        {
            return await _profileRepository.FindBy(x => x.IsActive == true && (!includeAdmin ? x.ProfileType != EnumProfileType.Admin : true ) )
                   .Select(x => new DropDownList()
                   {
                       Id = x.Id.Value,
                       Description = x.Description
                   }).ToListAsync();
        }
    }
}
