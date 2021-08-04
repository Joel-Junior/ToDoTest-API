using TodosProject.Domain.Entities;
using TodosProject.Infra.CrossCutting;
using TodosProject.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TodosProject.Infra.Data.Seed
{
    public static class AllSeeds
    {
        public static void SeedUser(this TodosProjectContext context)
        {
            if (context.User.Any())
                return;

            context.Add(new User
            {
                Id = 1,
                Login = "admin@todosproject.com.br",
                CreatedTime = DateTime.Now,
                IsActive = true,
                IsAuthenticated = true,
                Password = new HashingManager().HashToString("123mudar") ,
                LastPassword = string.Empty
            });
        }
    }
}
