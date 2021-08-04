using System;
using System.Collections.Generic;
using TodosProject.Domain.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodosProject.Domain.Entities
{
    public class AccessGroup : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; } 
        public long IdOrganization { get; set; }
        public Organization Organization { get; set; }
        public List<AccessGroupUser> AccessGroupUsers { get; set; } = new List<AccessGroupUser>();

    }

}
