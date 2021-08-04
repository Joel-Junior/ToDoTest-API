using TodosProject.Domain.Base;
using System.Collections.Generic;

namespace TodosProject.Domain.Filters
{
    public class UserFilter : BaseFilter
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string LastPassword { get; set; }
        public bool? IsAuthenticated { get; set; }
        public long? IdProfile { get; set; }
        public bool? IsActive { get; set; }
        public string Name { get; set; }
        public long? Id { get; set; }
        public List<long> Organizations { get; set; }
        public List<long> AccessGroups { get; set; }
    }
}

