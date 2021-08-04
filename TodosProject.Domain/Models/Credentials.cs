using TodosProject.Domain.Entities;
using TodosProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace TodosProject.Domain.Models
{
    public class Credentials
    {
        public long? Id { get; set; }
        public string Login { get; set; }
        public string Perfil { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }
        public EnumProfileType ProfileType { get; set; }
        public bool? IsADM => ProfileType.ToString().Equals("Admin");
        public Permission Permissions { get; set; }
    }
}
