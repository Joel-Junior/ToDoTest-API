using System;
using TodosProject.Domain.Base;

namespace TodosProject.Domain.Filters
{
    public class ToDoFilter : BaseFilter
    {        
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public long? Id { get; set; }
        public UserFilter User { get; set; }
    }
}
