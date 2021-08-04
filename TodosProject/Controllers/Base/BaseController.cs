using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TodosProject.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TodosProject.Controllers.Base
{
    [Produces("application/json")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IMapper _mapper;
        protected readonly IGeneralService _generalService;

        protected BaseController(IMapper mapper, IGeneralService generalService)
        {
            _mapper = mapper;
            _generalService = generalService;
        }
        public int? UserId
        {
            get
            {
                var userClaim = User?.Claims?.FirstOrDefault(x => x.Type == "user.id");
                if (userClaim == null)
                {
                    return null;
                }

                if (int.TryParse(userClaim?.Value, out int userId))
                {
                    return userId;
                }

                return null;
            }
        }
    }
}
