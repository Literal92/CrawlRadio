using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MusicProject.DataLayer.Migrations
{
    public partial class V2019_07_27_1135 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreatDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ContentType = table.Column<string>(nullable: true),
                    ContentLength = table.Column<byte>(nullable: false),
                    IsValid = table.Column<bool>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppDataProtectionKeys",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FriendlyName = table.Column<string>(nullable: true),
                    XmlData = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppDataProtectionKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppLogItems",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    EventId = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    LogLevel = table.Column<string>(nullable: true),
                    Logger = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    StateJson = table.Column<string>(nullable: true),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLogItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppRoles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSettings",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AboutUs = table.Column<string>(nullable: true),
                    CafebazarLink = table.Column<string>(nullable: true),
                    ChangeLog = table.Column<string>(nullable: true),
                    Site = table.Column<string>(nullable: true),
                    Instagram = table.Column<string>(nullable: true),
                    Telegram = table.Column<string>(nullable: true),
                    Soundcloud = table.Column<string>(nullable: true),
                    TypeId = table.Column<int>(nullable: false),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSqlCache",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 449, nullable: false),
                    Value = table.Column<byte[]>(nullable: false),
                    ExpiresAtTime = table.Column<DateTimeOffset>(nullable: false),
                    SlidingExpirationInSeconds = table.Column<long>(nullable: true),
                    AbsoluteExpiration = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSqlCache", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
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
                    FirstName = table.Column<string>(maxLength: 450, nullable: true),
                    LastName = table.Column<string>(maxLength: 450, nullable: true),
                    PhotoFileName = table.Column<string>(maxLength: 450, nullable: true),
                    ActivationCode = table.Column<string>(maxLength: 5, nullable: true),
                    BirthDate = table.Column<DateTimeOffset>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    LastVisitDateTime = table.Column<DateTimeOffset>(nullable: true),
                    IsEmailPublic = table.Column<bool>(nullable: false),
                    Gender = table.Column<bool>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categorys",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TypeId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 350, nullable: false),
                    SeoDescription = table.Column<string>(nullable: true),
                    SeoKeyboard = table.Column<string>(nullable: true),
                    Pic = table.Column<string>(maxLength: 800, nullable: true),
                    Thumbnail = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    PublishDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ContentText = table.Column<string>(nullable: true),
                    DatePublished = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    VisitCount = table.Column<int>(nullable: false),
                    LikeCount = table.Column<int>(nullable: false),
                    IsPublish = table.Column<bool>(nullable: false),
                    IsSelected = table.Column<bool>(nullable: true),
                    ZipMp3128 = table.Column<string>(nullable: true),
                    ZipMp3128Size = table.Column<string>(nullable: true),
                    ZipMp364 = table.Column<string>(nullable: true),
                    ZipMp364Size = table.Column<string>(nullable: true),
                    ZipMp3320 = table.Column<string>(nullable: true),
                    ZipMp3320Size = table.Column<string>(nullable: true),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categorys_Categorys",
                        column: x => x.ParentId,
                        principalSchema: "dbo",
                        principalTable: "Categorys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContactUs",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactUs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentLists",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PublishDateTime = table.Column<DateTimeOffset>(nullable: true),
                    DatePublished = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Pic = table.Column<string>(nullable: true),
                    Remix = table.Column<string>(nullable: true),
                    IsPublish = table.Column<bool>(nullable: false),
                    Thumbnail = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    VisitCount = table.Column<int>(nullable: false),
                    LikeCount = table.Column<int>(nullable: false),
                    ZipMp3128 = table.Column<string>(nullable: true),
                    ZipMp364 = table.Column<string>(nullable: true),
                    ZipMp3320 = table.Column<string>(nullable: true),
                    ZipMp364Size = table.Column<string>(nullable: true),
                    ZipMp3128Size = table.Column<string>(nullable: true),
                    ZipMp3320Size = table.Column<string>(nullable: true),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Subject = table.Column<string>(nullable: true),
                    OrderType = table.Column<string>(nullable: true),
                    ProjectDuration = table.Column<string>(nullable: true),
                    Level = table.Column<string>(nullable: true),
                    Budget = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Place = table.Column<string>(nullable: true),
                    Speecher = table.Column<string>(nullable: true),
                    Senario = table.Column<string>(nullable: true),
                    Time = table.Column<string>(nullable: true),
                    Count = table.Column<string>(nullable: true),
                    HasSpeecher = table.Column<string>(nullable: true),
                    HasSenario = table.Column<string>(nullable: true),
                    HasEdit = table.Column<string>(nullable: true),
                    FrameCount = table.Column<string>(nullable: true),
                    File = table.Column<string>(nullable: true),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Podcasts",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TypeId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    SeoDescription = table.Column<string>(nullable: true),
                    SeoKeyboard = table.Column<string>(nullable: true),
                    Pic = table.Column<string>(nullable: true),
                    Video = table.Column<string>(nullable: true),
                    Thumbnail = table.Column<string>(nullable: true),
                    PublishDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ContentText = table.Column<string>(nullable: true),
                    DatePublished = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    VisitCount = table.Column<int>(nullable: false),
                    LikeCount = table.Column<int>(nullable: false),
                    IsPublish = table.Column<bool>(nullable: false),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Podcasts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SessionRequests",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    Job = table.Column<string>(nullable: true),
                    ShopName = table.Column<string>(nullable: true),
                    ShopTell = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    SendSms = table.Column<bool>(nullable: false),
                    Newsletter = table.Column<bool>(nullable: false),
                    SessionTime = table.Column<string>(nullable: true),
                    SessionDate = table.Column<string>(nullable: true),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sliders",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TypeId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    SubTitle = table.Column<string>(nullable: true),
                    Pic = table.Column<string>(nullable: true),
                    Thumbnail = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sliders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    EnglishTitle = table.Column<string>(nullable: true),
                    SeoDescription = table.Column<string>(nullable: true),
                    SeoKeyword = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Pic = table.Column<string>(nullable: true),
                    Thumbnail = table.Column<string>(nullable: true),
                    IsSelected = table.Column<bool>(nullable: false),
                    IsPublish = table.Column<bool>(nullable: false),
                    VisitCount = table.Column<int>(nullable: false),
                    LikeCount = table.Column<int>(nullable: false),
                    ChildCount = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    Facebook = table.Column<string>(nullable: true),
                    Instagram = table.Column<string>(nullable: true),
                    Twitter = table.Column<string>(nullable: true),
                    Soundcloud = table.Column<string>(nullable: true),
                    Spotify = table.Column<string>(nullable: true),
                    Itunes = table.Column<string>(nullable: true),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppRoleClaims",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppRoleClaims_AppRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "AppRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserClaims",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUserClaims_AppUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserLogins",
                schema: "dbo",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AppUserLogins_AppUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserRoles",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AppUserRoles_AppRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "AppRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserRoles_AppUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserTokens",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AppUserTokens_AppUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserUsedPasswords",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HashedPassword = table.Column<string>(maxLength: 450, nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserUsedPasswords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUserUsedPasswords_AppUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TypeId = table.Column<int>(nullable: false),
                    Body = table.Column<string>(maxLength: 500, nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    Like = table.Column<int>(nullable: false),
                    Dislike = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    Tell = table.Column<string>(maxLength: 11, nullable: true),
                    UserCommentId = table.Column<int>(nullable: false),
                    IsPublished = table.Column<bool>(nullable: false),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    AttachmentId = table.Column<int>(nullable: true),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Attachment_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Comments",
                        column: x => x.ParentId,
                        principalSchema: "dbo",
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_AppUsers_UserCommentId",
                        column: x => x.UserCommentId,
                        principalSchema: "dbo",
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomUserTokens",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Token = table.Column<string>(nullable: true),
                    TokenExpiresDateTime = table.Column<DateTimeOffset>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomUserTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomUserTokens_AppUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Token = table.Column<string>(nullable: true),
                    FirebaseToken = table.Column<string>(nullable: true),
                    DeviceId = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    PhoneConfirm = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: true),
                    ActiveCode = table.Column<string>(nullable: true),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_AppUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contents",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(nullable: false),
                    TypeId = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    HeadLine = table.Column<string>(maxLength: 500, nullable: true),
                    Title = table.Column<string>(maxLength: 500, nullable: false),
                    SubTitle = table.Column<string>(maxLength: 500, nullable: true),
                    Lead = table.Column<string>(maxLength: 1500, nullable: true),
                    ContentText = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    LikeCount = table.Column<int>(nullable: false),
                    VisitCount = table.Column<int>(nullable: false),
                    IsArchive = table.Column<bool>(nullable: false),
                    SeoTitle = table.Column<string>(maxLength: 500, nullable: true),
                    SeoDescription = table.Column<string>(maxLength: 500, nullable: true),
                    SeoKeyboard = table.Column<string>(maxLength: 500, nullable: true),
                    SeoUrl = table.Column<string>(maxLength: 500, nullable: true),
                    Video = table.Column<string>(nullable: true),
                    Pic = table.Column<string>(nullable: true),
                    Thumbnail = table.Column<string>(nullable: true),
                    MediumPic = table.Column<string>(nullable: true),
                    Pic2 = table.Column<string>(nullable: true),
                    Thumbnail2 = table.Column<string>(nullable: true),
                    MediumPic2 = table.Column<string>(nullable: true),
                    Pic3 = table.Column<string>(nullable: true),
                    Thumbnail3 = table.Column<string>(nullable: true),
                    MediumPic3 = table.Column<string>(nullable: true),
                    Svg = table.Column<string>(nullable: true),
                    Pdf = table.Column<string>(nullable: true),
                    Mp3128 = table.Column<string>(nullable: true),
                    Mp364 = table.Column<string>(nullable: true),
                    Mp3320 = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    PublishDateTime = table.Column<DateTimeOffset>(nullable: true),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contents_Categorys",
                        column: x => x.CategoryId,
                        principalSchema: "dbo",
                        principalTable: "Categorys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentListFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContentListId = table.Column<int>(nullable: false),
                    Mp364 = table.Column<string>(nullable: true),
                    Mp3128 = table.Column<string>(nullable: true),
                    Mp3320 = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    LikeCount = table.Column<int>(nullable: false),
                    VisitCount = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Ext = table.Column<string>(nullable: true),
                    FileSize = table.Column<long>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentListFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentListFiles_ContentLists_ContentListId",
                        column: x => x.ContentListId,
                        principalSchema: "dbo",
                        principalTable: "ContentLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryTags",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryTags_Categories",
                        column: x => x.CategoryId,
                        principalSchema: "dbo",
                        principalTable: "Categorys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryTags_Tags",
                        column: x => x.TagId,
                        principalSchema: "dbo",
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentListTags",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContentListId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentListTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentListTags_ContentLists",
                        column: x => x.ContentListId,
                        principalSchema: "dbo",
                        principalTable: "ContentLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentListTags_Tags",
                        column: x => x.TagId,
                        principalSchema: "dbo",
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PodcastTags",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PodcastId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PodcastTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PodcastTags_Podcasts",
                        column: x => x.PodcastId,
                        principalSchema: "dbo",
                        principalTable: "Podcasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PodcastTags_Tags",
                        column: x => x.TagId,
                        principalSchema: "dbo",
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryComments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(nullable: false),
                    CommentId = table.Column<int>(nullable: false),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryComments_Categorys_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "dbo",
                        principalTable: "Categorys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryComments_Comments_CommentId",
                        column: x => x.CommentId,
                        principalSchema: "dbo",
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentListComments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContentListId = table.Column<int>(nullable: false),
                    CommentId = table.Column<int>(nullable: false),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentListComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentListComments_Comments_CommentId",
                        column: x => x.CommentId,
                        principalSchema: "dbo",
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentListComments_ContentLists_ContentListId",
                        column: x => x.ContentListId,
                        principalSchema: "dbo",
                        principalTable: "ContentLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PodcastComments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PodcastId = table.Column<int>(nullable: false),
                    CommentId = table.Column<int>(nullable: false),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PodcastComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PodcastComments_Comments_CommentId",
                        column: x => x.CommentId,
                        principalSchema: "dbo",
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PodcastComments_Podcasts_PodcastId",
                        column: x => x.PodcastId,
                        principalSchema: "dbo",
                        principalTable: "Podcasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TagComments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TagId = table.Column<int>(nullable: false),
                    CommentId = table.Column<int>(nullable: false),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TagComments_Comments_CommentId",
                        column: x => x.CommentId,
                        principalSchema: "dbo",
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagComments_Tags_TagId",
                        column: x => x.TagId,
                        principalSchema: "dbo",
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentComments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContentId = table.Column<int>(nullable: false),
                    CommentId = table.Column<int>(nullable: true),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentComments_Comments_CommentId",
                        column: x => x.CommentId,
                        principalSchema: "dbo",
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContentComments_Contents_ContentId",
                        column: x => x.ContentId,
                        principalSchema: "dbo",
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentFiles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContentId = table.Column<int>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    FileName2 = table.Column<string>(nullable: true),
                    FileName3 = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Pic = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ContentText = table.Column<string>(nullable: true),
                    Thumbnail = table.Column<string>(maxLength: 500, nullable: true),
                    LikeCount = table.Column<int>(nullable: false),
                    VisitCount = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Ext = table.Column<string>(nullable: true),
                    FileSize = table.Column<long>(nullable: false),
                    FileSize2 = table.Column<long>(nullable: false),
                    FileSize3 = table.Column<long>(nullable: false),
                    IsPublish = table.Column<bool>(nullable: true),
                    IsSelected = table.Column<bool>(nullable: true),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentFiles_Contents",
                        column: x => x.ContentId,
                        principalSchema: "dbo",
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentRelateds",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContentId = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    RelatedId = table.Column<int>(nullable: false),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentRelateds", x => x.id);
                    table.ForeignKey(
                        name: "FK_ContentRelateds_Contents",
                        column: x => x.ContentId,
                        principalSchema: "dbo",
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContentRelateds_Contents1",
                        column: x => x.RelatedId,
                        principalSchema: "dbo",
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContentSelecteds",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContentId = table.Column<int>(nullable: false),
                    ContentListId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentSelecteds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentSelecteds_Contents",
                        column: x => x.ContentId,
                        principalSchema: "dbo",
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentSelecteds_ContentLists",
                        column: x => x.ContentListId,
                        principalSchema: "dbo",
                        principalTable: "ContentLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentTags",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContentId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    CreatedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedByBrowserName = table.Column<string>(maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentTags_Contents",
                        column: x => x.ContentId,
                        principalSchema: "dbo",
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentTags_Tags",
                        column: x => x.TagId,
                        principalSchema: "dbo",
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentListFiles_ContentListId",
                table: "ContentListFiles",
                column: "ContentListId");

            migrationBuilder.CreateIndex(
                name: "IX_AppDataProtectionKeys_FriendlyName",
                schema: "dbo",
                table: "AppDataProtectionKeys",
                column: "FriendlyName",
                unique: true,
                filter: "[FriendlyName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AppRoleClaims_RoleId",
                schema: "dbo",
                table: "AppRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "dbo",
                table: "AppRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "Index_ExpiresAtTime",
                schema: "dbo",
                table: "AppSqlCache",
                column: "ExpiresAtTime");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserClaims_UserId",
                schema: "dbo",
                table: "AppUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserLogins_UserId",
                schema: "dbo",
                table: "AppUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserRoles_RoleId",
                schema: "dbo",
                table: "AppUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "dbo",
                table: "AppUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "dbo",
                table: "AppUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserUsedPasswords_UserId",
                schema: "dbo",
                table: "AppUserUsedPasswords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryComments_CategoryId",
                schema: "dbo",
                table: "CategoryComments",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryComments_CommentId",
                schema: "dbo",
                table: "CategoryComments",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Categorys_ParentId",
                schema: "dbo",
                table: "Categorys",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTags_CategoryId",
                schema: "dbo",
                table: "CategoryTags",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTags_TagId",
                schema: "dbo",
                table: "CategoryTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AttachmentId",
                schema: "dbo",
                table: "Comments",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentId",
                schema: "dbo",
                table: "Comments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserCommentId",
                schema: "dbo",
                table: "Comments",
                column: "UserCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentComments_CommentId",
                schema: "dbo",
                table: "ContentComments",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentComments_ContentId",
                schema: "dbo",
                table: "ContentComments",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentFiles_ContentId",
                schema: "dbo",
                table: "ContentFiles",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentListComments_CommentId",
                schema: "dbo",
                table: "ContentListComments",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentListComments_ContentListId",
                schema: "dbo",
                table: "ContentListComments",
                column: "ContentListId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentListTags_ContentListId",
                schema: "dbo",
                table: "ContentListTags",
                column: "ContentListId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentListTags_TagId",
                schema: "dbo",
                table: "ContentListTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentRelateds_ContentId",
                schema: "dbo",
                table: "ContentRelateds",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentRelateds_RelatedId",
                schema: "dbo",
                table: "ContentRelateds",
                column: "RelatedId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_CategoryId",
                schema: "dbo",
                table: "Contents",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentSelecteds_ContentId",
                schema: "dbo",
                table: "ContentSelecteds",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentSelecteds_ContentListId",
                schema: "dbo",
                table: "ContentSelecteds",
                column: "ContentListId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTags_ContentId",
                schema: "dbo",
                table: "ContentTags",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTags_TagId",
                schema: "dbo",
                table: "ContentTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomUserTokens_UserId",
                schema: "dbo",
                table: "CustomUserTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_UserId",
                schema: "dbo",
                table: "Devices",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PodcastComments_CommentId",
                schema: "dbo",
                table: "PodcastComments",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_PodcastComments_PodcastId",
                schema: "dbo",
                table: "PodcastComments",
                column: "PodcastId");

            migrationBuilder.CreateIndex(
                name: "IX_PodcastTags_PodcastId",
                schema: "dbo",
                table: "PodcastTags",
                column: "PodcastId");

            migrationBuilder.CreateIndex(
                name: "IX_PodcastTags_TagId",
                schema: "dbo",
                table: "PodcastTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_TagComments_CommentId",
                schema: "dbo",
                table: "TagComments",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_TagComments_TagId",
                schema: "dbo",
                table: "TagComments",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentListFiles");

            migrationBuilder.DropTable(
                name: "AppDataProtectionKeys",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AppLogItems",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AppRoleClaims",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AppSettings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AppSqlCache",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AppUserClaims",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AppUserLogins",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AppUserRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AppUserTokens",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AppUserUsedPasswords",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CategoryComments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CategoryTags",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ContactUs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ContentComments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ContentFiles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ContentListComments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ContentListTags",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ContentRelateds",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ContentSelecteds",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ContentTags",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CustomUserTokens",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Devices",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Orders",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PodcastComments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PodcastTags",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SessionRequests",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Sliders",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TagComments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AppRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ContentLists",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Contents",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Podcasts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Comments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Categorys",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "AppUsers",
                schema: "dbo");
        }
    }
}
