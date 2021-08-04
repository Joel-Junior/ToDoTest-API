using TodosProject.Domain.Base;
using TodosProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TodosProject.Domain.Entities
{
    public class Profile : BaseEntity
    {
        public string Description { get; set; }
        public EnumProfileType ProfileType { get; set; }
        public EnumAccessGroup AccessGroup { get; set; }
        public EnumLoginType LoginType { get; set; }
        public List<User> Users { get; set; }
        public List<ProfileRole> ProfileRoles { get; set; }        
    }

    public class SubModulePermission
    {
        public string SubModule { get; set; }

        public long SubModuleID { get; set; }

        public bool Insert { get; set; }

        public bool Read { get; set; }

        public bool Edit { get; set; }

        public bool Delete { get; set; }

    }
    public class Module
    {
        public string Description { get; set; }

        public long ModuleID { get; set; }

        public List<SubModulePermission> SubModulePermissions { get; set; } = new List<SubModulePermission>();
    }
    public class Permission
    {
        public List<Module> Modules { get; set; } = new List<Module>();
    }
    
}
