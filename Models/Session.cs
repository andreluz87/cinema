    using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Models
{
    public class Session
    {
        public static int _daysForDelete = 10;
        public int SessionId { get; set; }
        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Hora de Início")]
        [Required(ErrorMessage = "Date Required")]
        [DataType(DataType.Time)]
        public TimeSpan? Start { get; set; }
        [Display(Name = "Hora final")]
        public TimeSpan? End { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Valor do Ingresso")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal TicketValue { get; set; }

        //2D: 0, 3D: 1
        [Display(Name = "Tipo do Filme")]
        public int AnimationType { get; set; }

        //Original: 0 ou dublado: 1
        public int Audio { get; set; }
        
        [Display(Name = "Filme")]
        [Required]
        [RegularExpression(@"^[1-9][0-9]*$",
         ErrorMessage = "Um filme deve ser selecionado")]
        public int MovieId { get; set; }
        
        [Display(Name = "Sala")]
        [Required]
        [RegularExpression(@"^[1-9][0-9]*$",
         ErrorMessage = "Uma sala deve ser selecionada")]
        public int RoomId { get; set; }
        //Referencia MovieId a tabela Movie
        [Display(Name = "Filme")]
        public virtual Movie Movie { get; set; }
        //Referencia RoomId a tabela Room
        [Display(Name = "Sala")]
        public virtual Room Room { get; set; }
    }
}
