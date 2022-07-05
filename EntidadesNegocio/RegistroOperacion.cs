using System;
using System.ComponentModel.DataAnnotations;
using CapaDatos.Repositorio.EF6;

namespace CapaDominio.EntidadesNegocio
{
    public enum CategoriaRegistroOperacion
    {
        [Display(Name = "Veedor")]
        Veedor = 0,
        [Display(Name = "ContenidoSitio")]
        ContenidoSitio = 1,
        [Display(Name = "Proyecto")]
        Proyecto = 2,
        [Display(Name = "Usuario")]
        Usuario = 3,
       
    }

    public class RegistroOperacion : Entity
    {
        public int RegistroOperacionId { get; set; }

        [Required]
        [Display(Name = "Categoría:")]
        public CategoriaRegistroOperacion Categoria { get; set; }
        
        [Required]
        [Display(Name = "Id del registro:")]
        public int RegistroId { get; set; } 
        
        [Required]
        [Display(Name = "Usuario:")]
        public string Usuario { get; set; }

        [Required]
        [Display(Name = "Nombre del usuario:")]
        public string NombreUsuario { get; set; }

        [Required]
        [Display(Name = "Descripción operación:")]
        public string DescripcionOperacion { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de la operación")]
        public DateTime FechaOperacion { get; set; }

        public override string ToString()
        {
            return string.Format("Descripción operación: {0}\nUsuario: {1}({2})\nFecha operación:{3}\n\n", DescripcionOperacion, NombreUsuario, Usuario, FechaOperacion);
        }
    }
}
