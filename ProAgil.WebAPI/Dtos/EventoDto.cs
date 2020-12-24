using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProAgil.WebAPI.Dtos
{
    public class EventoDto
    {
        public int Id  { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Local  { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string DataEvento  { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Tema  { get; set; }
        [Required]
        [Range(2, 120000, ErrorMessage = "O campo {0} deverá ser entre 2 e 120.000 caracteres")]
        public int QtdPessoas { get; set; }                 
        public string Telefone { get; set; }
        [EmailAddress(ErrorMessage = "O campo {0} não é um forma válido")]
        public string Email { get; set; }         
        public string ImageURL{ get; set; }
        public List<LoteDto> Lotes { get; set; }
        public List<RedeSocialDto> RedesSociais { get; set; }
        public List<PalestranteDto> Palestrantes { get; set; }
    }
}