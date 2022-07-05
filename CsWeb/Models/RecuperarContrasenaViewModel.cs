using System.ComponentModel.DataAnnotations;

namespace CsWeb.Models
{
    public class RecuperarContrasenaViewModel
    {
        #region Properties

        [Required]
        [StringLength(512)]
        [EmailAddress(ErrorMessage = "Por favor ingrese un correo valido.")]
        [Display(Name = "Correo electrónico:")]
        public string CorreoElectronico { get; set; }

        #endregion

        #region Constructor

        public RecuperarContrasenaViewModel()
        {
            CorreoElectronico = string.Empty;
        }

        #endregion
    }
}
