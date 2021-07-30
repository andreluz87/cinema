using System;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class Movie
    {
        public int MovieId { get; set; }

        [StringLength(255, MinimumLength = 3, ErrorMessage = "O {0} deve conter no mínimo {2} caracteres)")]
        [Display(Name = "Título")]
        [Required(ErrorMessage = "Título é obrigatório.")]
        public string Title { get; set; }

        [StringLength(255)]
        [Display(Name = "Imagem")]
        [Required(ErrorMessage = "Imagem do filme é obrigatória.")]
        public string Image { get; set; } 

        [StringLength(255)]
        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [Display(Name = "Duração")]
        [Required(ErrorMessage = "A duração do filme é obrigatória.")]
        [DataType(DataType.Time)]
        public TimeSpan? Duration { get; set; }
    }
}
