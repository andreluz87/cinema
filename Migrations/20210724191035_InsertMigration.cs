using Microsoft.EntityFrameworkCore.Migrations;

namespace Cinema.Migrations
{
    public partial class InsertMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Rooms([Name], SeatNumber) VALUES ('Sala 1', 40)");
            migrationBuilder.Sql("INSERT INTO Rooms([Name], SeatNumber) VALUES ('Sala 2', 50)");
            migrationBuilder.Sql("INSERT INTO Rooms([Name], SeatNumber) VALUES ('Sala 3', 60)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Movies");
            migrationBuilder.Sql("DELETE FROM Rooms");
            migrationBuilder.Sql("DELETE FROM Sessions");
        }
    }
}
