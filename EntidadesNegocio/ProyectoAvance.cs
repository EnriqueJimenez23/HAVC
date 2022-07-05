using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CapaDatos.Repositorio.EF6;


namespace CapaDominio.EntidadesNegocio
{
    public class ProyectoAvance : Entity
    {
        public int ProyectoAvanceId { get; set; }
        public int ProyectoId { get; set; }
        public string Indicador { get; set; }
        public string Avance{ get; set; }
        public string Meta{ get; set; }
        public string Estado { get; set; }
        [Display(Name = "Fecha avance:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaAvance { get; set; }
        [Display(Name = "Fecha meta:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaMeta { get; set; }
        public string Usuario { get; set; }
        public DateTime? FechaCreado { get; set; }
        [NotMapped]
        public string NombreProyecto { get; set; }
    }
}
