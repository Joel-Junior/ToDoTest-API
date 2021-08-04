using TodosProject.Domain.Base;
using TodosProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TodosProject.Domain.Dto
{
    public class UserReturnedDto : BaseDto
    {
        [DisplayName("E-mail")]
        public string Login { get; set; }
        public string Password { get; set; }
        public string LastPassword { get; set; }
        [DisplayName("Autenticado")]
        public bool IsAuthenticated { get; set; }
        public long IdProfile { get; set; }
        [DisplayName("Perfil")]
        public string Profile { get; set; }
        public bool? IsADM => Profile?.Equals("Admin");
        public override string ToString() => $"Login: {Login}";
        public Permission permission { get; set; }
        [DisplayName("Nome")]
        public string Name { get; set; }
        [DisplayName("Telefone")]
        public string Phone { get; set; }
        public List<OrganizationUser> OrganizationUsers { get; set; }
        public List<AccessGroupUser> AccessGroupUsers { get; set; }
    }

    public class UserSendDto : BaseDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string LastPassword { get; set; }
        public bool IsAuthenticated { get; set; }
        public long IdProfile { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public override string ToString() => $"Login: {Login}";
    }
}
