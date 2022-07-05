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
    public class DatosGeo : Entity
    {
        public int DatosGeoId { get; set; }

        public string CodMunicipio { get; set; }
        [Display(Name = "Municipio")]
        public string Municipio { get; set; }
        public string CodDepartamento { get; set; }
        [Display(Name = "Departamento")]
        public string Departamento { get; set; }
        [Display(Name = "Subregión")]
        public string Subregion { get; set; }
        [Display(Name = "Población total Municipio")]
        public string PobTotal { get; set; }
        [Display(Name = "Extensión Km2")]
        public string Extension { get; set; }
        [Display(Name = "Población rural")]
        public string PobRural { get; set; }
        [Display(Name = "Población urbana")]
        public string PobUrbana { get; set; }
        [Display(Name = "Total hombres")]
        public string PobHombres { get; set; }
        [Display(Name = "Total mujeres")]
        public string PobMujeres { get; set; }
        [Display(Name = "Población indigena")]
        public string PobIndigena { get; set; }
        [Display(Name = "Población afrodescendiente")]
        public string PobAfro { get; set; }
        [Display(Name = "Población palenque")]
        public string PobPalenque { get; set; }
        [Display(Name = "Población raizal")]
        public string PobRaizal { get; set; }
        [Display(Name = "Población Rom")]
        public string PobRom { get; set; }
        [Display(Name = "Veedurías ciudadanas Acuerdo de Paz y/o temas afines")]
        public string VeeduriasPaz	{ get; set; }
        [Display(Name = "Total Veedurías Ciudadanas")]
        public string TotalVeedurías { get; set; }
        [Display(Name = "Juntas de acción")]
        public string TotalJuntas { get; set; }
        [Display(Name = " ")]
        public string Dato1 { get; set; }
    }
}
