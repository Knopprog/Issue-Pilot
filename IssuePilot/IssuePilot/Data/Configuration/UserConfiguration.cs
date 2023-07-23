using IssuePilot.Models.DBModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace IssuePilot.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        private const string ADMIN_ID = "B22698B8-42A2-4115-9631-1C2D1E2AC5F7";
        private const string DEMO_ADMIN_ID = "B22698B8-42A2-4115-9631-1C2D1E2AC5C5";
        private const string DEMO_MANAGER_ID = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1";
        private const string DEMO_USER_ID = "e28b8357-2955-4f3c-9c3d-1ec6ab1f4335";

        public void Configure(EntityTypeBuilder<User> builder)
        {
            User admin = new User
            {
                Id = ADMIN_ID,
                Firstname = "Admin",
                Surname = "Admin",
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "Admin@Admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = new Guid().ToString("D"),
                CreateDate = DateTime.Now
            };

            admin.PasswordHash = PasswordGenerate(admin);
            builder.HasData(admin);

            User demoManager = new User
            {
                Id = DEMO_MANAGER_ID,
                Firstname = "Demo",
                Surname = "Manager",
                UserName = "Demo-Manager",
                NormalizedUserName = "DEMO-MANAGER",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = new Guid().ToString("D"),
                CreateDate = DateTime.Now
            };
            demoManager.PasswordHash = PasswordGenerate(demoManager);
            builder.HasData(demoManager);

            User demoUser = new User
            {
                Id = DEMO_USER_ID,
                Firstname = "Demo",
                Surname = "Benutzer",
                UserName = "Demo-Benutzer",
                NormalizedUserName = "DEMO-BENUTZER",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = new Guid().ToString("D"),
                CreateDate = DateTime.Now
            };
            demoUser.PasswordHash = PasswordGenerate(demoUser);
            builder.HasData(demoUser);

            User demoAdmin = new User
            {
                Id = DEMO_ADMIN_ID,
                Firstname = "Demo",
                Surname = "Admin",
                UserName = "Demo-Admin",
                NormalizedUserName = "DEMO-ADMIN",
                Email = "demoadmin@Admin.com",
                NormalizedEmail = "DEMOADMIN@ADMIN.COM",
                SecurityStamp = new Guid().ToString("D"),
                CreateDate = DateTime.Now
            };

            demoAdmin.PasswordHash = PasswordGenerate(demoAdmin);
            builder.HasData(demoAdmin);
        }

        public string PasswordGenerate(User user)
        {
            PasswordHasher<User> passHash = new PasswordHasher<User>();
            return passHash.HashPassword(user, "password");
        }
    }
}
