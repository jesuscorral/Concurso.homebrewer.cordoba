using System.ComponentModel.DataAnnotations;

namespace BeerContest.Domain.Models
{
    public class BeerRegistrationModel
    {
        [Required(ErrorMessage = "El número de socio ACCE es obligatorio")]
        [Display(Name = "Número socio ACCE")]
        public string ACCEMemberNumber { get; set; }

        [Required(ErrorMessage = "El nombre y apellidos son obligatorios")]
        [Display(Name = "Nombre y apellidos")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [Display(Name = "Fecha de nacimiento")]
        [CustomValidation(typeof(BeerRegistrationModel), nameof(ValidateAge))]
        public DateTime BirthDate { get; set; } = DateTime.Now.AddYears(-18);

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        [MinLength(9, ErrorMessage = "El formato del teléfono no es válido")]
        [Display(Name = "Teléfono")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        [Display(Name = "Categoría")]
        public BeerCategory Category { get; set; } = BeerCategory.ClassicStyles;

        [Required(ErrorMessage = "El estilo de cerveza es obligatorio")]
        [Display(Name = "Estilo de cerveza")]
        public string BeerStyle { get; set; }

        [Required(ErrorMessage = "Grado de alcohol es obligatoria")]
        [Display(Name = "Grado de alcohol (%)")]
        public double AlcoholContent { get; set; }

        [Required(ErrorMessage = "Fecha de elaboración es obligatoria")]
        [Display(Name = "Fecha de elaboración")]
        public DateTime ElaborationDate { get; set; }

        [Required(ErrorMessage = "Fecha de embotellado es obligatorio")]
        [Display(Name = "Fecha de embotellado")]
        public DateTime BottleDate { get; set; } 

        [Required(ErrorMessage = "Maltas utilizadas es obligatorio")]
        [Display(Name = "Maltas utilizadas")]
        public string Malts { get; set; }

        [Required(ErrorMessage = "Lúpulos utilizados es obligatorio")]
        [Display(Name = "Lúpulos utilizados")]
        public string Hops { get; set; }

        [Required(ErrorMessage = "Levaduras utilizadas es obligatorio")]
        [Display(Name = "Levaduras utilizadas")]
        public string Yeast { get; set; }

        [Display(Name = "Otros ingredientes")]
        public string Additives { get; set; }

        [Display(Name = "Instrucciones de entrada")]
        public string EntryInstructions { get; set; }

        public static ValidationResult? ValidateAge(DateTime birthDate, ValidationContext context)
        {
            var age = DateTime.Now.Year - birthDate.Year;
            if (birthDate > DateTime.Now.AddYears(-age)) age--;

            return age >= 18
                ? ValidationResult.Success
                : new ValidationResult("Debe ser mayor de 18 años.");
        }
    }
}
