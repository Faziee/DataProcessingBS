using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIsActiveToBool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
UPDATE ApiKeys
        SET Is_Active = CASE
            WHEN Is_Active IN ('yes', 'true', '1') THEN '1'
            WHEN Is_Active IN ('no', 'false', '0') THEN '0'
            ELSE '0'
        END;");

            migrationBuilder.AlterColumn<bool>(
                name: "Is_Active",
                table: "ApiKeys",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Is_Active",
                table: "ApiKeys",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
