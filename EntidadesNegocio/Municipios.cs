using System;
using System.ComponentModel.DataAnnotations;
using CapaDatos.Repositorio.EF6;
using System.Reflection;
using CapaServicios.Servicios;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CapaDominio.EntidadesNegocio
{
    public class Municipios : Entity
    {
        [Key]
        [StringLength(5)]
        public string CodMunicipio { get; set; }
        [StringLength(256)]
        public string Municipio { get; set; }
        [StringLength(2)]
        public string CodDepartamento { get; set; }
        [StringLength(256)]
        public string Departamento { get; set; }
        [StringLength(256)]
        public string Subregion { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string PobTotal { get; set; }
        public string Extension { get; set; }
        public string PobRural { get; set; }
        public string PobUrbana { get; set; }
        public string PobHombres { get; set; }
        public string PobMujeres { get; set; }
        public string PobIndigena { get; set; }
        public string PobAfro { get; set; }
        public string PobPalenque { get; set; }
        public string PobRaizal { get; set; }
        public string PobRom { get; set; }

}
}
