using TodosProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TodosProject.Infra.Data.Mapping
{
    public class AccessGroupMapping : IEntityTypeConfiguration<AccessGroup>
    {
        private EntityTypeBuilder<AccessGroup> _builder;

        public void Configure(EntityTypeBuilder<AccessGroup> builder)
        {
            _builder = builder;
            _builder.ToTable("AccessGroups");
            ConfigureForeignKeys();
            ConfigureColumns();
        }

        private void ConfigureForeignKeys()
        {
            
        }


        private void ConfigureColumns()
        {
            _builder.HasKey(x => x.Id);
            _builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnName("Id");
            _builder.Property(x => x.Name).IsRequired(true).HasColumnName("Name").HasColumnType("VARCHAR(300)");
            _builder.Property(x => x.Description).IsRequired(true).HasColumnName("Description").HasColumnType("NVARCHAR(MAX)");
            _builder.Property(x => x.CreatedTime).HasColumnName("Created_Time");
            _builder.Property(x => x.UpdateTime).HasColumnName("Update_Time");
            _builder.Property(x => x.IsActive).HasDefaultValue(true).HasColumnName("Is_Active");
        }
        
    }
}
