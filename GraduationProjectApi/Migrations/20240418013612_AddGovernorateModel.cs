using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IdentityManagerServerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddGovernorateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Governorate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Governorate", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Governorate",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "القاهرة" },
                    { 2, "الإسكندرية" },
                    { 3, "البحيرة" },
                    { 4, "بورسعيد" },
                    { 5, "الإسماعيلية" },
                    { 6, "الغربية" },
                    { 7, "المنوفية" },
                    { 8, "الدقهلية" },
                    { 9, "كفر الشيخ" },
                    { 10, "شمال سيناء" },
                    { 11, "جنوب سيناء" },
                    { 12, "السويس" },
                    { 13, "الأقصر" },
                    { 14, "أسوان" },
                    { 15, "البحر الأحمر" },
                    { 16, "سوهاج" },
                    { 17, "قنا" },
                    { 18, "الفيوم" },
                    { 19, "بني سويف" },
                    { 20, "المنيا" },
                    { 21, "أسيوط" },
                    { 22, "دمياط" },
                    { 23, "الشرقية" },
                    { 24, "الجيزة" },
                    { 25, "الوادي الجديد" },
                    { 26, "سومة القناطر" },
                    { 27, "الأقصر" },
                    { 28, "الوادي الجديد" },
                    { 29, "أسوان" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Governorate");
        }
    }
}
