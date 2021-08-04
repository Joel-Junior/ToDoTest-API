using TodosProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TodosProject.Infra.Data.Mapping
{
    public class OrganizationMapping : IEntityTypeConfiguration<Organization>
    {
        private EntityTypeBuilder<Organization> _builder;

        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            _builder = builder;
            _builder.ToTable("Organizations");
            ConfigureColumns();
            ConfigureForeignKeys();
            ConfigureIndexes();
        }

        private void ConfigureForeignKeys()
        {            
            _builder.HasOne(a => a.Address).WithMany().HasForeignKey(fk => fk.IdAddress).OnDelete(DeleteBehavior.Restrict);

            // 1 : N => Organizaton : Licenses
            _builder.HasMany(o => o.Licenses)
                .WithOne(p => p.Organization)
                .HasForeignKey(p => p.IdOrganization);

            // 1 : N => Organizaton : Phones
            _builder.HasMany(o => o.Phones)
                .WithOne(p => p.Organization)
                .HasForeignKey(p => p.IdOrganization);

            // 1 : N => Organizaton : AccessGroups
            _builder.HasMany(o => o.AccessGroups)
                .WithOne(p => p.Organization)
                .HasForeignKey(p => p.IdOrganization);


        }

        private void ConfigureIndexes()
        {
            _builder.HasIndex(a => a.IdAddress).IsUnique(true);
        }

        private void ConfigureColumns()
        {
            _builder.HasKey(x => x.Id);
            _builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnName("Id");
            _builder.Property(x => x.Name).IsRequired(true).HasColumnName("Name").HasColumnType("VARCHAR(300)");
            _builder.Property(x => x.Description).IsRequired(true).HasColumnName("Description").HasColumnType("NVARCHAR(MAX)");
            _builder.Property(x => x.Logo).IsRequired(false).HasColumnName("Logo").HasColumnType("NVARCHAR(MAX)");           
            _builder.Property(x => x.CreatedTime).HasColumnName("Created_Time");
            _builder.Property(x => x.UpdateTime).HasColumnName("Update_Time");
            _builder.Property(x => x.IsActive).HasDefaultValue(true).HasColumnName("Is_Active");
        }
        
    }
}
