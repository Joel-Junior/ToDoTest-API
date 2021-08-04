using TodosProject.Domain.Entities;
using TodosProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace TodosProject.Application.Interfaces
{
    public interface IAccountService
    {
        ResultReturned CheckUserAuthentication(string login, string password);
        Credentials GetUserCredentials(string login);
        ResultReturned ChangePassword(long id, User user);
        ResultReturned ResetPassword(string email);
        ResultReturned Register(User user);
    }
}
