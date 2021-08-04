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
    public class UserService : IUserService
    {
        public readonly IRepository<User> _userRepository;
        public readonly IRepository<OrganizationUser> _orgUserRepository;
        public readonly IRepository<AccessGroupUser> _accUserRepository;
        private readonly IGeneralService _generalService;
        private readonly IOrganizationService _orgService;
        private readonly string _url;
        public UserService(IRepository<User> userRepository, IRepository<OrganizationUser> orgUserRepository,
            IRepository<AccessGroupUser> accUserRepository,
            AppSettingsConfig appSettingsConfig, IGeneralService generalService)
        {
            _generalService = generalService;
            _userRepository = userRepository;
            _url = appSettingsConfig.LinkApplication;
            _orgUserRepository = orgUserRepository;
            _accUserRepository = accUserRepository;
        }

        public List<User> GetAll()
        {
            var query = _userRepository.GetAllTracking().Include(x => x.Profile).Include(x => x.OrganizationUsers)
                .Include(x => x.AccessGroupUsers)
                .AsQueryable().Select(p=>
                new User
                {
                    Id = p.Id,
                    Login = p.Login,
                    Name = p.Name,
                    Phone = p.Phone,
                    IsAuthenticated = p.IsAuthenticated,
                    IsActive = p.IsActive,
                    Password = "-",
                    LastPassword = "-",
                    Profile = p.Profile,
                    OrganizationUsers = p.OrganizationUsers,
                    AccessGroupUsers = p.AccessGroupUsers,
                }
                ).ToList();

            return query;//_userRepository.GetAll().ToList();
        }

        public UserPaged GetAllPaginate(UserFilter filter)
        {
            var query = GetAllUsers(filter);

            List<UserReturnedDto> queryResult = new List<UserReturnedDto>();
            if (!string.IsNullOrEmpty(filter.Search))
            {
                var resultado = query.AsQueryable().AsNoTracking().AsEnumerable()
                    .Where(x =>
                    (x.Phone != null ? x.Phone.ToString().Contains(filter.Search) :false) ||
                    x.Login.Contains(filter.Search.ToLower()) ||
                    x.Profile.ProfileType.GetDisplayName().ToLower().Contains(filter.Search.ToLower()) ||
                   
                    (x.Name != null ? x.Name.ToString().ToLower().Contains(filter.Search.ToLower()) : false )
                
                ).ToList();
                queryResult = (from p in resultado
                                                      orderby p.Login ascending
                                                      select new UserReturnedDto
                                                      {
                                                          Id = p.Id,
                                                          Login = p.Login,
                                                          Name = p.Name,
                                                          Phone = p.Phone,
                                                          IsAuthenticated = p.IsAuthenticated,
                                                          IsActive = p.IsActive,
                                                          Password = "-",
                                                          LastPassword = "-",
                                                          Profile = p.Profile.ProfileType.GetDisplayName(),
                                                          OrganizationUsers = p.OrganizationUsers,
                                                          AccessGroupUsers = p.AccessGroupUsers,
                                                      }).ToList();
            }
            else
                queryResult = (from p in query.AsQueryable()
                                                      orderby p.Login ascending
                                                      select new UserReturnedDto
                                                      {
                                                          Id = p.Id,
                                                          Login = p.Login,
                                                          Name = p.Name,
                                                          Phone = p.Phone,
                                                          IsAuthenticated = p.IsAuthenticated,
                                                          IsActive = p.IsActive,
                                                          Password = "-",
                                                          LastPassword = "-",
                                                          Profile = p.Profile.ProfileType.GetDisplayName(),
                                                          OrganizationUsers = p.OrganizationUsers,
                                                          AccessGroupUsers = p.AccessGroupUsers,
                                                      }).ToList();

            return new UserPaged
            {
                UserReturnedSet = queryResult.Count() > (filter.pageIndex) * filter.pageSize ? queryResult.Skip((filter.pageIndex) * filter.pageSize).Take(filter.pageSize).ToList() : queryResult.ToList(),
                NextPage = (filter.pageSize * filter.pageIndex) >= queryResult.Count() ? null : (int?)filter.pageIndex + 1,
                Page = filter.pageIndex,
                Total = queryResult.Count()
            };
        }

        public UserReturnedDto GetById(long id)
        {
            return (from p in _userRepository.FindBy(x => x.Id == id).AsQueryable()
                    orderby p.Login ascending
                    select new UserReturnedDto
                    {
                        Id = p.Id,
                        Login = p.Login,
                        Name = p.Name,
                        Phone = p.Phone,
                        IsAuthenticated = p.IsAuthenticated,
                        IsActive = p.IsActive,
                        Password = "-",
                        LastPassword = "-",
                        IdProfile = p.IdProfile,
                        Profile = p.Profile.ProfileType.GetDisplayName()
                    }).FirstOrDefault();
        }

        public UserReturnedDto GetByLogin(string login)
        {
            if (!string.IsNullOrEmpty(login))
            {
                return (from p in _userRepository.FindBy(x => x.Login.ToUpper().Trim() == login.ToUpper().Trim()).AsQueryable()
                        orderby p.Login ascending
                        select new UserReturnedDto
                        {
                            Id = p.Id,
                            Login = p.Login,
                            Name = p.Name,
                            Phone = p.Phone,
                            IsAuthenticated = p.IsAuthenticated,
                            IsActive = p.IsActive,
                            Password = "-",
                            LastPassword = "-",
                            Profile = p.Profile.ProfileType.GetDisplayName()
                        }).FirstOrDefault();
            }

            return null;
        }

        public List<DropDownList> GetUsers()
        {
            return _userRepository.FindBy(x => x.IsActive == true && x.IsAuthenticated == true)
                   .Select(x => new DropDownList()
                   {
                       Id = x.Id.Value,
                       Description = x.Login
                   }).ToList();
        }

        public bool ExistById(int id)
        {
            return _userRepository.Exist(x => x.IsActive == true && x.Id == id);
        }

        public bool ExistByLogin(string Login)
        {
            return _userRepository.Exist(x => x.IsActive == true && x.Login == Login.ToUpper().Trim());
        }

        public ResultReturned Add(User user)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(user.Login) && string.IsNullOrWhiteSpace(user.Password))
                    return new ResultReturned() { Result = false, Message = "Para realizar o login, o campo login e senha deve ser preenchido" };

                else if (!ExistByLogin(user.Login))
                {
                    //user.Password = new HashingManager().HashToString(user.Password);
                    user.CreatedTime = DateTime.Now;
                    user.IsActive = true;

                    //ENVIA E-MAIL                    
                    var html = HtmlToEmail(user.Login, user.Login, user.Password);
                    _generalService.sendEmail(new EmailConfig("Acesso o Sistema - Xr Proj", user.Login, "", html, user.Name));
                    

                    _userRepository.Add(user);
                    _userRepository.SaveChanges();
                    return new ResultReturned() { Result = true, Message = Constants.SuccessInAdd,Data = user.Id } ;
                }

                return new ResultReturned() { Result = false, Message = $"Já existe um login {user.Login} cadastrado, troque de login" };
            }
            catch (Exception ex)
            {
                return new ResultReturned() { Result = false, Message = Constants.ErrorInAdd };
            }
        }

        public ResultReturned Update(long id, User user)
        {
            User userDb = _userRepository.GetById(id);

            if (userDb is not null)
            {
                userDb.Login = user.Login;
                userDb.Name = user.Name;
                userDb.IdProfile = user.IdProfile;
                userDb.Phone = user.Phone;
                //userDb.Password = new HashingManager().HashToString(user.Password);
                userDb.UpdateTime = DateTime.Now;
                _userRepository.Update(userDb);
                _userRepository.SaveChanges();
                return new ResultReturned() { Result = true, Message = Constants.SuccessInUpdate };
            }

            return new ResultReturned() { Result = true, Message = Constants.ErrorInUpdate };
        }

        public async Task<ResultReturned> Delete(long id, bool isDeletePhysical = false)
        {
            User user = await _userRepository.GetAllTracking().Include(x => x.OrganizationUsers) 
                .Include(ac=>ac.AccessGroupUsers)
                .Where(u=>u.Id == id).FirstOrDefaultAsync();

            if(user is not null)
            {
                _orgUserRepository.RemoveRange(user.OrganizationUsers);
                _orgUserRepository.SaveChanges();
                _accUserRepository.RemoveRange(user.AccessGroupUsers);
                _accUserRepository.SaveChanges();

                if (!isDeletePhysical)
                {
                    user.UpdateTime = DateTime.Now;
                    user.IsActive = user.IsActive ? false : true;
                    _userRepository.Update(user);
                    _userRepository.SaveChanges();                    
                }
                else _userRepository.Remove(user);

                return new ResultReturned() { Result = true, Message = Constants.SuccessInDelete };
            }
            else             
                return new ResultReturned() { Result = false, Message = Constants.ErrorInDelete };
        }

        private IQueryable<User> GetAllUsers(UserFilter filter)
        {            
            return _userRepository.GetAllTracking().Include(x => x.Profile).Include(x=>x.OrganizationUsers)
                .Include(x => x.AccessGroupUsers)
                .Where(GetPredicate(filter)).AsQueryable();
        }

        private Expression<Func<User, bool>> GetPredicate(UserFilter filter)
        {
            return p =>
                   (string.IsNullOrWhiteSpace(filter.Login) || p.Login.Trim().ToUpper().Contains(filter.Login.Trim().ToUpper()))
                   &&
                   (!filter.IsAuthenticated.HasValue || filter.IsAuthenticated == p.IsAuthenticated)
                   &&
                   (!filter.Status.HasValue || filter.Status == p.IsActive)
                   &&
                   (string.IsNullOrEmpty(filter.Name) || p.Name.Trim().ToUpper().Contains(filter.Name.Trim().ToUpper()))
                   &&
                   (!filter.IsActive.HasValue || filter.IsActive == p.IsActive)
                   &&
                   (!filter.IdProfile.HasValue || filter.IdProfile == p.IdProfile)
                    &&
                   ( (filter.Organizations == null || !filter.Organizations.Any()) || p.OrganizationUsers.Where(x => x.IdOrganization == filter.Organizations[0]).Any())
                   &&
                   ( (filter.AccessGroups == null || !filter.AccessGroups.Any()) || p.AccessGroupUsers.Where(x => x.IdAccessGroup == filter.AccessGroups[0]).Any())
                   ;
        }


        public async Task<bool> VerifyExistsLogin(string login, long? id)
        {
            try
            {
                if (id.HasValue)
                    return await _userRepository.FindBy(x => x.IsActive && x.Login.ToUpper().Trim() == login.ToUpper().Trim() && x.Id != id.Value)
                            .AsQueryable().AnyAsync();
                else
                    return await _userRepository.FindBy(x => x.IsActive && x.Login.ToLower() == login.ToLower()).AsQueryable().AnyAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> VerifyExistsEmail(string email, long? id)
        {
            try
            {
                if (id.HasValue)
                    return await _userRepository.FindBy(x => x.IsActive && x.Login.ToLower() == email.ToLower() && x.Id != id.Value).AsQueryable().AnyAsync();
                else
                    return await _userRepository.FindBy(x => x.IsActive && x.Login.ToLower() == email.ToLower()).AsQueryable().AnyAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> InactivateActivateUser(long userID)
        {
            try
            {
                var user = await _userRepository.FindBy(x => x.Id == userID)?.FirstOrDefaultAsync();

                if (user != null)
                {
                    user.IsActive = !user.IsActive;
                    
                    user.UpdateTime = DateTime.Now;
                    _userRepository.Update(user);
                    _userRepository.SaveChanges();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdatePassword(long id, string password, string lastpassword)
        {
            try
            {
                var user = await _userRepository.GetAllTracking().AsQueryable().Where(x => x.Id == id).FirstOrDefaultAsync();

                if (user != null)
                {                    
                    if (!new HashingManager().Verify(lastpassword, user.Password))
                        return false;

                    user.LastPassword = user.Password;
                    user.Password = new HashingManager().HashToString(password);
                    user.UpdateTime = DateTime.Now;

                    _userRepository.Update(user);
                    _userRepository.SaveChanges();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string HtmlToEmail(string name, string email, string hash)
        {
            var html = string.Empty;

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TemplateEmail", "acesso_sistema.html");

            if (File.Exists(filePath))
            {
                html = File.ReadAllText(filePath);
                html = html.Replace("%NAME%", name);
                html = html.Replace("%EMAIL%", email);
                html = html.Replace("%LOGIN%", email);
                html = html.Replace("%PASSWORD%", hash);
                html = html.Replace("%LINK%", $"{_url}/#/login");
            }

            return html;
        }
    }
}
