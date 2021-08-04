
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TodosProject.Domain.Enums
{
    public enum EnumEmailTemplate : byte
    {
        [Display(Name = "TodosProject")]
        TodosProject = 1
    }
}
