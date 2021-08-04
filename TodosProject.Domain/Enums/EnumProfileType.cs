using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TodosProject.Domain.Enums
{
    public enum EnumProfileType : byte
    {
        [Display(Name = "User")]
        User = 0,

        [Display(Name = "Admin")]
        Admin = 1,

        [Display(Name = "Manager")]
        Manager = 2,
    }
}