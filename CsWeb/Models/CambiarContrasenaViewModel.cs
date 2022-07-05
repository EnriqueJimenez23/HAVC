using System.ComponentModel.DataAnnotations;

namespace CsWeb.Models
{
    public class CambiarContrasenaViewModel
    {
        public int UsuarioId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña:")]
        public string NuevaContrasena { get; set; }

        [Required]
        [RegularExpression("^.{6,}$", ErrorMessage = "La contraseña debe tener mimino 6 caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar nueva contraseña:")]
        [Compare("NuevaContrasena", ErrorMessage = "El campo Nueva contraseña y el campo Confirmar nueva contraseña no coinciden.")]
        public string ConfirmarNuevaContrasena { get; set; }
    }
}