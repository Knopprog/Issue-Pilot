using IssuePilot.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace IssuePilot.Data.Configuration
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {

        public void Configure(EntityTypeBuilder<Project> builder)
        {
            // Manager-ID: 92d45cb7-746e-46d2-ad7b-ddc551eb1ef1
            // DemoAdmin-ID: B22698B8-42A2-4115-9631-1C2D1E2AC5C5

            builder.ToTable("Projects");
            builder.HasData(
                new
                {
                    Id = 1,
                    CreateDate = DateTime.Now,
                    Title = "Testprojekt",
                    Description = "Ein vom System generiertes Project zum Testen.",
                    CreatorId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    DeletedTicketsCount = 0
                },
                 new
                 {
                     Id = 2,
                     CreateDate = DateTime.Now,
                     Title = "Testprojekt 2: Lorem ipsum dolor sit amet",
                     Description = "Lorem ipsum dolor sit amet, consectetur adipisici elit, sed eiusmod tempor incidunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquid ex ea commodi consequat. Quis aute iure reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint obcaecat cupiditat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                     CreatorId = "B22698B8-42A2-4115-9631-1C2D1E2AC5C5",
                     DeletedTicketsCount = 0
                 });
        }


    }
}
