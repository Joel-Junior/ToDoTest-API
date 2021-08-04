using TodosProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TodosProject.Infra.Data.Mapping
{
    public class AccessGroupUserMapping : IEntityTypeConfiguration<AccessGroupUser>
    {
        private EntityTypeBuilder<AccessGroupUser> _builder;

        public void Configure(EntityTypeBuilder<AccessGroupUser> builder)
        {
            _builder = builder;
            _builder.ToTable("AccessGroupUsers");
            ConfigurePrimaryKey();
            ConfigureColumns();
            ConfigureForeignKeys();
            ConfigureIndexes();
        }

        private void ConfigureColumns()
        {
            _builder.Property(a => a.IdAccessGroup).IsRequired(true).HasColumnName("Id_AccessGroup");
            _builder.Property(a => a.IdUser).IsRequired(true).HasColumnName("Id_User");
        }

        private void ConfigureForeignKeys()
        {
            _builder.HasOne(a => a.AccessGroup)
                .WithMany(a => a.AccessGroupUsers)
                .HasForeignKey(a => a.IdAccessGroup);

            _builder.HasOne(a => a.User)
                .WithMany(a => a.AccessGroupUsers)
                .HasForeignKey(a => a.IdUser);
        }

        private void ConfigureIndexes()
        {
            _builder.HasIndex(a =>
                new
                {
                    a.IdAccessGroup,
                    a.IdUser
                })
                .IsUnique(true);
        }

        private void ConfigurePrimaryKey()
        {
            _builder.HasKey(a => new
            {
                a.IdAccessGroup,
                a.IdUser
            });
        }
    }
}
