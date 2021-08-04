using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodosProject.Application.Interfaces;
using TodosProject.Domain;
using TodosProject.Domain.Dto;
using TodosProject.Domain.Entities;
using TodosProject.Domain.Filters;
using TodosProject.Domain.Models;

namespace TodosProject.Application.Services
{
    public class AccessGroupService : IAccessGroupService
    {
        public readonly IRepository<AccessGroup> _accessRepository;
        private readonly IRepository<AccessGroupUser> _accessGUserRepository;
        private readonly IGeneralService _generalService;
        private readonly IUserService _userService;
        protected readonly IMapper _mapper;
        private readonly string _url;
        public AccessGroupService(IRepository<AccessGroup> accessRepository, IRepository<AccessGroupUser> accessGUserRepository, IGeneralService generalService, IMapper mapper,IUserService userService)
        {
            _generalService = generalService;
            _accessRepository = accessRepository;
            _mapper = mapper;
            _userService = userService;
            _accessGUserRepository = accessGUserRepository;
        }

        public ResultReturned Add(AccessGroup access)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(access.Name))
                    return new ResultReturned() { Result = false, Message = "Para realizar o cadastro, o campo nome deve ser preenchido" };

                else if (!ExistByAccessGroup(access.Name))
                {
                    access.CreatedTime = DateTime.Now;
                    access.IsActive = true;

                    _accessRepository.Add(access);
                    _accessRepository.SaveChanges();
                    return new ResultReturned() { Result = true, Message = Constants.SuccessInAdd , Data = (dynamic)access.Id };
                }

