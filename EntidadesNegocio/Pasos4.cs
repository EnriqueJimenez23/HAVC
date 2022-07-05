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
    public class Pasos4 : Entity
    {
        public int Pasos4Id { get; set; }
        public int PasosId { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Paso cronograma:")]
        public string PasoCronograma { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "¿Qué hay que hacer?:")]
        public string QueHacer { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "¿Quiénes son los responsables de la tarea ?: ")]
        public string Responsables { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "¿Con qué recursos?:")]
        public string Recursos { get; set; }

        [Display(Name = "¿Cuándo hacerlo?:")]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Fecha { get; set; }

        [Display(Name = "Avance en porcentaje:")]
        public int Avance { get; set; }
        [Display(Name = "Estado")]
        public string Estado { get; set; }
        [NotMapped]
        public string Otra { get; set; }
        [NotMapped]
        public int NumPaso { get; set; }
        [NotMapped]
        public string NombreObjeto { get; set; }
        public string Usuario { get; set; }
        public DateTime? FechaCreado { get; set; }
        public DateTime? FechaModifico { get; set; }

      
    }
}
