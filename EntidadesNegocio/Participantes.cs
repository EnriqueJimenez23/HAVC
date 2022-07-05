using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Linq;
using System.Text;
using CapaDatos.Repositorio.EF6;
using CapaServicios.Servicios;

namespace CapaDominio.EntidadesNegocio
{
    public class Participante : Entity
    {
        public int Paso3Id { get; set; }
        public int PasosId { get; set; }
        [StringLength(1024)]
        [Display(Name = "Nombre:")]
        public string Nombres { get; set; }
       
        [StringLength(512)]
        [Display(Name = "Documento de identidad:")]
        public string Identificacion { get; set; }

        [StringLength(512)]
        [Display(Name = "Lugar de residencia:")]
        public string LugarResidencia { get; set; }

        [StringLength(512)]
        [Display(Name = "Dirección:")]
        public string Direccion { get; set; }

        [StringLength(512)]
        [Display(Name = "Teléfono:")]
        public string Telefono { get; set; }
     
        [DataType(DataType.MultilineText)]
        [Display(Name = "Observaciones:")]
        public string Observaciones { get; set; }

       
        [DataType(DataType.DateTime)]
        public DateTime FechaCreado { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? FechaActualizacion { get; set; }
        
    }
}
