using TodosProject.Domain.Entities;
using TodosProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TodosProject.Application.Interfaces
{
    public interface IProfileService
    {
        Task<List<DropDownList>> GetProfilesAvailable(bool includeAdmin = false);
    }
}
