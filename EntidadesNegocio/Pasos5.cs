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
    public class Pasos5 : Entity
    {
        public int Pasos5Id { get; set; }
        public int PasosId { get; set; }

        [StringLength(128)]
        [Display(Name = "Tipo indicador:")]
        public string TipoIndicador { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Objetivo del proyecto:")]
        public string ObjetivoProyecto { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Evaluación cualitativa:")]
        public string EvaluacionCualitativa { get; set; }

        [StringLength(2)]
        [Display(Name = "Cumple: ")]
        public string Cumple { get; set; }

        [Display(Name = "Fecha evaluación:")]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaEvaluacion { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; }

        public string Usuario { get; set; }
        public DateTime? FechaCreado { get; set; }
        public DateTime? FechaModifico { get; set; }
        [NotMapped]
        public int NumPaso { get; set; }       
    }
}
