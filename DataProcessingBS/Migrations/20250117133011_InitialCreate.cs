using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    account_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Payment_Method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Blocked = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Is_Invited = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Trial_End_Date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.account_id);
                });

            migrationBuilder.CreateTable(
                name: "ApiKeys",
                columns: table => new
                {
                    ApiKey_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Account_Id = table.Column<int>(type: "int", nullable: false),
                    Create_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Is_Active = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiKeys", x => x.ApiKey_Id);
                });

            migrationBuilder.CreateTable(
                name: "Episodes",
                columns: table => new
                {
                    Episode_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Media_Id = table.Column<int>(type: "int", nullable: false),
                    Series_Id = table.Column<int>(type: "int", nullable: false),
                    Season_Number = table.Column<int>(type: "int", nullable: false),
                    Episode_Number = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episodes", x => x.Episode_Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Genre_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Genre_Id);
                });

            migrationBuilder.CreateTable(
                name: "Invitations",
                columns: table => new
                {
                    Invitation_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Inviter_Id = table.Column<int>(type: "int", nullable: false),
                    Invitee_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => x.Invitation_Id);
                });

            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    Media_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Genre_Id = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age_Rating = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quality = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.Media_Id);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Movie_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Media_Id = table.Column<int>(type: "int", nullable: false),
                    Has_Subtitles = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Movie_Id);
                });

            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    Series_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Genre_Id = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age_Rating = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.Series_Id);
                });

            migrationBuilder.CreateTable(
                name: "Subtitles",
                columns: table => new
                {
                    Subtitle_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Media_Id = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subtitles", x => x.Subtitle_Id);
                });

            migrationBuilder.CreateTable(
                name: "Watches",
                columns: table => new
                {
                    Watch_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Profile_Id = table.Column<int>(type: "int", nullable: false),
                    Media_Id = table.Column<int>(type: "int", nullable: false),
                    Watch_Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pause_Time = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Watches", x => x.Watch_Id);
                });

            migrationBuilder.CreateTable(
                name: "WatchLists",
                columns: table => new
                {
                    WatchList_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Profile_Id = table.Column<int>(type: "int", nullable: false),
                    Media_Id = table.Column<int>(type: "int", nullable: false),
                    Added_Date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchLists", x => x.WatchList_Id);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Profile_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Account_Id = table.Column<int>(type: "int", nullable: false),
                    Profile_Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Child_Profile = table.Column<bool>(type: "bit", nullable: false),
                    User_Age = table.Column<int>(type: "int", nullable: true),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Profile_Id);
                    table.ForeignKey(
                        name: "FK_Profiles_Accounts_Account_Id",
                        column: x => x.Account_Id,
                        principalTable: "Accounts",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Subscription_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Account_Id = table.Column<int>(type: "int", nullable: false),
                    Subscription_Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Start_Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Renewal_Date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Subscription_Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Accounts_Account_Id",
                        column: x => x.Account_Id,
                        principalTable: "Accounts",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_Account_Id",
                table: "Profiles",
                column: "Account_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_Account_Id",
                table: "Subscriptions",
                column: "Account_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiKeys");

            migrationBuilder.DropTable(
                name: "Episodes");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Invitations");

            migrationBuilder.DropTable(
                name: "Media");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Series");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Subtitles");

            migrationBuilder.DropTable(
                name: "Watches");

            migrationBuilder.DropTable(
                name: "WatchLists");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
