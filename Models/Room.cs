using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        [StringLength(100)]
        [Display(Name = "#")]
        public string Name { get; set; }
        [Display(Name = "Quantidade de assentos")]
        public int SeatNumber { get; set; }
    }
}
