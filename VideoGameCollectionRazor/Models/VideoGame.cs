using System.ComponentModel.DataAnnotations;

namespace VideoGameCollection.Models
{
    public class VideoGame
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Il titolo è obbligatorio")]
        [StringLength(200)]
        [Display(Name = "Titolo")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "La piattaforma è obbligatoria")]
        [Display(Name = "Piattaforma")]
        public string Platform { get; set; } = string.Empty;

        [Range(1, 10, ErrorMessage = "Il voto deve essere tra 1 e 10")]
        [Display(Name = "Voto Personale")]
        public int PersonalScore { get; set; } = 5;

        [Display(Name = "Genere")]
        public string? Genre { get; set; }

        [Display(Name = "Immagine")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Data Aggiunta")]
        public DateTime AddedDate { get; set; } = DateTime.Now;

        [Display(Name = "Note")]
        public string? Notes { get; set; }

        // ID esterno da RAWG (per evitare duplicati)
        public int? ExternalId { get; set; }
    }
}
