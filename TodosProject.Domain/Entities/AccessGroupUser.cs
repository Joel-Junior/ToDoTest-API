using System;
using System.Collections.Generic;
using System.Text;

namespace TodosProject.Domain.Entities
{
    public class AccessGroupUser
    {
        public long IdAccessGroup { get; set; }
        public long IdUser { get; set; }
        public virtual AccessGroup AccessGroup { get; set; }
        public virtual User User { get; set; }
    }
}
