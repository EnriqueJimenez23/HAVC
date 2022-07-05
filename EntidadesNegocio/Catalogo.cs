using System.ComponentModel.DataAnnotations;
using CapaDatos.Repositorio.EF6;

namespace CapaDominio.EntidadesNegocio
{
    public enum CategoriaCatalogo
    {
        [Display(Name = "Zonas geográficas")]
        ZonasGeograficas = 0,
        [Display(Name = "Tipos de identificación")]
        TiposIdentificacion = 1,
        [Display(Name = "Subregion")]
        Subregion = 2,
        [Display(Name = "Punto")]
        Punto = 3,
        [Display(Name = "PilarPDET")]
        PilarPDET = 4,
        [Display(Name = "EstadoProyecto")]
        EstadoProyecto = 5,
        [Display(Name = "ActividadCronograma")]
        ActividadCronograma = 6,
    }

    public class Catalogo : Entity
    {
        public int CatalogoId { get; set; }

        [Display(Name = "Categoría padre:")]
        public int CatalogoPadreId { get; set; }

        [Required]
        [Display(Name = "Categoría:")]
        public CategoriaCatalogo Categoria { get; set; }
        
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
