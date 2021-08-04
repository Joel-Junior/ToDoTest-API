using TodosProject.Domain.Base;
using TodosProject.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace TodosProject.Domain.Models
{
	public class UserPaged : BasePaged
	{
		#region Properties

		public List<UserReturnedDto> UserReturnedSet { get; set; }
		public List<ToDoDto> ToDoReturnedSet { get; set; }
		public List<LicenseDto> LicenseReturnedSet { get; set; }
		public List<AccessGroupDto> AccessGroupReturnedSet { get; set; }
		#endregion
	}
}
