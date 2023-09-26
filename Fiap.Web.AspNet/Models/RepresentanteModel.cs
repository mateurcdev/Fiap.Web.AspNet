using System.ComponentModel.DataAnnotations;

namespace Fiap.Web.AspNet.Models
{

    public class RepresentanteModel
    {

        public int RepresentanteId { get; set; }

        [Required(ErrorMessage = "Nome do representante é obrigatório!")]
        [StringLength(80,
            MinimumLength = 2,
            ErrorMessage = "O nome deve ter, no mínimo, 2 e, no máximo, 80 caracteres")]
        [Display(Name = "Nome do Representante")]
        public string? NomeRepresentante { get; set; }

        [Required(ErrorMessage = "CPF é obrigatório!")]
        [Display(Name = "CPF")]
        public string? Cpf { get; set; }

        public string? Token { get; set; }

        public RepresentanteModel()
        {

        }

        public RepresentanteModel(int representanteId, string nomeRepresentante)
        {
            RepresentanteId = representanteId;
            NomeRepresentante = nomeRepresentante;
        }

        public RepresentanteModel(int representanteId, string cpf, string nomeRepresentante)
        {
            RepresentanteId = representanteId;
            Cpf = cpf;
            NomeRepresentante = nomeRepresentante;
        }
    }
}
