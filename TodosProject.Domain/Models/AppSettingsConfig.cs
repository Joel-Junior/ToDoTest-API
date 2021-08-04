using System;
using System.Collections.Generic;
using System.Text;

namespace TodosProject.Domain.Models
{
    public class AppSettingsConfig
    {
        public string LinkApplication { get; set; }

        public string VtexEndPoint { get; set; }

        public string VtexAppKey { get; set; }

        public string VtexAppToken { get; set; }

        public string LinkApiEngineIntegration { get; set; }

        public string Token { get; set; }
    }
}
