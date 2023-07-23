using IssuePilot.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IssuePilot.Data.Configuration
{

    public class ProjectMemberEntriesConfiguration : IEntityTypeConfiguration<ProjectMemberEntry>
    {
        public void Configure(EntityTypeBuilder<ProjectMemberEntry> builder)
        {
            builder.ToTable("ProjectMemberEntries");
            builder.HasData(
                new
                {
                    FK_ProjectId = 1,
                    FK_UserId = "e28b8357-2955-4f3c-9c3d-1ec6ab1f4335",
                    ProjectRoleId = ProjectRoleId.Member
                },
                new
                {
                    FK_ProjectId = 1,
                    FK_UserId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    ProjectRoleId = ProjectRoleId.Owner
                },
                new
                {
                    FK_ProjectId = 1,
                    FK_UserId = "B22698B8-42A2-4115-9631-1C2D1E2AC5C5",
                    ProjectRoleId = ProjectRoleId.Member
                },
                new
                {
                    FK_ProjectId = 2,
                    FK_UserId = "B22698B8-42A2-4115-9631-1C2D1E2AC5C5",
                    ProjectRoleId = ProjectRoleId.Member
                },
                new
                {
                    FK_ProjectId = 2,
                    FK_UserId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    ProjectRoleId = ProjectRoleId.Owner
                });
        }
    }
}
