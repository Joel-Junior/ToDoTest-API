using TodosProject.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TodosProject.Domain.Dto
{
    public class LicenseDto : BaseDto
    {
        [DisplayName("Data de Ativação")]
        public DateTime? ActivationDate { get; set; }

        [DisplayName("Data de Expiração")]
        public DateTime? ExpirationDate { get; set; }

        [DisplayName("Chave da Licença")]
        public Guid CodeID { get; set; }

        [DisplayName("Minutos utilizados de multiplayer")]
        public long MultiplayerMinutes { get; set; }

        [DisplayName("Máximo de armazenamento de arquivos")]
        public int MaxFileCapacity { get; set; }
        public long IdOrganization { get; set; }
        [DisplayName("Organização")]
        public string OrganizationExtension => Organization != null ? $"{Organization.Name}" : "";
        public OrganizationDto Organization { get; set; }
    }
}
