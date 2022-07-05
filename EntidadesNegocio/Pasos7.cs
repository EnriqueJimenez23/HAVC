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
    public class Pasos7 : Entity
    {
        public int Pasos7Id { get; set; }
        public int PasosId { get; set; }

        [StringLength (20)]
        [Display(Name = "Número informe:")]
        public string NumeroInforme { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Objeto de veeduría: ")]
        public string ObjetoVeeduria { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Introducción:")]
        public string Introduccion { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Metodología: ")]
        public string Metodologia { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Resultados:")]
        public string Resultados { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Recomendaciones:")]
        public string Recomendaciones { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Quien recibe informe:")]
        public string QuienRecibeInforme { get; set; }

        [StringLength (256)]
        [Display(Name = "Correo de quien recibe informe:")]
        public string CorreoRecibeInforme { get; set; }

        [Display(Name = "Fecha entrega de informe:")]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaEntregaInforme { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; }

        public string Usuario { get; set; }
        public DateTime? FechaCreado { get; set; }
        public DateTime? FechaModifico { get; set; }
      
    }
}
