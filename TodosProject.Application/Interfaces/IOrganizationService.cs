using TodosProject.Domain.Dto;
using TodosProject.Domain.Entities;
using TodosProject.Domain.Filters;
using TodosProject.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodosProject.Application.Interfaces
{
    public interface IOrganizationService
    {
        List<Organization> GetAll();
        Task<OrganizationPaged> GetAllPaginate(OrganizationFilter filter);
        Task<Organization> GetById(long id);
        Task<List<DropDownList>> GetOrganizationsAvailable();
        ResultReturned Add(Organization org);
        ResultReturned Update(long id, Organization org);
        ResultReturned Delete(long id, bool isDeletePhysical = false);
        Task<ResultReturned> AddMemberUser(UserFilter userFilter);
        Task<UserPaged> GetMemberUsers(UserFilter userFilter);
        Task<ResultReturned> DeleteMemberUsers(long idUser,long idOrganization);
        Task<OrganizationPaged> GetAccessGroups(AccessGroupFilter accessFilter);
        Task<bool> CheckExistingOneManager(long idUser, long idOrganization);
    }
}
