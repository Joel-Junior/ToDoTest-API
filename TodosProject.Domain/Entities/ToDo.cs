using TodosProject.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace TodosProject.Domain.Entities
{
    public class ToDo : BaseEntity
    {
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ConcludeDate { get; set; }
        public long IdUser { get; set; }
        public User User { get; set; }        
    }
}
