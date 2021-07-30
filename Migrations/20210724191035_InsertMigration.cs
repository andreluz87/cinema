using Microsoft.EntityFrameworkCore.Migrations;

namespace Cinema.Migrations
{
    public partial class InsertMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Movies(Title, Image, Description, Duration) VALUES ('Um Lugar Silencioso II', 'aquit.jpg', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed a sapien neque. Nulla lacinia eget neque quis rutrum. Quisque suscipit quam non erat efficitur, quis aliquam erat ornare.', '02:10:00') ");
            migrationBuilder.Sql("INSERT INTO Movies(Title, Image, Description, Duration) VALUES ('Cruella', 'cruella.jpg', 'Nullam consectetur arcu ac enim posuere fringilla. Quisque eu nisl tellus. Quisque placerat purus non nisl ultrices congue. Nunc aliquam a nunc sed lobortis. Integer tempus orci eu mi dapibus, eleifend pulvinar enim consequat.', '01:54:00') ");
            migrationBuilder.Sql("INSERT INTO Movies(Title, Image, Description, Duration) VALUES ('Veloses e Furiosos 9', 'furious.jpg', 'Pellentesque ut diam eget purus pharetra porta. Cras rhoncus iaculis elit, non commodo tellus euismod ac. Etiam et mattis lacus. Cras bibendum ex sed eros sollicitudin faucibus.', '02:18:00') ");
            migrationBuilder.Sql("INSERT INTO Movies(Title, Image, Description, Duration) VALUES ('Viuva Negra', 'viuva.jpg', 'Cras ut magna nisi. Ut ullamcorper magna sed neque mollis, ornare.', '01:57:00') ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Movies");
            migrationBuilder.Sql("DELETE FROM Rooms");
            migrationBuilder.Sql("DELETE FROM Sessions");
        }
    }
}
