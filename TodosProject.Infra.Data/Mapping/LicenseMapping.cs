using TodosProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TodosProject.Infra.Data.Mapping
{
    public class LicenseMapping : IEntityTypeConfiguration<License>
    {
        private EntityTypeBuilder<License> _builder;

        public void Configure(EntityTypeBuilder<License> builder)
        {
            _builder = builder;
            _builder.ToTable("Licenses");
            ConfigureColumns();
            ConfigureForeignKeys();
        }

        private void ConfigureColumns()
        {
            _builder.HasKey(x => x.Id);
            _builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnName("Id");
            _builder.Property(x => x.ActivationDate).IsRequired(true).HasColumnName("Activation_Date");
            _builder.Property(x => x.ExpirationDate).HasColumnName("Expiration_Date");
            _builder.Property(x => x.MaxFileCapacity).HasColumnName("Max_File_Capacity");
            _builder.Property(x => x.MultiplayerMinutes).HasColumnName("Multiplayer_Minutes");
            _builder.Property(x => x.CodeID).IsRequired(true).HasColumnName("Code_ID");
            _builder.Property(x => x.CreatedTime).HasColumnName("Created_Time");
            _builder.Property(x => x.UpdateTime).HasColumnName("Update_Time");
            _builder.Property(x => x.IsActive).HasDefaultValue(true).HasColumnName("Is_Active");
        }

        private void ConfigureForeignKeys()
        {
            _builder.HasOne(a => a.Organization).WithMany(x=>x.Licenses).HasForeignKey(a => a.IdOrganization);
        }

    }
}
