using TodosProject.Domain.Dto;
using TodosProject.Domain.Entities;
using TodosProject.Domain.Filters;
using TodosProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TodosProject.Application.Interfaces
{
    public interface ILicenseService
    {
        Task<List<LicenseDto>> GetLicencesAvailableOrganization(long id);
        UserPaged GetAllPaginate(LicenseFilter filter);
        ResultReturned Add(License li);
        ResultReturned Update(long id, License org);
        ResultReturned Delete(long id, bool isDeletePhysical = false);
        Task<License> GetById(long id);
        ResultReturned AddMinutesMultiplayer(string chaveLicense, long multiplayerMinutes);
        List<License> GetAll();
    }
}
