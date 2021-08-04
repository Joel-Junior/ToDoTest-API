using TodosProject.Domain.Base;
using TodosProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace TodosProject.Domain.Dto
{
    public class AccessGroupDto : BaseDto
    {
        [DisplayName("Nome")]
        public string Name { get; set; }
        [DisplayName("Descrição")]
        public string Description { get; set; }        
        public long IdOrganization { get; set; }
        public OrganizationDto Organization { get; set; }
        //public List<UserReturnedDto> AccessGroupUsers { get; set; }
    }

}
