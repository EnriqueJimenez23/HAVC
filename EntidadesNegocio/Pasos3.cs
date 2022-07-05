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
    public class Pasos3 : Entity
    {
        public int Pasos3Id { get; set; }
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

        public string Usuario { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? FechaCreado { get; set; }
        [NotMapped]
        public int NumPaso { get; set; }
        [NotMapped]
        public string NombreObjeto { get; set; }
    }
}
