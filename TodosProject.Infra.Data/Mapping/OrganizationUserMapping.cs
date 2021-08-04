using TodosProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TodosProject.Infra.Data.Mapping
{
    public class OrganizationUserMapping : IEntityTypeConfiguration<OrganizationUser>
    {
        private EntityTypeBuilder<OrganizationUser> _builder;

        public void Configure(EntityTypeBuilder<OrganizationUser> builder)
        {
            _builder = builder;
            _builder.ToTable("OrganizationUsers");
            ConfigurePrimaryKey();
            ConfigureColumns();
            ConfigureForeignKeys();
            ConfigureIndexes();
        }

        private void ConfigureColumns()
        {
            _builder.Property(a => a.IdOrganization).IsRequired(true).HasColumnName("Id_Organization");
            _builder.Property(a => a.IdUser).IsRequired(true).HasColumnName("Id_User");
            _builder.Property(a => a.IdProfile).IsRequired(true).HasColumnName("Id_Profile");
        }

        private void ConfigureForeignKeys()
        {
            _builder.HasOne(a => a.Organization)
                .WithMany(a => a.OrganizationUsers)
                .HasForeignKey(a => a.IdOrganization);

            _builder.HasOne(a => a.User)
                .WithMany(a => a.OrganizationUsers)
                .HasForeignKey(a => a.IdUser);

            _builder.HasOne(a => a.Profile).WithMany().HasForeignKey(fk => fk.IdProfile).OnDelete(DeleteBehavior.Restrict);


        }

        private void ConfigureIndexes()
        {
            _builder.HasIndex(a =>
                new
                {
                    a.IdOrganization,
                    a.IdUser
                })
                .IsUnique(true);
        }

        private void ConfigurePrimaryKey()
        {
            _builder.HasKey(a => new
            {
                a.IdOrganization,
                a.IdUser
            });
        }
    }
}
