using TodosProject.Domain.Base;
using System.Collections.Generic;

namespace TodosProject.Domain.Filters
{
    public class LicenseFilter : BaseFilter
    {
        public long? Id { get; set; }
        public long? IdOrganization { get; set; }
        public string CodeID { get; set; }
    }
}
