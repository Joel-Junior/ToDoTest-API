using TodosProject.Domain.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodosProject.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string LastPassword { get; set; }
        public bool IsAuthenticated { get; set; }
        public long IdProfile { get; set; }
        public Profile Profile { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        [NotMapped]
        public Permission Permissions { get; set; } = new Permission();
        [NotMapped]
        public string NewPassword { get; set; }

        public override string ToString() => $"Login: {Login}";

        public List<OrganizationUser> OrganizationUsers { get; set; } = new List<OrganizationUser>();
        public List<AccessGroupUser> AccessGroupUsers { get; set; } = new List<AccessGroupUser>();  

    }
}
