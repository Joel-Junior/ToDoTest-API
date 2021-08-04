using TodosProject.Domain.Entities;
using TodosProject.Infra.Data.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TodosProject.Infra.Data.Context
{
    public class TodosProjectContext : DbContext
    {
        public TodosProjectContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = typeof(TodosProjectContext).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new ProfileMapping());
            modelBuilder.ApplyConfiguration(new RoleMapping());
            modelBuilder.ApplyConfiguration(new ProfileRoleMapping());
            modelBuilder.ApplyConfiguration(new OrganizationMapping());
            modelBuilder.ApplyConfiguration(new AddressMapping());
            modelBuilder.ApplyConfiguration(new PhoneMapping());
            modelBuilder.ApplyConfiguration(new AccessGroupMapping());
            modelBuilder.ApplyConfiguration(new AccessGroupUserMapping());            
            modelBuilder.ExecuteSeed();
            base.OnModelCreating(modelBuilder);
        }

        public new DbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Profile> Profile { get; set; }
        public virtual DbSet<ProfileRole> ProfileRole { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Organization> Organization { get; set; }
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Phone> Phone { get; set; }
        public virtual DbSet<OrganizationUser> OrganizationUser { get; set; }
        public virtual DbSet<License> License { get; set; }
        public virtual DbSet<AccessGroup> AccessGroup { get; set; }
        public virtual DbSet<AccessGroupUser> AccessGroupUser { get; set; }

    }
}