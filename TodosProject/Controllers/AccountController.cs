using AutoMapper;
using TodosProject.Application.Interfaces;
using TodosProject.Controllers.Base;
using TodosProject.Domain.Dto;
using TodosProject.Domain.Entities;
using TodosProject.Domain.Enums;
using TodosProject.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodosProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private IAccountService _accountService;
        private IUserService _userService;
        public AccountController(IMapper mapper, IGeneralService generalService, IAccountService accountService,IUserService userService) : base(mapper, generalService)
        {
            _accountService = accountService;
            _userService = userService;
        }
                
        [HttpPost("v1/Login")]
        public IActionResult Login([FromBody] AuthenticationDTO login)
        {
            ResultReturned resultReturned = _accountService.CheckUserAuthentication(login.Login, login.Password);
            if (resultReturned.Result)
            {
                Credentials credentials = _accountService.GetUserCredentials(login.Login);
                credentials.Token = _generalService.CreateJwtToken(credentials);
                return Ok(new
                {
                    success = resultReturned.Result,
                    message = "Login efetuado com sucesso.",                    
                    profile = DefPermissions(credentials)
                });
            }
            else
            {
                return Unauthorized(new { success = false, message = "Login não autorizado." });
            }
        }

        [HttpPost("v1/ChangePassword")]
        public IActionResult ChangePassword([FromBody] User user)
        {
            ResultReturned resultReturned = _accountService.ChangePassword(_generalService.GetCurrentUserId(), user);
            if (resultReturned.Result)
                return Ok(new
                {
                    success = resultReturned.Result,
                    message = resultReturned.Message 
                });
            return BadRequest(new
            {
                success = resultReturned.Result,
                message = resultReturned.Message
            });
        }

        [HttpGet("v1/ResetPassword/{email}")]
        public IActionResult ResetPassword(string email)
        {
            ResultReturned resultReturned = null;
            try
            {
                resultReturned = _accountService.ResetPassword(email);
                if (resultReturned.Result)
                    return Ok(new
                    {
                        success = resultReturned.Result,
                        message = resultReturned.Message,
                    });
                else
                    return Ok(new
                    {
                        success = true,
                        message = resultReturned.Message,
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        private Credentials DefPermissions(Credentials credentials)
        {
            switch (credentials.ProfileType)
            {
                case EnumProfileType.User:
                    credentials.Permissions.Modules = new List<Module>() {
                        new Module { Description = "DASHBOARD", SubModulePermissions = null },
                        new Module { Description = "CADASTROS", SubModulePermissions = new List<SubModulePermission>(){
                                             new SubModulePermission { SubModule = "ORGANIZACOES", Insert = true,Read = true,Edit = true },
                                             new SubModulePermission { SubModule = "ACCESSGROUPS",Insert = false,Read = false,Edit = false,Delete = false },
                                                                } 
                                    },
                    };
                    break;
                case EnumProfileType.Admin:
                    credentials.Permissions.Modules = new List<Module>() {
                        new Module { Description = "DASHBOARD", SubModulePermissions = null },
                        new Module
                        {
                            Description = "CADASTROS",
                            SubModulePermissions = new List<SubModulePermission>(){
                                new SubModulePermission
                                {
                                    SubModule = "USUÁRIOS",Insert = true,Read = true,Edit = true,Delete = true,
                                },
                                new SubModulePermission
                                {
                                    SubModule = "ORGANIZACOES",Insert = true,Read = true,Edit = true,Delete = true,
                                },
                                new SubModulePermission
                                {
                                    SubModule = "LICENCAS",Insert = true,Read = true,Edit = true,Delete = true,
                                },
                                new SubModulePermission
                                {
                                    SubModule = "ACCESSGROUPS",Insert = true,Read = true,Edit = true,Delete = true,
                                },
                            }
                        },
                        
                    };
                    break;
                case EnumProfileType.Manager:
                    credentials.Permissions.Modules = new List<Module>() {
                        new Module { Description = "DASHBOARD", SubModulePermissions = null },
                        new Module { Description = "CADASTROS", SubModulePermissions = new List<SubModulePermission>(){
                                             new SubModulePermission { SubModule = "ORGANIZACOES", Insert = true,Read = true,Edit = true },
                                           } },
                      
                    };
                    break;
                default:
                    break;
            }

            return credentials;
        }

        [Route("v1/Register")]
        [HttpPost()]
        public async Task<IActionResult> Register([FromBody] UserSendDto model)
        {
            ResultReturned resultReturned = null;
            try
            {
                User user = _mapper.Map<User>(model);
                resultReturned = _accountService.Register(user);
                if (resultReturned.Result)
                    return Ok(new
                    {
                        success = resultReturned.Result,
                        message = resultReturned.Message,
                    });
                else
                    return Ok(new
                    {
                        success = false,
                        message = resultReturned.Message,
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }            
        }

        [HttpGet("v1/VerifyExistsEmail")]
        public async Task<IActionResult> VerifyExistsEmail(string email, long? id)
        {
            try
            {
                var result = await _userService.VerifyExistsEmail(email, id);
                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao verificar e-mail existente!");
            }
        }
    }
}
