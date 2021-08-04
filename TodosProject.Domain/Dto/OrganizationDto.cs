using TodosProject.Domain.Base;
using TodosProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace TodosProject.Domain.Dto
{
    public class OrganizationDto : BaseDto
    {
        [DisplayName("Nome")]
        public string Name { get; set; }
        [DisplayName("Descrição")]
        public string Description { get; set; }
        
        public string Logo { get; set; }
        public string LogoUpload { get; set; }
        public long? IdAddress { get; set; }

        [DisplayName("Endereço")]
        public string AddressExtension => Address != null ? $"{Address.Cep} - {Address.PublicPlace}, {Address.Number} - {Address.Neighborhood} - {Address.City}/{Address.State}" : "";
        public long? IdUser { get; set; }

        public AddressDTO Address { get; set; }
        [DisplayName("Telefone")]
        public string PhonesExtension => Phones.Any() ? string.Join(" / ",Phones.Select(x=>x.Number)) : "";
        public List<PhoneDTO> Phones { get; set; }
        public List<AccessGroupDto> AccessGroups { get; set; }
        public bool IsManager { get; set; }
    }

    public class PhoneDTO
    {
        public long? Id { get; set; }
        public string Number { get; set; }
    }

    public class AddressDTO
    {
        public long? Id { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Cep { get; set; }
        public string PublicPlace { get; set; }
        public string Neighborhood { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
    }
}
