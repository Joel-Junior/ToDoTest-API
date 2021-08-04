using TodosProject.Domain.Base;

namespace TodosProject.Domain.Filters
{
    public class OrganizationFilter : BaseFilter
    {        
        public string Description { get; set; }
        public long? Id { get; set; }
        public UserFilter user { get; set; }
    }
}
