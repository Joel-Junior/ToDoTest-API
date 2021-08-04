using TodosProject.Domain.Base;
using TodosProject.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace TodosProject.Domain.Models
{
    public class ToDoPaged : BasePaged
    {
        public List<ToDoDto> ToDoReturnedSet { get; set; }
    }
}
