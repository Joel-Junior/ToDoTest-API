using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TodosProject.Domain.Enums
{
    public enum EnumAccessGroup : byte
    {
        [Display(Name = "Internal")]
        Internal = 0,

        [Display(Name = "Externo")]
        Externo = 1,
    }
}