                return new ResultReturned() { Result = false, Message = $"Já existe um Grupo de Acesso {access.Name} cadastrado, troque de nome" };
            }
            catch (Exception ex)
            {
                return new ResultReturned() { Result = false, Message = Constants.ErrorInAdd };
            }
        }

        public ResultReturned Delete(long id, bool isDeletePhysical = false)
        {
            AccessGroup access = _accessRepository.GetById(id);

            if (isDeletePhysical && access is not null)
            {
                if(access.AccessGroupUsers.Any())
                { 
                    _accessGUserRepository.RemoveRange(access.AccessGroupUsers);
                }
                _accessRepository.Remove(access);
            }              

            else if (!isDeletePhysical && access is not null)
            {
                access.UpdateTime = DateTime.Now;
                access.IsActive = access.IsActive ? false : true;
                _accessRepository.Update(access);
                _accessRepository.SaveChanges();
                return new ResultReturned() { Result = true, Message = Constants.SuccessInDelete };
            }

            return new ResultReturned() { Result = false, Message = Constants.ErrorInDelete };
        }

        public List<AccessGroup> GetAll()
        {
            return _accessRepository.GetAll().ToList();
        }

        public async Task<OrganizationPaged> GetAllPaginate(AccessGroupFilter filter)
        {

            var query = _accessRepository.GetAllTracking()                    
                    .Where(x => (filter.IdOrganization.HasValue && filter.IdOrganization.Value == x.IdOrganization) && (filter.Status == null || filter.Status == x.IsActive)).AsQueryable();

            if (!string.IsNullOrEmpty(filter.Search))
                query = query.Where(x =>
                    
                    x.Name.ToLower().Contains(filter.Search.ToLower()) ||
                    x.Description.ToLower().Contains(filter.Search.ToLower())                  
                    
                ).AsQueryable();

            if (!string.IsNullOrEmpty(filter.Description))
                query = query.Where(x =>
                    x.Description.ToLower().Contains(filter.Description.ToLower()) ||
                    x.Name.ToLower().Contains(filter.Description.ToLower()) 
                ).AsQueryable();

            var resultado = query.AsQueryable().AsNoTracking().OrderBy(p => p.Name).ToList();
            var queryResult = _mapper.Map<List<AccessGroupDto>>(resultado);
           
            return new OrganizationPaged
            {
                AccessGroupReturnedSet = queryResult.Skip(filter.pageSize * filter.pageIndex).Take(filter.pageSize).ToList(),
                NextPage = (filter.pageSize * filter.pageIndex) >= queryResult.Count() ? null : (int?)filter.pageIndex + 1,
                Page = filter.pageIndex,
                Total = queryResult.Count() 
            };
        }
       
        public async Task<AccessGroup> GetById(long id)
        {
            try
            {
                return await _accessRepository.FindBy(x => x.Id == id)
                    .AsNoTracking()                    
                    .Include(x => x.AccessGroupUsers)
                    .FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        public async Task<List<DropDownList>> GetAccessGroupsAvailable()
        {
            return await _accessRepository.FindBy(x => x.IsActive == true)
                   .Select(x => new DropDownList()
                   {
                       Id = x.Id.Value,
                       Description = x.Name
                   }).ToListAsync();
        }

        public ResultReturned Update(long id, AccessGroup acGp)
        {
            AccessGroup acGpDb = _accessRepository.GetById(id);

            if (acGpDb is not null)
            {
                acGpDb.Description = acGp.Description;
                acGpDb.Name = acGp.Name;
                acGpDb.UpdateTime = DateTime.Now;

                _accessRepository.Update(acGpDb);
                _accessRepository.SaveChanges();
                return new ResultReturned() { Result = true, Message = Constants.SuccessInUpdate };
            }

            return new ResultReturned() { Result = true, Message = Constants.ErrorInUpdate };
        }

        public bool ExistByAccessGroup(string description)
        {
            return _accessRepository.Exist(x => x.IsActive == true && x.Description == description.ToUpper().Trim());
        }

        public async Task<ResultReturned> AddMemberUser(UserFilter userFilter)
        {

            var access = _accessRepository.FindBy(x => x.IsActive == true && x.Id == userFilter.AccessGroups[0])
                    .AsNoTracking()                   
                    .Include(x => x.AccessGroupUsers)
                    .FirstOrDefault();

            if (access == null)
                throw new Exception("Grupo de Acesso não encontrado");

            var user = _userService.GetByLogin(userFilter.Login);
            
            if (user != null && user.Id.HasValue)
            {               
                if(!access.AccessGroupUsers.Any() || !access.AccessGroupUsers.Where(u => u.IdUser == user.Id).Any())
                {
                    var acGpUsers = new AccessGroupUser() { IdUser = user.Id.Value, IdAccessGroup = access.Id.Value };
                    
                    _accessGUserRepository.Add(acGpUsers);
                    _accessGUserRepository.SaveChanges();
                }
            }
            
            return new ResultReturned() { Result = true, Message = Constants.ErrorInUpdate };
        }

        public async Task<UserPaged> GetMemberUsers(UserFilter userFilter)
        {
            var users = _userService.GetAllPaginate(userFilter);
            return users;
        }

        public async Task<ResultReturned> DeleteMemberUsers(long idUser, long idAccessGroup)
        {
            AccessGroupUser acGp = _accessGUserRepository.FindBy(x=>x.IdUser == idUser && x.IdAccessGroup == idAccessGroup).FirstOrDefault();

            if (acGp is not null)
            {

                _accessGUserRepository.Remove(acGp);
                _accessGUserRepository.SaveChanges();
                return new ResultReturned() { Result = true, Message = Constants.SuccessInDelete };
            }

            return new ResultReturned() { Result = false, Message = Constants.ErrorInDelete };
        }

        public async Task DeleteMembersUsersOrganization(long idUser, long idOrganization)
        {
            List<AccessGroup> groups = await _accessRepository.FindBy(x => x.IdOrganization == idOrganization)
                    .AsNoTracking()
                    .Include(x => x.AccessGroupUsers).Where(a => a.AccessGroupUsers.Any(u => u.IdUser == idUser)).ToListAsync();

            if (groups is not null && groups.Any())
            {
                foreach (var group in groups)
                {
                    _accessGUserRepository.RemoveRange(group.AccessGroupUsers);
                    _accessGUserRepository.SaveChanges();

                }                
            }
        }
    }
}
