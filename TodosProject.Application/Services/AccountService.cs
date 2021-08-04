using TodosProject.Application.Interfaces;
using TodosProject.Domain;
using TodosProject.Domain.Entities;
using TodosProject.Domain.Models;
using TodosProject.Infra.CrossCutting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TodosProject.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IGeneralService _generalService;
        private readonly string _url;
        public AccountService(IRepository<User> userRepository, IGeneralService generalService, AppSettingsConfig appSettingsConfig)
        {
            _userRepository = userRepository;
            _generalService = generalService;
            _url = appSettingsConfig.LinkApplication;
        }

        public ResultReturned CheckUserAuthentication(string login, string password)
        {
            try
            {
                User user = _userRepository.FindBy(x => x.Login.ToUpper().Trim() == login.ToUpper().Trim() && x.IsActive == true).FirstOrDefault();
                if (user is not null)
                {
                    if (new HashingManager().Verify(password, user.Password))
                        return new ResultReturned() { Result = true, Message = "Usuário OK" };

                    return new ResultReturned() { Result = false, Message = "Autenticação invalida" };
                }

                return new ResultReturned() { Result = false, Message = "Erro na validação" };
            }
            catch (Exception)
            {
                return new ResultReturned() { Result = false, Message = "Erro na validação" };
            }
        }

        public Credentials GetUserCredentials(string login)
        {
            Credentials credenciais = new Credentials();
            User user = _userRepository.GetAllTracking().Include("Profile.ProfileRoles.Role").Where(p => p.Login == login).FirstOrDefault();
            if (user != null)
            {
                credenciais.Id = user.Id;
                credenciais.Login = user.Login;
                credenciais.Perfil = user.Profile.Description;
                credenciais.ProfileType = user.Profile.ProfileType;
                credenciais.Roles = new List<string>() { };
                credenciais.Roles = user.Profile.ProfileRoles.Select(x => x.Role.RoleTag).ToList();
                credenciais.Permissions = new Permission();
                if(!user.IsAuthenticated)
                {
                    user.IsAuthenticated = true;
                    _userRepository.Update(user);
                    _userRepository.SaveChanges();
                }
            }

            return credenciais;
        }

        public ResultReturned ChangePassword(long id, User user)
        {
            User dbUser = _userRepository.GetById(id);
            if (dbUser != null)
            {
                if (new HashingManager().Verify(user.Password, dbUser.Password) && dbUser.Login.ToUpper() == user.Login.ToUpper())
                {
                    dbUser.LastPassword = dbUser.Password;
                    dbUser.Password = new HashingManager().HashToString(user.Password);
                    dbUser.IsAuthenticated = true;
                    dbUser.UpdateTime = DateTime.Now;
                    _userRepository.Update(dbUser);
                    _userRepository.SaveChanges();
                    return new ResultReturned() { Result = true, Message = Constants.SuccessInChangePassword };
                }
            }
            return new ResultReturned() { Result = false, Message = Constants.ErrorInChangePassword };
        }

        public ResultReturned ResetPassword(string login)
        {
            string password = "123mudar";

            if (!string.IsNullOrWhiteSpace(login))
            {
                User user = _userRepository.FindBy(x => x.Login == login.ToUpper().Trim()).FirstOrDefault();
                if (user is not null)
                {
                    user.LastPassword = user.Password;
                    user.Password = new HashingManager().HashToString("123mudar");
                    user.IsAuthenticated = false;
                    user.IsActive = true;
                    user.UpdateTime = DateTime.Now; 

                    //ENVIA E-MAIL
                    var html = HtmlToEmail(user.Login, user.Login, password, "recuperacao_senha.html");
                    _generalService.sendEmail(new EmailConfig("Reset de senha - Todos Project", user.Login, "", html, user.Login));

                    _userRepository.Update(user);
                    _userRepository.SaveChanges();
                    return new ResultReturned() { Result = true, Message = Constants.SuccessInResetPassword };
                }

                return new ResultReturned() { Result = false, Message = Constants.ErrorInResetPassword };
            }
            return new ResultReturned() { Result = false, Message = Constants.ErrorInResetPassword };
        }

        private string HtmlToEmail(string name, string email, string hash,string nameFileTemplate)
        {
            var html = string.Empty;

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TemplateEmail", nameFileTemplate);

            if (File.Exists(filePath))
            {
                html = File.ReadAllText(filePath);
                html = html.Replace("%NAME%", name);
                html = html.Replace("%EMAIL%", email);
                html = html.Replace("%LOGIN%", email);
                html = html.Replace("%PASSWORD%", hash);
                html = html.Replace("%LINK%", $"{_url}/#/login");
            }

            return html;
        }

        public ResultReturned Register(User user)
        {
            if (!string.IsNullOrWhiteSpace(user.Login))
            {
                User userOrig = _userRepository.FindBy(x => x.Login == user.Login.ToUpper().Trim()).FirstOrDefault();
                if (userOrig is null)
                {
                    string password = user.Password;
                    user.Password = new HashingManager().HashToString(user.Password);
                    user.IsAuthenticated = false;
                    user.IsActive = true;
                    user.IdProfile = 1;//usuario comum
                    user.CreatedTime = DateTime.Now;

                    
                    //ENVIA E-MAIL
                    var html = HtmlToEmail(user.Name,user.Login, "","acesso_sistema.html");
                    _generalService.sendEmail(new EmailConfig("Primeiro Acesso - Todos Project", user.Login, "", html, user.Name));
                    

                    _userRepository.Add(user);
                    _userRepository.SaveChanges();
                    return new ResultReturned() { Result = true, Message = Constants.SuccessInResetPassword };
                }
                else
                    return new ResultReturned() { Result = false, Message = "Usuário já existe com esse e-mail" };
            }
            return new ResultReturned() { Result = false, Message = Constants.ErrorInResetPassword };
        }
    }

}

