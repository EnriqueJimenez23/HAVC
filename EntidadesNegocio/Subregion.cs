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
    public class Subregion : Entity
    {
        public int SubregionId { get; set; }
        [StringLength(128)]
        [Display(Name = "Subregión PDET:")]
        public string NombreSubregion { get; set; }
        [StringLength(128)]
        [Display(Name = "Departamento:")]
        public string Departamento{ get; set; }
        [StringLength(128)]
        [Display(Name = "Municipio:")]
        public string Municipio{ get; set; }
        [StringLength(2)]
        public string CodDepartamento { get; set; }
        [StringLength(5)]
        public string CodMunicipio{ get; set; }
       
       
    }
}
