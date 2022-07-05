using System.ComponentModel.DataAnnotations;
using CapaDatos.Repositorio.EF6;

namespace CapaDominio.EntidadesNegocio
{
    

    public class CatalogoT : Entity
    {
        public int CatalogoTId { get; set; }

        [Display(Name = "Categoría padre:")]
        public string Padre { get; set; }

        [Required]
        [Display(Name = "Categoría:")]
        public string Categoria { get; set; }
        
        [Required]
        [Display(Name = "Etiqueta:")]
        public string Etiqueta { get; set; }

        [Required]
        [Display(Name = "Valor:")]
        public string Valor { get; set; }

        [Display(Name = "Descripción:")]
        public string Descripcion { get; set; }
    }
}
