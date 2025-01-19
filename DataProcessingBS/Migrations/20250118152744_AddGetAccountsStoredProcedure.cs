using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    /// <inheritdoc />
    public partial class AddGetAccountsStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var UpdateProcedure = "CREATE PROCEDURE [dbo].[GetAccounts]\nAs\nBEGIN\n\tSET NOCOUNT ON;\n SELECT * FROM [dbo].[Accounts]\nEND;";
            migrationBuilder.Sql(UpdateProcedure);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var dropStatement = "DROP PROCEDURE [dbo].[GetAccounts];";
            migrationBuilder.Sql(dropStatement);
        }
    }
}
