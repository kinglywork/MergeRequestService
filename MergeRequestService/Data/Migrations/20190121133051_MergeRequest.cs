using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MergeRequestService.Data.Migrations
{
    public partial class MergeRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MailSendingJobs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    CronExpression = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailSendingJobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MergeRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SourceBranch = table.Column<string>(nullable: false),
                    TargetBranch = table.Column<string>(nullable: false),
                    ChangeSetId = table.Column<int>(nullable: false),
                    DevChangeSetId = table.Column<int>(nullable: true),
                    Reviewer = table.Column<string>(nullable: true),
                    Memo = table.Column<string>(nullable: true),
                    SubmitBy = table.Column<string>(nullable: true),
                    SubmitAt = table.Column<DateTime>(nullable: false),
                    UpdateBy = table.Column<string>(nullable: true),
                    UpdateAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MergeRequests", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MailSendingJobs");

            migrationBuilder.DropTable(
                name: "MergeRequests");
        }
    }
}
