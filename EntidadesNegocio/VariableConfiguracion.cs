using System.ComponentModel.DataAnnotations;
using CapaDatos.Repositorio.EF6;

namespace CapaDominio.EntidadesNegocio
{
    public enum CategoriaVariableConfiguracion
    {
        [Display(Name="Valor por defecto")]
        ValorPorDefecto = 0,
        [Display(Name="Configuración correo electrónico")]
        ConfiguracionCorreoElectronico = 1,
        [Display(Name="Mensaje")]
        Mensaje = 2,
       
    }

    public class VariableConfiguracion : Entity
    {
        public int VariableConfiguracionId { get; set; }

        [Required]
        public CategoriaVariableConfiguracion Categoria { get; set; }
        
        [Required]
        [StringLength(512)]
        [Display(Name = "Nombre:")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Valor:")]
        public string Valor { get; set; }

        [StringLength(4000)]
        [Display(Name = "Descripción:")]
        public string Descripcion { get; set; }
    }
}
