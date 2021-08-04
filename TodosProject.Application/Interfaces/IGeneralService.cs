using TodosProject.Domain.Enums;
using TodosProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TodosProject.Application.Interfaces
{
    public interface IGeneralService
    {
        long GetCurrentUserId();
        string GetCurrentUserName();
        string CreateJwtToken(Credentials credentials);
        Task<RequestData> RequestDataToExternalAPI(string url);
        Task<RequestData> RequestLogin(string url, string key = "");
        Task sendEmail(EmailConfig emailConfig);
    }
}
