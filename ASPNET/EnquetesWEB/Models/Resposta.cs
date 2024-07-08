using System.ComponentModel.DataAnnotations;

namespace EnquetesWEB.Models
{
    public class Resposta
    {
        [Required(ErrorMessage ="O campo Nome deve ser preenchido!!!")]
        public string Nome {get; set;}
        [Required(ErrorMessage ="O campo Email deve ser preenchido!!!")]
        [EmailAddress(ErrorMessage ="O campo Email não corresponde a um endereco valido")]
        public string Email {get; set;}
        [Required(ErrorMessage ="O campo Sim deve ser preenchido!!!")]
        public bool? Sim {get; set;}

    
    }
}