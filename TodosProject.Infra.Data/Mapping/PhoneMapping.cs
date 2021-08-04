using TodosProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TodosProject.Infra.Data.Mapping
{
    public class PhoneMapping : IEntityTypeConfiguration<Phone>
    {
        private EntityTypeBuilder<Phone> _builder;
        public void Configure(EntityTypeBuilder<Phone> builder)
        {
            _builder = builder;
            _builder.ToTable("Phones");
            ConfigureColumns();           
        }
        private void ConfigureColumns()
        {
            _builder.HasKey(x => x.Id);
            _builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnName("Id");
            _builder.Property(x => x.Number).IsRequired(true).HasMaxLength(255).HasColumnName("Number");
        }
    }
}
