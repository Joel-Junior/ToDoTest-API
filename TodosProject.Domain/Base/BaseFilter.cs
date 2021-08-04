using System;
using System.Collections.Generic;
using System.Text;

namespace TodosProject.Domain.Base
{
    public class BaseFilter
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public string Search { get; set; }
        public bool? Status { get; set; }
    }
}
