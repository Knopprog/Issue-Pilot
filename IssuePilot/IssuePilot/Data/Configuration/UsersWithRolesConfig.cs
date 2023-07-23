using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IssuePilot.Data.Configuration
{
    public class UsersWithRolesConfig : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        private const string ADMIN_ID = "B22698B8-42A2-4115-9631-1C2D1E2AC5F7";
        private const string DEMO_MANAGER_ID = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1";
        private const string DEMO_USER_ID = "e28b8357-2955-4f3c-9c3d-1ec6ab1f4335";
        private const string DEMO_ADMIN_ID = "B22698B8-42A2-4115-9631-1C2D1E2AC5C5";

        private const string ADMIN_ROLE_ID = "2301D884-221A-4E7D-B509-0113DCC043E1";
        private const string USER_ROLE_ID = "78A7570F-3CE5-48BA-9461-80283ED1D94D";
        private const string MANAGER_ROLE_ID = "7D9B7113-A8F8-4035-99A7-A20DD400F6A3";

        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            IdentityUserRole<string> admin = new IdentityUserRole<string>
            {
                RoleId = ADMIN_ROLE_ID,
                UserId = ADMIN_ID
            };

            builder.HasData(admin);

            IdentityUserRole<string> manager = new IdentityUserRole<string>
            {
                RoleId = MANAGER_ROLE_ID,
                UserId = DEMO_MANAGER_ID
            };

            builder.HasData(manager);

            IdentityUserRole<string> user = new IdentityUserRole<string>
            {
                RoleId = USER_ROLE_ID,
                UserId = DEMO_USER_ID
            };

            builder.HasData(user);

            IdentityUserRole<string> demoAdmin = new IdentityUserRole<string>
            {
                RoleId = ADMIN_ROLE_ID,
                UserId = DEMO_ADMIN_ID
            };

            builder.HasData(demoAdmin);
        }
    }
}
