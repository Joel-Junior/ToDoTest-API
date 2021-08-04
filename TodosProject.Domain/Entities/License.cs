using TodosProject.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace TodosProject.Domain.Entities
{
    public class License : BaseEntity
    {
        public DateTime ActivationDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public Guid CodeID { get; set; }

        public long MultiplayerMinutes { get; set; }

        public int MaxFileCapacity { get; set; }
        public long IdOrganization { get; set; }
        public Organization Organization { get; set; }        
    }
}
