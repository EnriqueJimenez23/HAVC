using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using CapaDatos.Repositorio.EF6;

namespace CapaDominio.EntidadesNegocio
{
  
    public class Usuario : Entity
    {
        public int UsuarioId { get; set; }

        [Required]
        [StringLength(512)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Usuario:")]
        public string NombreUsuario { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña:")]
        public string Contrasena { get; set; }

     //   [Required]
        [Display(Name = "Perfil:")]
        public virtual Perfil Perfil { get; set; }

     //   [Required]
        [Display(Name = "Usuario activo:")]
        public bool Activo { get; set; }

        [StringLength(128)]
        [Display(Name = "Nombre completo:")]
        public string NombreCompleto { get; set; }


        [StringLength(512)]
        [Display(Name = "Tipo de identificación:")]
        public string TipoIdentificacion { get; set; }

        [StringLength(512)]
        [Display(Name = "Número de identificación:")]
        public string Identificacion { get; set; }

        [StringLength(512)]
        [Display(Name = "Teléfono:")]
        public string Telefono { get; set; }

        [StringLength(512)]
        [Display(Name = "Subregion:")]
        public string Subregion { get; set; }
        [StringLength(512)]
        [Display(Name = "Departamento:")]
        public string Departamento { get; set; }

        [StringLength(2)]
        public string CodDepartamento{ get; set; }

        [StringLength(512)]
        [Display(Name = "Municipio:")]
        public string Municipio { get; set; }

        [StringLength(5)]
        public string CodMunicipio { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha registro:")]
        public DateTime FechaRegistro { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha último acceso:")]
        public DateTime? FechaUltimoAcceso { get; set; }
        

        public override string ToString()
        {
            return $"Usuario: {NombreUsuario} - Nombre: {NombreCompleto} - Perfil: {Perfil.NombrePerfil} - Activo: {Activo}";
           
        }
    }
}
