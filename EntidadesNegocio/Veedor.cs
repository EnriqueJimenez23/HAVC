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
    public class Veedor : Entity
    {
        public int VeedorId { get; set; }
        [StringLength(512)]
        [Display(Name = "Nombres:")]
        public string Nombres { get; set; }
        [StringLength(512)]
        [Display(Name = "Apellidos:")]
        public string Apellidos { get; set; }

        [StringLength(64)]
        [Display(Name = "Tipo de identificación:")]
        public string TipoIdentificacion { get; set; }

        [StringLength(512)]
        [Display(Name = "Número de identificación:")]
        public string Identificacion { get; set; }

        [StringLength(512)]
        [Display(Name = "Subregion PDET:")]
        public string Subregion { get; set; }

        [StringLength(512)]
        [Display(Name = "Departamento:")]
        public string Departamento { get; set; }

       
        [StringLength(512)]
        [Display(Name = "Departamento:")]
        public string CodDepartamento { get; set; }

        
        [StringLength(512)]
        [Display(Name = "Municipio:")]
        public string Municipio { get; set; }

     
        [StringLength(512)]
        [Display(Name = "Municipio:")]
        public string CodMunicipio { get; set; }

        [StringLength(512)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo electrónico:")]
        public string Correo { get; set; }

        [Display(Name = "Teléfono de contacto:")]
        public string Telefono { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Observaciones:")]
        public string Observaciones { get; set; }

       
        [DataType(DataType.DateTime)]
        public DateTime FechaCreado { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? FechaActualizacion { get; set; }
        
      
        [StringLength(128)]
        public string UsuarioCreacion { get; set; }
        [StringLength(128)]
        public string UsuarioActualizacion { get; set; }

        //[NotMapped]
        //[DataType(DataType.MultilineText)]
        //public string RegOperaciones { get; set; }
        [NotMapped]
        public bool Autorizo { get; set; }

  
    }
}
