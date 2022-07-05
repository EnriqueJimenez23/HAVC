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
    public class Pasos9 : Entity
    {
        public int Pasos9Id { get; set; }
        public int PasosId { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "¿Qué medio va a utilizar para comunicar a la comunidad ?: ")]
        public string MedioDivulgacion { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "¿Qué información va a presentar?: ")]
        public string InformacionPresentar{ get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "¿Qué herramienta va a utilizar para dar la información?:")]
        public string Herramientas { get; set; }

        [Display(Name = "Fecha de divulgación a la comunidad:")]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaDivulgacion { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "¿A quién invitará?:")]
        public string AquienInvitara { get; set; }
        [Display(Name = "Temas a tratar:")]
        public string Temas { get; set; }
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
