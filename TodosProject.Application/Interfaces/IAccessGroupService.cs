using TodosProject.Domain.Dto;
using TodosProject.Domain.Entities;
using TodosProject.Domain.Filters;
using TodosProject.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodosProject.Application.Interfaces
{
    public interface IAccessGroupService
    {
        List<AccessGroup> GetAll();
        Task<OrganizationPaged> GetAllPaginate(AccessGroupFilter filter);
        Task<AccessGroup> GetById(long id);
        Task<List<DropDownList>> GetAccessGroupsAvailable();
        ResultReturned Add(AccessGroup org);
        ResultReturned Update(long id, AccessGroup access);
        bool ExistByAccessGroup(string description);
        ResultReturned Delete(long id, bool isDeletePhysical = false);        
        Task<ResultReturned> AddMemberUser(UserFilter userFilter);
        Task<UserPaged> GetMemberUsers(UserFilter userFilter);
        Task<ResultReturned> DeleteMemberUsers(long idUser,long idOrganization);
        Task DeleteMembersUsersOrganization(long idUser, long idOrganization);
    }
}
