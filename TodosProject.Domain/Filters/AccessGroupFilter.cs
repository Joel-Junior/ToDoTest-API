using TodosProject.Domain.Base;

namespace TodosProject.Domain.Filters
{
    public class AccessGroupFilter : BaseFilter
    {        
        public string Description { get; set; }
        public string Name { get; set; }
        public long? Id { get; set; }
        public long? IdOrganization { get; set; }
        public UserFilter user { get; set; }
    }
}
