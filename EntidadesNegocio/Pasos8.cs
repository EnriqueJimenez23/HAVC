using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Linq;
using System.Text;
using CapaDatos.Repositorio.EF6;
using CapaServicios.Servicios;
using System.Collections.Generic;

namespace CapaDominio.EntidadesNegocio
{
    public class Pasos8 : Entity
    {
        public int Pasos8Id { get; set; }
        public int PasosId { get; set; }

        [StringLength (20)]
        [Display(Name = "Número informe:")]
        public string NumeroInforme { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Actividad: ")]
        public string Actividad { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Plan de seguimiento:")]
        public string PlanSeguimiento { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Acciones después de seguimiento: ")]
        public string AccionesDespuesSeguimiento { get; set; }

       
        [Display(Name = "Fecha próximo seguimiento:")]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaProximoSeguimiento { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; }

        public string Usuario { get; set; }
        public DateTime? FechaCreado { get; set; }
        public DateTime? FechaModifico { get; set; }
        [NotMapped]
        public int NumPaso { get; set; }
        [NotMapped]
        public string NombreObjeto { get; set; }

    }
}
