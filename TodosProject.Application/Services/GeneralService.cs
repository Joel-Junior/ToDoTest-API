using TodosProject.Application.Interfaces;
using TodosProject.Domain;
using TodosProject.Domain.Enums;
using TodosProject.Domain.Models;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TodosProject.Application.Services
{
    public sealed class GeneralService : IGeneralService
    {
        private readonly IHttpContextAccessor _accessor;

        TokenConfiguration _tokenConfiguration { get; }
        EmailSettings _emailSettings { get; }

        public GeneralService(IHttpContextAccessor accessor, TokenConfiguration tokenConfiguration, EmailSettings emailSettings)
        {
            _accessor = accessor;
            _tokenConfiguration = tokenConfiguration;
            _emailSettings = emailSettings;
        }

        public long GetCurrentUserId()
        {
            if (!_accessor.HttpContext.User.Identity.IsAuthenticated)
                return 0;

            string userId = _accessor.HttpContext.User.FindFirst(x => x.Type == "Id")?.Value;
            return long.TryParse(userId, out _) ? long.Parse(userId) : 0;
        }

        public string GetCurrentUserName()
        {
            if (!_accessor.HttpContext.User.Identity.IsAuthenticated)
                return string.Empty;

            return _accessor.HttpContext.User.FindFirst(x => x.Type == "Name")?.Value ?? string.Empty;
        }

        public string CreateJwtToken(Credentials credentials)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenConfiguration.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _tokenConfiguration.Issuer,
                Audience = _tokenConfiguration.Audience,
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim("Id",credentials.Id.ToString()),
                        new Claim("Login", credentials.Login.ToString()),
                        new Claim("Roles", string.Join(",",credentials.Roles))
                }),
                Expires = DateTime.UtcNow.AddSeconds(_tokenConfiguration.Seconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<RequestData> RequestDataToExternalAPI(string url)
        {
            RequestData requestDataDto = new RequestData();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        requestDataDto.Data = await response.Content.ReadAsStringAsync();
                        requestDataDto.IsSuccess = true;
                        return requestDataDto;
                    }
                }
            }
            catch (Exception)
            {
                requestDataDto.Data = string.Format(Constants.ExceptionRequestAPI, url);
                requestDataDto.IsSuccess = false;
            }
            return requestDataDto;
        }

        public async Task<RequestData> RequestLogin(string url, string key = "")
        {
            RequestData requestDataDto = new RequestData();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.Timeout = TimeSpan.FromMinutes(1);
                    var stringContent = new StringContent(key, Encoding.UTF8, "application/xml");
                    HttpResponseMessage response = await client.PostAsync(url, stringContent);
                    if (response.IsSuccessStatusCode)
                    {
                        requestDataDto.Data = await response.Content.ReadAsStringAsync();
                        requestDataDto.IsSuccess = true;
                    }
                    else
                    {
                        requestDataDto.Data = string.Format(Constants.ExceptionRequestAPI, url);
                        requestDataDto.IsSuccess = false;
                    }
                    return requestDataDto;
                }
            }
            catch (Exception)
            {
                requestDataDto.Data = string.Format(Constants.ExceptionRequestAPI, url);
                requestDataDto.IsSuccess = false;
            }
            return requestDataDto;
        }

        public async Task sendEmail(EmailConfig emailConfig)
        {
            EmailSettings emailSettings = _emailSettings;
            emailSettings = new EmailSettings(emailSettings);
            emailConfig = new EmailConfig(emailSettings, emailConfig);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(emailConfig.emailFrom.Address);
            emailConfig.emailTo.Split(';').ToList().ForEach(p => email.To.Add(MailboxAddress.Parse(p.Trim())));
            email.Subject = GetEmailSubjectTemplate(emailConfig.enumEmailDisplay, emailConfig.enumEmailTemplate, emailConfig.emailSubject);
            email.Priority = MessagePriority.Urgent;
            email.Body = BuildMessage(emailConfig).Result;
            await executeMailWithMailKit(emailConfig, email);
        }

        private async Task executeMailWithMailKit(EmailConfig emailConfig, MimeMessage email)
        {
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect(emailConfig.emailSettings.PrimaryDomain, emailConfig.emailSettings.PrimaryPort);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(emailConfig.emailSettings.UsernameEmail, emailConfig.emailSettings.UserPassword);
                    await client.SendAsync(email);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
        private string GetEmailSubjectTemplate(EnumEmailDisplay enumEmailDisplay, EnumEmailTemplate enumEmailTemplate, string subject)
        {
            Dictionary<EnumEmailDisplay, string> dictionary = new Dictionary<EnumEmailDisplay, string>();
            dictionary.Add(EnumEmailDisplay.Padrao, $"Bem vindo ao sistema {enumEmailTemplate.GetDisplayName()}");
            dictionary.Add(EnumEmailDisplay.BoasVindas, $"Bem vindo ao sistema {enumEmailTemplate.GetDisplayName()}");
            dictionary.Add(EnumEmailDisplay.EsqueciSenha, $"{enumEmailTemplate.GetDisplayName()} - Esqueci a senha");
            dictionary.Add(EnumEmailDisplay.TrocaSenha, $"{enumEmailTemplate.GetDisplayName()} - Solicitação de troca de senha");
            dictionary.Add(EnumEmailDisplay.ConfirmacaoSenha, $"{enumEmailTemplate.GetDisplayName()} - Confirmação de senha");
            return dictionary[enumEmailDisplay];
        }
        private string GetEmailBodyTemplate(EnumEmailDisplay enumEmailDisplay, EnumEmailTemplate enumEmailTemplate, string body, string userName)
        {
            switch (enumEmailDisplay)
            {
                case EnumEmailDisplay.Padrao:
                    break;
                case EnumEmailDisplay.BoasVindas:
                    body = $"Olá, {userName}" + "<br>";
                    body += $"Seja bem vindo ao <b>{enumEmailTemplate.GetDisplayName()}</b>" + "<br> ";
                    body += $"Utilize a senha <b>{1234}</b> para acessar o sistema e usufrua de todas as ferramentas disponíveis." + "<br>";
                    break;
                case EnumEmailDisplay.EsqueciSenha:
                    body = $"<center>Olá, {userName}</center>";
                    body += $"<center>Conforme sua solicitação enviamos este email para que você possa concluir sua solicitação de esqueci a senha. Clique no botão abaixo.</center>" + "<br> ";
                    break;
                case EnumEmailDisplay.TrocaSenha:
                    body = $"<center>Olá, {userName}</center>";
                    body += $"<center>Conforme sua solicitação enviamos este email para que você possa concluir sua solicitação de troca de senha. Clique no botão abaixo.</center>" + "<br> ";
                    break;
                case EnumEmailDisplay.ConfirmacaoSenha:
                    body = $"<center>Olá, {userName}</center>";
                    body += $"<center>Quero reporta-lo que a sua confirmação de senha foi realizada com sucesso no periodo das {DateTime.Now.ToShortDateString()} - {DateTime.Now.ToShortTimeString()}</center>" + "<br> ";
                    break;
            }
            return body;
        }
        private async Task<MimeEntity> BuildMessage(EmailConfig emailConfig)
        {
            return new TextPart(TextFormat.Html)
            {
                Text = GetEmailBodyTemplate(emailConfig.enumEmailDisplay, emailConfig.enumEmailTemplate, emailConfig.emailBody, emailConfig.userName),
            };
        }
    }
}
