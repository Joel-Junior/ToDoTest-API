using TodosProject.Domain.Base;
using TodosProject.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace TodosProject.Domain.Models
{
    public class OrganizationPaged : BasePaged
    {
        public List<OrganizationDto> OrganizationReturnedSet { get; set; }
        public List<AccessGroupDto> AccessGroupReturnedSet { get; set; }
    }
}
