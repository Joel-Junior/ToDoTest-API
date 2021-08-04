using TodosProject.Domain.Entities;
using TodosProject.Domain.Enums;
using TodosProject.Infra.CrossCutting;
using TodosProject.Infra.Data.Context;
using TodosProject.Infra.Data.Seed;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TodosProject.Infra.Data
{
    public static class ModelBuilderExtensions
    {
        public static void ExecuteSeed(this ModelBuilder modelBuilder)
        {
            SeedProfile(modelBuilder);
            SeedRole(modelBuilder);
            SeedProfileRole(modelBuilder);
            SeedUser(modelBuilder);
        }

        private static void SeedProfile(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Profile>().HasData(
               new Profile() { Id = 1, Description = "Perfil Usuário", AccessGroup = EnumAccessGroup.Internal, LoginType = EnumLoginType.Cloud, CreatedTime = DateTime.Now, IsActive = true, ProfileType = EnumProfileType.User },
               new Profile() { Id = 2, Description = "Perfil Administrador", AccessGroup = EnumAccessGroup.Internal, LoginType = EnumLoginType.Cloud, CreatedTime = DateTime.Now, IsActive = true, ProfileType = EnumProfileType.Admin },
               new Profile() { Id = 3, Description = "Perfil Manager", AccessGroup = EnumAccessGroup.Internal, LoginType = EnumLoginType.Cloud, CreatedTime = DateTime.Now, IsActive = true, ProfileType = EnumProfileType.Manager }
            );
        }

        private static void SeedRole(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
               new Role() { Id = 1, Description = "Regra de acesso a tela de Auditoria", CreatedTime = DateTime.Now, RoleTag = "ROLE_AUDIT"  }
            );
        }

        private static void SeedProfileRole(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProfileRole>().HasData(
               new ProfileRole() { IdProfile = 1, IdRole = 1 }
            );
        }

        private static void SeedUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
              new User
              {
                  Id = 1,
                  Login = "admin@todosproject.com.br",
                  CreatedTime = DateTime.Now,
                  IsActive = true,
                  IsAuthenticated = true,
                  Password = new HashingManager().HashToString("123mudar"),
                  LastPassword = string.Empty,
                  IdProfile = 2
              }
            );
        }
    }
}