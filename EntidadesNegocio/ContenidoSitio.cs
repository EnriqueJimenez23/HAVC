using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using CapaDatos.Repositorio.EF6;


namespace CapaDominio.EntidadesNegocio
{
    public class ContenidoSitio : Entity
    {
        public int ContenidoSitioId { get; set; }

        [StringLength(512)]
        [Display(Name = "Sección:")]
        public string Seccion { get; set; }
       
        [Display(Name = "Título:")]
        public string Titulo { get; set; }

        [Display(Name = "Enlace:")]
        public string Enlace { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Contenido:")]
        public string Contenido { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción:")]
        public string Descripcion { get; set; }

        [Display(Name = "Imagen:")]
        public string Imagen { get; set; }

        [StringLength(1024)]
        [Display(Name = "PalabraClave:")]
        public string PalabraClave { get; set; }

        [StringLength(2)]
        [Display(Name = "Publicar:")]
        public string Publicar { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        [Display(Name = "Archivo:")]
        public HttpPostedFileBase Archivo { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? FechaCreado { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? FechaActualizacion { get; set; }
        
        [StringLength(128)]
        public string UsuarioCreo { get; set; }
        [StringLength(128)]
        public string UsuarioModifico { get; set; }
    }
}
