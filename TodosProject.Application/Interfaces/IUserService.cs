using TodosProject.Domain.Dto;
using TodosProject.Domain.Entities;
using TodosProject.Domain.Filters;
using TodosProject.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodosProject.Application.Interfaces
{
    public interface IUserService
    {
        List<User> GetAll();
        UserPaged GetAllPaginate(UserFilter filter);
        UserReturnedDto GetById(long id);
        UserReturnedDto GetByLogin(string login);
        List<DropDownList> GetUsers();
        ResultReturned Add(User user);
        ResultReturned Update(long id, User user);
        Task<ResultReturned> Delete(long id, bool isDeletePhysical = false);
        Task<bool> InactivateActivateUser(long userID);        
        Task<bool> VerifyExistsLogin(string login, long? id);
        Task<bool> VerifyExistsEmail(string email, long? id);
        Task<bool> UpdatePassword(long id, string password, string lastpassword);

    }
}
