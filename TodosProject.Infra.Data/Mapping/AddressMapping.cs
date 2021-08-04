using TodosProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TodosProject.Infra.Data.Mapping
{
    class AddressMapping : IEntityTypeConfiguration<Address>
    {
        private EntityTypeBuilder<Address> _builder;

        public void Configure(EntityTypeBuilder<Address> builder)
        {
            _builder = builder;
            _builder.ToTable("Address");
            ConfigureColumns();
        }

        private void ConfigureColumns()
        {
            _builder.HasKey(x => x.Id);
            _builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnName("Id");
            _builder.Property(p => p.City).HasColumnName("City").HasColumnType("VARCHAR(255)");
            _builder.Property(p => p.State).HasColumnName("State").HasColumnType("CHAR(2)");
            _builder.Property(p => p.Cep).HasColumnName("Cep").HasColumnType("VARCHAR(10)");
            _builder.Property(p => p.PublicPlace).HasColumnName("PublicPlace").HasColumnType("NVARCHAR(MAX)");
            _builder.Property(p => p.Neighborhood).HasColumnName("Neighborhood").HasColumnType("VARCHAR(225)");
            _builder.Property(p => p.Number).HasColumnName("Number").HasColumnType("VARCHAR(10)");
            _builder.Property(p => p.Complement).HasColumnName("Complement").HasColumnType("VARCHAR(255)");
        }

    }

}
