using System;
using System.Collections.Generic;
using System.Text;

namespace TodosProject.Domain.Models
{
    public class ResultReturned
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }
}
