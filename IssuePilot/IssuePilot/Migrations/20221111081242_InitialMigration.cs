using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IssuePilot.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 254, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Firstname = table.Column<string>(maxLength: 20, nullable: true),
                    Surname = table.Column<string>(maxLength: 20, nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewsEntryCases",
                columns: table => new
                {
                    EntryCaseId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsEntryCases", x => x.EntryCaseId);
                });

            migrationBuilder.CreateTable(
                name: "ProjectRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketHistoryEntryCases",
                columns: table => new
                {
                    EntryCaseId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketHistoryEntryCases", x => x.EntryCaseId);
                });

            migrationBuilder.CreateTable(
                name: "TicketStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 140, nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    DeletedTicketsCount = table.Column<int>(nullable: false),
                    CreatorId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMemberEntries",
                columns: table => new
                {
                    FK_ProjectId = table.Column<int>(nullable: false),
                    FK_UserId = table.Column<string>(nullable: false),
                    ProjectRoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMemberEntries", x => new { x.FK_ProjectId, x.FK_UserId });
                    table.ForeignKey(
                        name: "FK_ProjectMemberEntries_Projects_FK_ProjectId",
                        column: x => x.FK_ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectMemberEntries_AspNetUsers_FK_UserId",
                        column: x => x.FK_UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectMemberEntries_ProjectRoles_ProjectRoleId",
                        column: x => x.ProjectRoleId,
                        principalTable: "ProjectRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    ProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketCategories_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 140, nullable: false),
                    Deadline = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(maxLength: 5000, nullable: true),
                    TicketStatusId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CloseDate = table.Column<DateTime>(nullable: true),
                    Weight = table.Column<int>(nullable: false),
                    ClosedByUserId = table.Column<string>(nullable: true),
                    TicketCreatorId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_AspNetUsers_ClosedByUserId",
                        column: x => x.ClosedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_AspNetUsers_TicketCreatorId",
                        column: x => x.TicketCreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_TicketStatuses_TicketStatusId",
                        column: x => x.TicketStatusId,
                        principalTable: "TicketStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(maxLength: 3001, nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<string>(nullable: true),
                    TicketId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageData = table.Column<byte[]>(nullable: false),
                    TicketId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketAssignees",
                columns: table => new
                {
                    FK_TicketId = table.Column<int>(nullable: false),
                    FK_UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketAssignees", x => new { x.FK_TicketId, x.FK_UserId });
                    table.ForeignKey(
                        name: "FK_TicketAssignees_Tickets_FK_TicketId",
                        column: x => x.FK_TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketAssignees_AspNetUsers_FK_UserId",
                        column: x => x.FK_UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketHistoryEntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferredUserId = table.Column<string>(nullable: true),
                    EntryCreatorId = table.Column<string>(nullable: true),
                    TicketId = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    TicketHistoryEntryCaseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketHistoryEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketHistoryEntries_AspNetUsers_EntryCreatorId",
                        column: x => x.EntryCreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketHistoryEntries_AspNetUsers_ReferredUserId",
                        column: x => x.ReferredUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketHistoryEntries_TicketHistoryEntryCases_TicketHistoryEntryCaseId",
                        column: x => x.TicketHistoryEntryCaseId,
                        principalTable: "TicketHistoryEntryCases",
                        principalColumn: "EntryCaseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketHistoryEntries_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketProjectCategories",
                columns: table => new
                {
                    TicketId = table.Column<int>(nullable: false),
                    TicketCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketProjectCategories", x => new { x.TicketId, x.TicketCategoryId });
                    table.ForeignKey(
                        name: "FK_TicketProjectCategories_TicketCategories_TicketCategoryId",
                        column: x => x.TicketCategoryId,
                        principalTable: "TicketCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketProjectCategories_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NewsEntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    NewsEntryCaseId = table.Column<int>(nullable: false),
                    Seen = table.Column<bool>(nullable: false),
                    OwnerId = table.Column<string>(nullable: true),
                    ReferredUserId = table.Column<string>(nullable: true),
                    TicketId = table.Column<int>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    CommentId = table.Column<int>(nullable: true),
                    StatusId = table.Column<int>(nullable: true),
                    RouteId = table.Column<int>(nullable: true),
                    ActionId = table.Column<int>(nullable: false),
                    ControllerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsEntries_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewsEntries_NewsEntryCases_NewsEntryCaseId",
                        column: x => x.NewsEntryCaseId,
                        principalTable: "NewsEntryCases",
                        principalColumn: "EntryCaseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewsEntries_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NewsEntries_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NewsEntries_AspNetUsers_ReferredUserId",
                        column: x => x.ReferredUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NewsEntries_TicketStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "TicketStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NewsEntries_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "78A7570F-3CE5-48BA-9461-80283ED1D94D", "ea6156b5-c8cc-491a-ad8b-5f4b15f17c00", "Benutzer", "BENUTZER" },
                    { "2301D884-221A-4E7D-B509-0113DCC043E1", "771f9501-1de4-4f61-9a62-7a0087a0d4b7", "Admin", "ADMIN" },
                    { "7D9B7113-A8F8-4035-99A7-A20DD400F6A3", "0ddf905f-8be3-4325-9612-3e13b5fceb4e", "Projektmanager", "PROJEKTMANAGER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreateDate", "Email", "EmailConfirmed", "Firstname", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "B22698B8-42A2-4115-9631-1C2D1E2AC5C5", 0, "22cd9460-d0e2-431f-a9cc-8598cc157312", new DateTime(2022, 11, 11, 9, 12, 41, 929, DateTimeKind.Local).AddTicks(8277), "demoadmin@Admin.com", false, "Demo", false, false, null, "DEMOADMIN@ADMIN.COM", "DEMO-ADMIN", "AQAAAAEAACcQAAAAELKR1L5L3W4RjeU7WYbengP+qFS2BD7U+fsgg0mAB0aVfuD4fcCOWa6sYCiZiqYLaw==", null, false, "00000000-0000-0000-0000-000000000000", "Admin", false, "Demo-Admin" },
                    { "e28b8357-2955-4f3c-9c3d-1ec6ab1f4335", 0, "bdbc7c99-6b06-4f7c-a4d7-48fff9d5076f", new DateTime(2022, 11, 11, 9, 12, 41, 924, DateTimeKind.Local).AddTicks(4911), null, true, "Demo", false, false, null, null, "DEMO-BENUTZER", "AQAAAAEAACcQAAAAEPxXNft5pE+YA+hWspZFnM2oSuZ7mj49grqrKwbXlg1AjSZnNxZ/D3e1gzsF6vXGRQ==", null, true, "00000000-0000-0000-0000-000000000000", "Benutzer", false, "Demo-Benutzer" },
                    { "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 0, "4ab495b7-a089-4bd0-b57d-5e4baccc28f4", new DateTime(2022, 11, 11, 9, 12, 41, 919, DateTimeKind.Local).AddTicks(1369), null, true, "Demo", false, false, null, null, "DEMO-MANAGER", "AQAAAAEAACcQAAAAELtG5CLi5uCArouczpEAIVm+qWoE5aZDiiRfmkLBNDwESnhMEGyZ9NSVm2OzV4ZD1g==", null, true, "00000000-0000-0000-0000-000000000000", "Manager", false, "Demo-Manager" },
                    { "B22698B8-42A2-4115-9631-1C2D1E2AC5F7", 0, "860d3d4b-cc52-4ff0-b249-8c3656177641", new DateTime(2022, 11, 11, 9, 12, 41, 907, DateTimeKind.Local).AddTicks(4174), "Admin@Admin.com", true, "Admin", false, false, null, "ADMIN@ADMIN.COM", "ADMIN", "AQAAAAEAACcQAAAAELUB6YwaTKyNRtILr6OP9t0h4J7L6KSJdEkWOpVsq/cycstlv2zJ/Kk9zs/xrKF+cg==", null, true, "00000000-0000-0000-0000-000000000000", "Admin", false, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "NewsEntryCases",
                columns: new[] { "EntryCaseId", "Name" },
                values: new object[,]
                {
                    { 0, "NewProject" },
                    { 1, "NewTicket" },
                    { 2, "NewComment" },
                    { 3, "NewStatus" },
                    { 4, "AssignedToTicket" },
                    { 5, "UnassignedFromTicket" },
                    { 6, "AddedToProject" },
                    { 7, "RemovedFromProject" }
                });

            migrationBuilder.InsertData(
                table: "ProjectRoles",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 0, "Member" },
                    { 1, "Owner" }
                });

            migrationBuilder.InsertData(
                table: "TicketHistoryEntryCases",
                columns: new[] { "EntryCaseId", "Name" },
                values: new object[,]
                {
                    { 8, "CommentDeleted" },
                    { 5, "TicketPaused" },
                    { 6, "TicketInProgress" },
                    { 4, "TicketCanceled" },
                    { 3, "TicketOpened" },
                    { 2, "TicketClosed" },
                    { 1, "MemberUnassigned" },
                    { 0, "MemberAssigned" },
                    { 7, "CommentAdded" }
                });

            migrationBuilder.InsertData(
                table: "TicketStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "InProgress" },
                    { 2, "Paused" },
                    { 3, "Canceled" },
                    { 4, "Closed" },
                    { 0, "Open" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[,]
                {
                    { "B22698B8-42A2-4115-9631-1C2D1E2AC5F7", "2301D884-221A-4E7D-B509-0113DCC043E1" },
                    { "B22698B8-42A2-4115-9631-1C2D1E2AC5C5", "2301D884-221A-4E7D-B509-0113DCC043E1" },
                    { "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", "7D9B7113-A8F8-4035-99A7-A20DD400F6A3" },
                    { "e28b8357-2955-4f3c-9c3d-1ec6ab1f4335", "78A7570F-3CE5-48BA-9461-80283ED1D94D" }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "CreateDate", "CreatorId", "DeletedTicketsCount", "Description", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 11, 11, 9, 12, 41, 936, DateTimeKind.Local).AddTicks(2181), "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 0, "Ein vom System generiertes Project zum Testen.", "Testprojekt" },
                    { 2, new DateTime(2022, 11, 11, 9, 12, 41, 936, DateTimeKind.Local).AddTicks(2929), "B22698B8-42A2-4115-9631-1C2D1E2AC5C5", 0, "Lorem ipsum dolor sit amet, consectetur adipisici elit, sed eiusmod tempor incidunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquid ex ea commodi consequat. Quis aute iure reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint obcaecat cupiditat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.", "Testprojekt 2: Lorem ipsum dolor sit amet" }
                });

            migrationBuilder.InsertData(
                table: "NewsEntries",
                columns: new[] { "Id", "ActionId", "CommentId", "ControllerId", "CreateDate", "NewsEntryCaseId", "OwnerId", "ProjectId", "ReferredUserId", "RouteId", "Seen", "StatusId", "TicketId" },
                values: new object[,]
                {
                    { 1, 1, null, 1, new DateTime(2022, 11, 11, 9, 12, 41, 938, DateTimeKind.Local).AddTicks(3455), 0, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 1, false, null, null },
                    { 2, 4, null, 2, new DateTime(2022, 11, 11, 9, 12, 41, 938, DateTimeKind.Local).AddTicks(4176), 6, "B22698B8-42A2-4115-9631-1C2D1E2AC5C5", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 1, false, null, null },
                    { 3, 4, null, 2, new DateTime(2022, 11, 11, 9, 12, 41, 938, DateTimeKind.Local).AddTicks(4199), 6, "e28b8357-2955-4f3c-9c3d-1ec6ab1f4335", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 1, false, null, null }
                });

            migrationBuilder.InsertData(
                table: "ProjectMemberEntries",
                columns: new[] { "FK_ProjectId", "FK_UserId", "ProjectRoleId" },
                values: new object[,]
                {
                    { 1, "e28b8357-2955-4f3c-9c3d-1ec6ab1f4335", 0 },
                    { 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 1 },
                    { 1, "B22698B8-42A2-4115-9631-1C2D1E2AC5C5", 0 },
                    { 2, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 1 },
                    { 2, "B22698B8-42A2-4115-9631-1C2D1E2AC5C5", 0 }
                });

            migrationBuilder.InsertData(
                table: "TicketCategories",
                columns: new[] { "Id", "Name", "ProjectId" },
                values: new object[,]
                {
                    { 8, "Dokumentation", 2 },
                    { 7, "Frage", 2 },
                    { 6, "Bug", 2 },
                    { 5, "Feature", 1 },
                    { 4, "Diskussion", 1 },
                    { 3, "Dokumentation", 1 },
                    { 2, "Frage", 1 },
                    { 1, "Bug", 1 },
                    { 10, "Feature", 2 },
                    { 9, "Diskussion", 2 }
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "CloseDate", "ClosedByUserId", "CreateDate", "Deadline", "Description", "ProjectId", "TicketCreatorId", "TicketStatusId", "Title", "Weight" },
                values: new object[,]
                {
                    { 12, null, null, new DateTime(2022, 11, 11, 9, 12, 41, 937, DateTimeKind.Local).AddTicks(2539), null, "Ein vom System generiertes Ticket zum Testen.", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 0, "TestTicket 12", 1 },
                    { 13, null, null, new DateTime(2022, 11, 11, 9, 12, 41, 937, DateTimeKind.Local).AddTicks(2541), null, "Ein vom System generiertes Ticket zum Testen.", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 0, "TestTicket 13", 1 },
                    { 9, null, null, new DateTime(2022, 11, 11, 9, 12, 41, 937, DateTimeKind.Local).AddTicks(2532), null, "Ein vom System generiertes Ticket zum Testen.", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 0, "TestTicket 9", 1 },
                    { 8, null, null, new DateTime(2022, 11, 11, 9, 12, 41, 937, DateTimeKind.Local).AddTicks(2529), null, "Ein vom System generiertes Ticket zum Testen.", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 0, "TestTicket 8", 1 },
                    { 7, null, null, new DateTime(2022, 11, 11, 9, 12, 41, 937, DateTimeKind.Local).AddTicks(2527), null, "Ein vom System generiertes Ticket zum Testen.", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 0, "TestTicket 7", 1 },
                    { 6, null, null, new DateTime(2022, 11, 11, 9, 12, 41, 937, DateTimeKind.Local).AddTicks(2516), null, "Ein vom System generiertes Ticket zum Testen.", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 0, "TestTicket 6", 1 },
                    { 5, new DateTime(2022, 11, 11, 9, 12, 41, 937, DateTimeKind.Local).AddTicks(1826), "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", new DateTime(2022, 11, 11, 9, 12, 41, 937, DateTimeKind.Local).AddTicks(1823), new DateTime(2022, 11, 13, 9, 12, 41, 937, DateTimeKind.Local).AddTicks(1828), "Ein vom System generiertes Ticket zum Testen.", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 4, "TestTicket 5", 0 },
                    { 4, null, null, new DateTime(2022, 11, 11, 9, 12, 41, 937, DateTimeKind.Local).AddTicks(1810), null, "Ein vom System generiertes Ticket zum Testen.", 1, "B22698B8-42A2-4115-9631-1C2D1E2AC5C5", 3, "TestTicket 4", 2 },
                    { 3, null, null, new DateTime(2022, 11, 11, 9, 12, 41, 937, DateTimeKind.Local).AddTicks(1783), new DateTime(2022, 11, 11, 9, 12, 41, 937, DateTimeKind.Local).AddTicks(1793), "Ein vom System generiertes Ticket zum Testen.", 1, "B22698B8-42A2-4115-9631-1C2D1E2AC5C5", 1, "TestTicket 3", 3 },
                    { 2, null, null, new DateTime(2022, 11, 11, 9, 12, 41, 937, DateTimeKind.Local).AddTicks(1211), null, "Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen.", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 2, "TestTicket 2", 2 },
                    { 1, null, null, new DateTime(2022, 11, 11, 9, 12, 41, 937, DateTimeKind.Local).AddTicks(417), new DateTime(2023, 11, 11, 9, 12, 41, 937, DateTimeKind.Local).AddTicks(431), "Ein vom System generiertes Ticket zum Testen.", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 0, "TestTicket", 1 },
                    { 11, null, null, new DateTime(2022, 11, 11, 9, 12, 41, 937, DateTimeKind.Local).AddTicks(2537), null, "Ein vom System generiertes Ticket zum Testen.", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 0, "TestTicket 11", 1 },
                    { 10, null, null, new DateTime(2022, 11, 11, 9, 12, 41, 937, DateTimeKind.Local).AddTicks(2534), null, "Ein vom System generiertes Ticket zum Testen.", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 0, "TestTicket 10", 1 }
                });

            migrationBuilder.InsertData(
                table: "NewsEntries",
                columns: new[] { "Id", "ActionId", "CommentId", "ControllerId", "CreateDate", "NewsEntryCaseId", "OwnerId", "ProjectId", "ReferredUserId", "RouteId", "Seen", "StatusId", "TicketId" },
                values: new object[,]
                {
                    { 4, 2, null, 1, new DateTime(2022, 11, 11, 9, 12, 41, 938, DateTimeKind.Local).AddTicks(4202), 1, "e28b8357-2955-4f3c-9c3d-1ec6ab1f4335", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 1, false, 0, 1 },
                    { 21, 2, null, 1, new DateTime(2022, 11, 11, 9, 12, 41, 938, DateTimeKind.Local).AddTicks(4938), 1, "B22698B8-42A2-4115-9631-1C2D1E2AC5C5", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 1, false, 0, 5 },
                    { 20, 2, null, 1, new DateTime(2022, 11, 11, 9, 12, 41, 938, DateTimeKind.Local).AddTicks(4936), 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 1, false, 0, 5 },
                    { 19, 2, null, 1, new DateTime(2022, 11, 11, 9, 12, 41, 938, DateTimeKind.Local).AddTicks(4934), 1, "e28b8357-2955-4f3c-9c3d-1ec6ab1f4335", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 1, false, 0, 5 },
                    { 18, 2, null, 1, new DateTime(2022, 11, 11, 9, 12, 41, 938, DateTimeKind.Local).AddTicks(4931), 1, "B22698B8-42A2-4115-9631-1C2D1E2AC5C5", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 1, false, 0, 4 },
                    { 17, 2, null, 1, new DateTime(2022, 11, 11, 9, 12, 41, 938, DateTimeKind.Local).AddTicks(4929), 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 1, false, 0, 4 },
                    { 16, 2, null, 1, new DateTime(2022, 11, 11, 9, 12, 41, 938, DateTimeKind.Local).AddTicks(4927), 1, "e28b8357-2955-4f3c-9c3d-1ec6ab1f4335", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 1, false, 0, 4 },
                    { 14, 2, null, 1, new DateTime(2022, 11, 11, 9, 12, 41, 938, DateTimeKind.Local).AddTicks(4922), 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 1, false, 0, 3 },
                    { 13, 2, null, 1, new DateTime(2022, 11, 11, 9, 12, 41, 938, DateTimeKind.Local).AddTicks(4920), 1, "e28b8357-2955-4f3c-9c3d-1ec6ab1f4335", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 1, false, 0, 3 },
                    { 15, 2, null, 1, new DateTime(2022, 11, 11, 9, 12, 41, 938, DateTimeKind.Local).AddTicks(4925), 1, "B22698B8-42A2-4115-9631-1C2D1E2AC5C5", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 1, false, 0, 3 },
                    { 11, 2, null, 1, new DateTime(2022, 11, 11, 9, 12, 41, 938, DateTimeKind.Local).AddTicks(4915), 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 1, false, 0, 2 },
                    { 10, 2, null, 1, new DateTime(2022, 11, 11, 9, 12, 41, 938, DateTimeKind.Local).AddTicks(4913), 1, "e28b8357-2955-4f3c-9c3d-1ec6ab1f4335", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 1, false, 0, 2 },
                    { 9, 2, null, 1, new DateTime(2022, 11, 11, 9, 12, 41, 938, DateTimeKind.Local).AddTicks(4911), 3, "e28b8357-2955-4f3c-9c3d-1ec6ab1f4335", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 2, false, 1, 2 },
                    { 8, 2, null, 1, new DateTime(2022, 11, 11, 9, 12, 41, 938, DateTimeKind.Local).AddTicks(4908), 3, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 2, false, 1, 2 },
                    { 7, 2, null, 1, new DateTime(2022, 11, 11, 9, 12, 41, 938, DateTimeKind.Local).AddTicks(4906), 3, "B22698B8-42A2-4115-9631-1C2D1E2AC5C5", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 2, false, 1, 2 },
                    { 6, 2, null, 1, new DateTime(2022, 11, 11, 9, 12, 41, 938, DateTimeKind.Local).AddTicks(4903), 1, "B22698B8-42A2-4115-9631-1C2D1E2AC5C5", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 1, false, 0, 1 },
                    { 5, 2, null, 1, new DateTime(2022, 11, 11, 9, 12, 41, 938, DateTimeKind.Local).AddTicks(4877), 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 1, false, 0, 1 },
                    { 12, 2, null, 1, new DateTime(2022, 11, 11, 9, 12, 41, 938, DateTimeKind.Local).AddTicks(4918), 1, "B22698B8-42A2-4115-9631-1C2D1E2AC5C5", 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 1, false, 0, 2 }
                });

            migrationBuilder.InsertData(
                table: "TicketProjectCategories",
                columns: new[] { "TicketId", "TicketCategoryId" },
                values: new object[,]
                {
                    { 4, 3 },
                    { 2, 1 },
                    { 1, 2 },
                    { 2, 2 },
                    { 3, 2 },
                    { 5, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CreatorId",
                table: "Comments",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TicketId",
                table: "Comments",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_TicketId",
                table: "Images",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsEntries_CommentId",
                table: "NewsEntries",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsEntries_NewsEntryCaseId",
                table: "NewsEntries",
                column: "NewsEntryCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsEntries_OwnerId",
                table: "NewsEntries",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsEntries_ProjectId",
                table: "NewsEntries",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsEntries_ReferredUserId",
                table: "NewsEntries",
                column: "ReferredUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsEntries_StatusId",
                table: "NewsEntries",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsEntries_TicketId",
                table: "NewsEntries",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMemberEntries_FK_UserId",
                table: "ProjectMemberEntries",
                column: "FK_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMemberEntries_ProjectRoleId",
                table: "ProjectMemberEntries",
                column: "ProjectRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CreatorId",
                table: "Projects",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Title",
                table: "Projects",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketAssignees_FK_UserId",
                table: "TicketAssignees",
                column: "FK_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketCategories_ProjectId",
                table: "TicketCategories",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketHistoryEntries_EntryCreatorId",
                table: "TicketHistoryEntries",
                column: "EntryCreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketHistoryEntries_ReferredUserId",
                table: "TicketHistoryEntries",
                column: "ReferredUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketHistoryEntries_TicketHistoryEntryCaseId",
                table: "TicketHistoryEntries",
                column: "TicketHistoryEntryCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketHistoryEntries_TicketId",
                table: "TicketHistoryEntries",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketProjectCategories_TicketCategoryId",
                table: "TicketProjectCategories",
                column: "TicketCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ClosedByUserId",
                table: "Tickets",
                column: "ClosedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ProjectId",
                table: "Tickets",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketCreatorId",
                table: "Tickets",
                column: "TicketCreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketStatusId",
                table: "Tickets",
                column: "TicketStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "NewsEntries");

            migrationBuilder.DropTable(
                name: "ProjectMemberEntries");

            migrationBuilder.DropTable(
                name: "TicketAssignees");

            migrationBuilder.DropTable(
                name: "TicketHistoryEntries");

            migrationBuilder.DropTable(
                name: "TicketProjectCategories");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "NewsEntryCases");

            migrationBuilder.DropTable(
                name: "ProjectRoles");

            migrationBuilder.DropTable(
                name: "TicketHistoryEntryCases");

            migrationBuilder.DropTable(
                name: "TicketCategories");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "TicketStatuses");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
