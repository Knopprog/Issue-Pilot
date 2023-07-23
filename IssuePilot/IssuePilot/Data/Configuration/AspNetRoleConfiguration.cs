using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IssuePilot.Data.Configuration
{
    public class AspNetRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        private const string ADMIN_ID = "2301D884-221A-4E7D-B509-0113DCC043E1";
        private const string MANAGER_ID = "7D9B7113-A8F8-4035-99A7-A20DD400F6A3";
        private const string USER_ID = "78A7570F-3CE5-48BA-9461-80283ED1D94D";
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.ToTable("AspNetRoles");
            builder.HasData(
                new IdentityRole
                {
                    Id = ADMIN_ID,
                    Name = "Admin",
                    NormalizedName = "ADMIN"

                },
                new IdentityRole
                {
                    Id = MANAGER_ID,
                    Name = "Projektmanager",
                    NormalizedName = "PROJEKTMANAGER"
                },
                new IdentityRole
                {
                    Id = USER_ID,
                    Name = "Benutzer",
                    NormalizedName = "BENUTZER"
                });
        }
    }
}
