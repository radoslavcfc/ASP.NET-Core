using Microsoft.EntityFrameworkCore.Migrations;

namespace Agency.Web.Migrations
{
    public partial class oneToOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Worker_WorkerId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactInfo_Address_AddressId",
                table: "ContactInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_Worker_ContactInfo_ContactInfoId",
                table: "Worker");

            migrationBuilder.DropForeignKey(
                name: "FK_Worker_Names_NamesId",
                table: "Worker");

            migrationBuilder.DropForeignKey(
                name: "FK_Worker_Nationality_NationalityId",
                table: "Worker");

            migrationBuilder.DropForeignKey(
                name: "FK_Worker_NewWorkerInfo_NewWorkerInfoId",
                table: "Worker");

            migrationBuilder.DropForeignKey(
                name: "FK_Worker_ReturneeInfo_ReturneeInfoId",
                table: "Worker");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Worker",
                table: "Worker");

            migrationBuilder.DropIndex(
                name: "IX_Worker_ContactInfoId",
                table: "Worker");

            migrationBuilder.DropIndex(
                name: "IX_Worker_NamesId",
                table: "Worker");

            migrationBuilder.DropIndex(
                name: "IX_Worker_NationalityId",
                table: "Worker");

            migrationBuilder.DropIndex(
                name: "IX_Worker_NewWorkerInfoId",
                table: "Worker");

            migrationBuilder.DropIndex(
                name: "IX_Worker_ReturneeInfoId",
                table: "Worker");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReturneeInfo",
                table: "ReturneeInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NewWorkerInfo",
                table: "NewWorkerInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Nationality",
                table: "Nationality");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactInfo",
                table: "ContactInfo");

            migrationBuilder.DropIndex(
                name: "IX_ContactInfo_AddressId",
                table: "ContactInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Address",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "ContactInfoId",
                table: "Worker");

            migrationBuilder.DropColumn(
                name: "NamesId",
                table: "Worker");

            migrationBuilder.DropColumn(
                name: "NationalityId",
                table: "Worker");

            migrationBuilder.DropColumn(
                name: "NeWorkerInfoId",
                table: "Worker");

            migrationBuilder.DropColumn(
                name: "NewWorkerInfoId",
                table: "Worker");

            migrationBuilder.DropColumn(
                name: "ReturneeInfoId",
                table: "Worker");

            migrationBuilder.RenameTable(
                name: "Worker",
                newName: "Workers");

            migrationBuilder.RenameTable(
                name: "ReturneeInfo",
                newName: "ReturneeInfos");

            migrationBuilder.RenameTable(
                name: "NewWorkerInfo",
                newName: "NewWorkerInfos");

            migrationBuilder.RenameTable(
                name: "Nationality",
                newName: "Nationalities");

            migrationBuilder.RenameTable(
                name: "ContactInfo",
                newName: "ContactInfos");

            migrationBuilder.RenameTable(
                name: "Address",
                newName: "Addresses");

            migrationBuilder.AddColumn<string>(
                name: "WorkerId",
                table: "Names",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkerId",
                table: "ReturneeInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkerId",
                table: "NewWorkerInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkerId",
                table: "Nationalities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkerId",
                table: "ContactInfos",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Workers",
                table: "Workers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReturneeInfos",
                table: "ReturneeInfos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NewWorkerInfos",
                table: "NewWorkerInfos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Nationalities",
                table: "Nationalities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactInfos",
                table: "ContactInfos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Names_IsDeleted",
                table: "Names",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Names_WorkerId",
                table: "Names",
                column: "WorkerId",
                unique: true,
                filter: "[WorkerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_IsDeleted",
                table: "AspNetUsers",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_IsDeleted",
                table: "AspNetRoles",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_IsDeleted",
                table: "Workers",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ReturneeInfos_IsDeleted",
                table: "ReturneeInfos",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ReturneeInfos_WorkerId",
                table: "ReturneeInfos",
                column: "WorkerId",
                unique: true,
                filter: "[WorkerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_NewWorkerInfos_IsDeleted",
                table: "NewWorkerInfos",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_NewWorkerInfos_WorkerId",
                table: "NewWorkerInfos",
                column: "WorkerId",
                unique: true,
                filter: "[WorkerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Nationalities_IsDeleted",
                table: "Nationalities",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Nationalities_WorkerId",
                table: "Nationalities",
                column: "WorkerId",
                unique: true,
                filter: "[WorkerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInfos_AddressId",
                table: "ContactInfos",
                column: "AddressId",
                unique: true,
                filter: "[AddressId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInfos_IsDeleted",
                table: "ContactInfos",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInfos_WorkerId",
                table: "ContactInfos",
                column: "WorkerId",
                unique: true,
                filter: "[WorkerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_IsDeleted",
                table: "Addresses",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Workers_WorkerId",
                table: "AspNetUsers",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactInfos_Addresses_AddressId",
                table: "ContactInfos",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactInfos_Workers_WorkerId",
                table: "ContactInfos",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Names_Workers_WorkerId",
                table: "Names",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Nationalities_Workers_WorkerId",
                table: "Nationalities",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NewWorkerInfos_Workers_WorkerId",
                table: "NewWorkerInfos",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReturneeInfos_Workers_WorkerId",
                table: "ReturneeInfos",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Workers_WorkerId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactInfos_Addresses_AddressId",
                table: "ContactInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactInfos_Workers_WorkerId",
                table: "ContactInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_Names_Workers_WorkerId",
                table: "Names");

            migrationBuilder.DropForeignKey(
                name: "FK_Nationalities_Workers_WorkerId",
                table: "Nationalities");

            migrationBuilder.DropForeignKey(
                name: "FK_NewWorkerInfos_Workers_WorkerId",
                table: "NewWorkerInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturneeInfos_Workers_WorkerId",
                table: "ReturneeInfos");

            migrationBuilder.DropIndex(
                name: "IX_Names_IsDeleted",
                table: "Names");

            migrationBuilder.DropIndex(
                name: "IX_Names_WorkerId",
                table: "Names");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_IsDeleted",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_IsDeleted",
                table: "AspNetRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Workers",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_Workers_IsDeleted",
                table: "Workers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReturneeInfos",
                table: "ReturneeInfos");

            migrationBuilder.DropIndex(
                name: "IX_ReturneeInfos_IsDeleted",
                table: "ReturneeInfos");

            migrationBuilder.DropIndex(
                name: "IX_ReturneeInfos_WorkerId",
                table: "ReturneeInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NewWorkerInfos",
                table: "NewWorkerInfos");

            migrationBuilder.DropIndex(
                name: "IX_NewWorkerInfos_IsDeleted",
                table: "NewWorkerInfos");

            migrationBuilder.DropIndex(
                name: "IX_NewWorkerInfos_WorkerId",
                table: "NewWorkerInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Nationalities",
                table: "Nationalities");

            migrationBuilder.DropIndex(
                name: "IX_Nationalities_IsDeleted",
                table: "Nationalities");

            migrationBuilder.DropIndex(
                name: "IX_Nationalities_WorkerId",
                table: "Nationalities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactInfos",
                table: "ContactInfos");

            migrationBuilder.DropIndex(
                name: "IX_ContactInfos_AddressId",
                table: "ContactInfos");

            migrationBuilder.DropIndex(
                name: "IX_ContactInfos_IsDeleted",
                table: "ContactInfos");

            migrationBuilder.DropIndex(
                name: "IX_ContactInfos_WorkerId",
                table: "ContactInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_IsDeleted",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "Names");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "ReturneeInfos");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "NewWorkerInfos");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "Nationalities");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "ContactInfos");

            migrationBuilder.RenameTable(
                name: "Workers",
                newName: "Worker");

            migrationBuilder.RenameTable(
                name: "ReturneeInfos",
                newName: "ReturneeInfo");

            migrationBuilder.RenameTable(
                name: "NewWorkerInfos",
                newName: "NewWorkerInfo");

            migrationBuilder.RenameTable(
                name: "Nationalities",
                newName: "Nationality");

            migrationBuilder.RenameTable(
                name: "ContactInfos",
                newName: "ContactInfo");

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "Address");

            migrationBuilder.AddColumn<string>(
                name: "ContactInfoId",
                table: "Worker",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NamesId",
                table: "Worker",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NationalityId",
                table: "Worker",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NeWorkerInfoId",
                table: "Worker",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewWorkerInfoId",
                table: "Worker",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReturneeInfoId",
                table: "Worker",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Worker",
                table: "Worker",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReturneeInfo",
                table: "ReturneeInfo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NewWorkerInfo",
                table: "NewWorkerInfo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Nationality",
                table: "Nationality",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactInfo",
                table: "ContactInfo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Address",
                table: "Address",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Worker_ContactInfoId",
                table: "Worker",
                column: "ContactInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Worker_NamesId",
                table: "Worker",
                column: "NamesId");

            migrationBuilder.CreateIndex(
                name: "IX_Worker_NationalityId",
                table: "Worker",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Worker_NewWorkerInfoId",
                table: "Worker",
                column: "NewWorkerInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Worker_ReturneeInfoId",
                table: "Worker",
                column: "ReturneeInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInfo_AddressId",
                table: "ContactInfo",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Worker_WorkerId",
                table: "AspNetUsers",
                column: "WorkerId",
                principalTable: "Worker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactInfo_Address_AddressId",
                table: "ContactInfo",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Worker_ContactInfo_ContactInfoId",
                table: "Worker",
                column: "ContactInfoId",
                principalTable: "ContactInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Worker_Names_NamesId",
                table: "Worker",
                column: "NamesId",
                principalTable: "Names",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Worker_Nationality_NationalityId",
                table: "Worker",
                column: "NationalityId",
                principalTable: "Nationality",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Worker_NewWorkerInfo_NewWorkerInfoId",
                table: "Worker",
                column: "NewWorkerInfoId",
                principalTable: "NewWorkerInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Worker_ReturneeInfo_ReturneeInfoId",
                table: "Worker",
                column: "ReturneeInfoId",
                principalTable: "ReturneeInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
