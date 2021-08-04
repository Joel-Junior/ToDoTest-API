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
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TodosProject.Application.Services
{
    public class OrganizationService : IOrganizationService
    {
        public readonly IRepository<Organization> _orgRepository;
        private readonly IRepository<OrganizationUser> _organizationUserRepository;
        private readonly IUserService _userService;
        private readonly IAccessGroupService _accessService;
        protected readonly IMapper _mapper;
        private readonly string _url;
        public OrganizationService(IRepository<Organization> orgRepository, IRepository<OrganizationUser> organizationUserRepository, IGeneralService generalService, IMapper mapper,IUserService userService,IAccessGroupService accessService)
        {            
            _orgRepository = orgRepository;
            _mapper = mapper;
            _userService = userService;
            _organizationUserRepository = organizationUserRepository;
            _accessService = accessService;
        }

        public ResultReturned Add(Organization org)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(org.Name))
                    return new ResultReturned() { Result = false, Message = "Para realizar o cadastro, o campo nome deve ser preenchido" };

                else if (!ExistByOrganization(org.Name))
                {                   
                    org.CreatedTime = DateTime.Now;
                    org.IsActive = true;

                    _orgRepository.Add(org);
                    _orgRepository.SaveChanges();
                    return new ResultReturned() { Result = true, Message = Constants.SuccessInAdd , Data = (dynamic)org.Id };
                }

                return new ResultReturned() { Result = false, Message = $"Já existe uma organização {org.Name} cadastrado, troque de nome" };
            }
            catch (Exception ex)
            {
                return new ResultReturned() { Result = false, Message = Constants.ErrorInAdd };
            }
        }

        public ResultReturned Delete(long id, bool isDeletePhysical = false)
        {
            Organization org = _orgRepository.GetById(id);

            if (isDeletePhysical && org is not null)
                _orgRepository.Remove(org);

            else if (!isDeletePhysical && org is not null)
            {
                org.UpdateTime = DateTime.Now;
                org.IsActive = org.IsActive ? false : true;
                _orgRepository.Update(org);
                _orgRepository.SaveChanges();
                return new ResultReturned() { Result = true, Message = Constants.SuccessInDelete };
            }

            if (!string.IsNullOrEmpty(org.Logo))
            {
                var filePath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "organizations"), org.Logo);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    
                }
            }
            return new ResultReturned() { Result = false, Message = Constants.ErrorInDelete };
        }

        public List<Organization> GetAll()
        {
            return _orgRepository.GetAll().ToList();
        }

        public async Task<OrganizationPaged> GetAllPaginate(OrganizationFilter filter)
        {
            bool isAdmin = false;
            if(filter.user != null && filter.user.Id.HasValue)
            {
                isAdmin = _userService.GetById(filter.user.Id.Value).IsADM.Value; 
            }
            var query = _orgRepository.GetAllTracking()
                    .Include(x => x.Address)
                    .Include(x=>x.Phones)
                    .Include(x=>x.OrganizationUsers).ThenInclude(x=>x.Profile)
                    .Where(x => !isAdmin ? (filter.user != null && filter.user.Id.HasValue && x.OrganizationUsers.Any(x=>x.IdUser == filter.user.Id.Value)):true && (filter.Status == null || filter.Status == x.IsActive)).AsQueryable();

            if (!string.IsNullOrEmpty(filter.Search))
                query = query.Where(x =>
                    
                    x.Name.ToLower().Contains(filter.Search.ToLower()) ||
                    x.Description.ToLower().Contains(filter.Search.ToLower()) ||                    
                    x.Address.PublicPlace.ToLower().Contains(filter.Search.ToLower())
                ).AsQueryable();

            if (!string.IsNullOrEmpty(filter.Description))
                query = query.Where(x =>
                    x.Description.ToLower().Contains(filter.Description.ToLower()) ||
                    x.Name.ToLower().Contains(filter.Description.ToLower()) 
                ).AsQueryable();

            var resultado = query.AsQueryable().AsNoTracking().OrderBy(p => p.Name).ToList();
            var queryResult = _mapper.Map<List<OrganizationDto>>(resultado);
            queryResult.ForEach(x => x.IsManager = !isAdmin ? resultado.Any(j => j.OrganizationUsers.Any(u => u.IdOrganization == x.Id && u.IdUser == filter.user.Id.Value && u.Profile.ProfileType==EnumProfileType.Manager)):true);
            return new OrganizationPaged
            {
                OrganizationReturnedSet = queryResult.Count() > (filter.pageIndex) * filter.pageSize ? queryResult.Skip((filter.pageIndex) * filter.pageSize).Take(filter.pageSize).ToList() : queryResult.ToList(),
                NextPage = (filter.pageSize * filter.pageIndex) >= queryResult.Count() ? null : (int?)filter.pageIndex + 1,
                Page = filter.pageIndex,
                Total = queryResult.Count()
            };
        }
       
        public async Task<Organization> GetById(long id)
        {
            try
            {
                return await _orgRepository.FindBy(x => x.Id == id)
                    .AsNoTracking()
                    .Include(x => x.Address)
                    .Include(x => x.OrganizationUsers)
                    .Include(x=> x.Phones)
                    .FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        public async Task<List<DropDownList>> GetOrganizationsAvailable()
        {
            return await _orgRepository.FindBy(x => x.IsActive == true)
                   .Select(x => new DropDownList()
                   {
                       Id = x.Id.Value,
                       Description = x.Name
                   }).ToListAsync();
        }

        public ResultReturned Update(long id, Organization org)
        {
            Organization orgDb = _orgRepository.GetById(id);

            if (orgDb is not null)
            {
                orgDb.Description = org.Description;
                orgDb.Name = org.Name;
                orgDb.Logo = org.Logo;
                //orgDb.IdAddress = org.IdAddress;
                orgDb.UpdateTime = DateTime.Now;
                orgDb.Address = org.Address;
                orgDb.Phones = org.Phones;
                _orgRepository.Update(orgDb);
                _orgRepository.SaveChanges();
                return new ResultReturned() { Result = true, Message = Constants.SuccessInUpdate };
            }

            return new ResultReturned() { Result = true, Message = Constants.ErrorInUpdate };
        }

        public bool ExistByOrganization(string description)
        {
            return _orgRepository.Exist(x => x.IsActive == true && x.Description == description.ToUpper().Trim());
        }
        
        public async Task<ResultReturned> AddMemberUser(UserFilter userFilter)
        {

            var org = _orgRepository.FindBy(x => x.IsActive == true && x.Id == userFilter.Organizations[0])
                    .AsNoTracking()
                    .Include(x => x.Address)
                    .Include(x => x.OrganizationUsers)
                    .Include(x => x.Phones).FirstOrDefault();

            if (org == null)
                throw new Exception("Organização não encotrada");

            var user = _userService.GetByLogin(userFilter.Login);
            
            if (user != null && user.Id.HasValue)
            {               
                if(!org.OrganizationUsers.Any() || !org.OrganizationUsers.Where(u => u.IdUser == user.Id).Any())
                {
                    var orgUsers = new OrganizationUser() { IdUser = user.Id.Value, IdOrganization = org.Id.Value,IdProfile = userFilter.IdProfile.Value };
                    
                    _organizationUserRepository.Add(orgUsers);
                    _organizationUserRepository.SaveChanges();
                }
            }
            else
            {
                var userNew = new User() { Login = userFilter.Login,
                    IsAuthenticated = false,
                    IdProfile = 1,//id profile de usuário comum
                    Password = new ExtensionMethods().BuildPassword(8) };
                var result =  _userService.Add(userNew);
                if (!result.Result)
                    throw new Exception("Erro ao Salvar Usuário");
                var orguser = new OrganizationUser() { IdUser = result.Data, IdOrganization = org.Id.Value,IdProfile = userFilter.IdProfile.Value };
                _organizationUserRepository.Add(orguser);
                _organizationUserRepository.SaveChanges();
            }
            return new ResultReturned() { Result = true, Message = Constants.ErrorInUpdate };
        }

        public async Task<UserPaged> GetMemberUsers(UserFilter userFilter)
        {
            var query = _organizationUserRepository.GetAllTracking()
                .Include(x => x.Profile)
                .Include(x => x.User).ThenInclude(x=>x.AccessGroupUsers)
                .Where(x=>x.IdOrganization == userFilter.Organizations[0]).AsQueryable();

            var queryResult = from p in query.AsQueryable()
                              orderby p.User.Login ascending
                              select new UserReturnedDto
                              {
                                  Id = p.User.Id,
                                  Login = p.User.Login,
                                  Name = p.User.Name,
                                  Phone = p.User.Phone,
                                  IsAuthenticated = p.User.IsAuthenticated,
                                  IsActive = p.User.IsActive,
                                  Password = "-",
                                  LastPassword = "-",
                                  Profile = p.Profile.ProfileType.GetDisplayName(),
                                  AccessGroupUsers = p.User.AccessGroupUsers,
                              };

            return new UserPaged
            {
                UserReturnedSet = queryResult.Count() > (userFilter.pageIndex) * userFilter.pageSize ? queryResult.Skip((userFilter.pageIndex) * userFilter.pageSize).Take(userFilter.pageSize).ToList() : queryResult.ToList(),
                NextPage = (userFilter.pageSize * userFilter.pageIndex) >= queryResult.Count() ? null : (int?)userFilter.pageIndex + 1,
                Page = userFilter.pageIndex,
                Total = queryResult.Count() > (userFilter.pageIndex + 1) * userFilter.pageSize ? (int)Math.Ceiling((decimal)queryResult.Count() / userFilter.pageSize) : queryResult.Count()
            };
           
        }

        public async Task<ResultReturned> DeleteMemberUsers(long idUser, long idOrganization)
        {
            OrganizationUser org = _organizationUserRepository.FindBy(x=>x.IdUser == idUser && x.IdOrganization == idOrganization).FirstOrDefault();

            if ( org is not null)
            {
                 await _accessService.DeleteMembersUsersOrganization(idUser,idOrganization);
                _organizationUserRepository.Remove(org);
                _organizationUserRepository.SaveChanges();
                return new ResultReturned() { Result = true, Message = Constants.SuccessInDelete };
            }

            return new ResultReturned() { Result = false, Message = Constants.ErrorInDelete };
        }

        public Task<OrganizationPaged> GetAccessGroups(AccessGroupFilter accessFilter)
        {
            var groups = _accessService.GetAllPaginate(accessFilter);
            return groups;
        }

        public Task<bool> CheckExistingOneManager(long idUser, long idOrganization)
        {
            return _organizationUserRepository.GetAllTracking()
                    .Include(x => x.Profile).Where(x => x.IdUser != idUser && x.IdOrganization == idOrganization && x.Profile.ProfileType == EnumProfileType.Manager ).AnyAsync();

        }
        
    }
}
