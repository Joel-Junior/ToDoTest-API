using TodosProject.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TodosProject.Domain.Dto
{
    public class ToDoDto : BaseDto
    {
        [DisplayName("Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(int.MaxValue, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Description { get; set; }
        [DisplayName("Data de Vencimento")]
        public DateTime DueDate { get; set; }

        [DisplayName("Data de Conclusão")]
        public DateTime? ConcludeDate { get; set; }
        public long IdUser { get; set; }

        [DisplayName("Usuário")]
        public string UserExtension => User != null ? $"{User.Login}" : "";
        public UserReturnedDto User { get; set; }
        [DisplayName("Atrasado?")]
        public bool Late => DueDate < DateTime.Now;
    }

}
