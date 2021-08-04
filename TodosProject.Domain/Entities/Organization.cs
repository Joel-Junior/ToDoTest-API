using System;
using System.Collections.Generic;
using TodosProject.Domain.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodosProject.Domain.Entities
{
    public class Organization : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }        
        public string Logo { get; set; }
        public long? IdAddress { get; set; }
        public Address Address { get; set; }
        public List<Phone> Phones { get; set; }
        public List<OrganizationUser> OrganizationUsers { get; set; } = new List<OrganizationUser>();
        public List<License> Licenses { get; set; } = new List<License>();
        public List<AccessGroup> AccessGroups { get; set; } = new List<AccessGroup>();
    }

    public class Phone
    {        
        public long? Id { get; set; }
        public long IdOrganization { get; set; }
        public string Number { get; set; }
        public Organization Organization { get; set; }
    }
}
