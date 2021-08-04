using System;
using System.Collections.Generic;
using System.Text;

namespace TodosProject.Domain.Entities
{
    public class OrganizationUser
    {
        public long IdOrganization { get; set; }
        public long IdUser { get; set; }
        public long IdProfile { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual User User { get; set; }
        public virtual Profile Profile { get; set; }
    }
}
