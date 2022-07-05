using System.ComponentModel.DataAnnotations;
using System.Web.Http.Results;

namespace CsWeb.Models
{
    public class InicioSesionViewModel
    {
        [Required]
        [StringLength(512)]        
        //[EmailAddress(ErrorMessage = "Por favor ingrese un correo valido.")]
        [Display(Name = "Usuario:")]
        public string NombreUsuario { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña:")]
        public string Contrasena { get; set; }
        
        [Display(Name = "Recordarme")]        
        public bool Recordarme { get; set; }
    }

}
