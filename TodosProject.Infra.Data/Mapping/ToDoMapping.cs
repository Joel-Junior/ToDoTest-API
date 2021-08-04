using TodosProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TodosProject.Infra.Data.Mapping
{
    public class ToDoMapping : IEntityTypeConfiguration<ToDo>
    {
        private EntityTypeBuilder<ToDo> _builder;

        public void Configure(EntityTypeBuilder<ToDo> builder)
        {
            _builder = builder;
            _builder.ToTable("ToDos");
            ConfigureColumns();
            ConfigureForeignKeys();
        }

        private void ConfigureColumns()
        {
            _builder.HasKey(x => x.Id);
            _builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnName("Id");
            _builder.Property(x => x.DueDate).IsRequired(true).HasColumnName("Due_Date");
            _builder.Property(x => x.ConcludeDate).HasColumnName("ConcludeDate_Date");
            _builder.Property(x => x.Description).IsRequired(true).HasColumnName("Description").HasColumnType("NVARCHAR(MAX)");           
            _builder.Property(x => x.CreatedTime).HasColumnName("Created_Time");
            _builder.Property(x => x.UpdateTime).HasColumnName("Update_Time");
            _builder.Property(x => x.IsActive).HasDefaultValue(true).HasColumnName("Is_Active");
        }

        private void ConfigureForeignKeys()
        {
            _builder.HasOne(a => a.User).WithMany().HasForeignKey(fk => fk.IdUser);
        }

    }
}
