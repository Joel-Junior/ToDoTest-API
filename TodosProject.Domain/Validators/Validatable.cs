using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TodosProject.Domain.Validators
{
    public abstract class Validatable
    {

        [JsonIgnore]
        public bool IsValid
        {
            get { return Errors == null; }
        }

        private List<string> Errors { get; set; }

        public void AddNotification(string message)
        {
            Errors = Errors ?? new List<string>();

            Errors.Add(message);
        }

        [JsonIgnore]
        public string Notification
        {
            get
            {
                var returnText = string.Empty;

                if (Errors != null && Errors.Count > 0)
                {
                    Errors.ForEach(er => returnText += $"{er} \n");
                }

                return returnText;
            }
        }
    }
}
