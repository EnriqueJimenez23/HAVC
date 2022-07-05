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
    public class Pasos6 : Entity
    {
        public int Pasos6Id { get; set; }
        public int PasosId { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Grupo de información:")]
        public string GrupoInformacion { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Identificación de la información requerida: ")]
        public string InformacionRequerida { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Fuente de donde tomará la información: ")]
        public string FuenteInformacion { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Mecanismo a utilizar para acceder a la información: ")]
        public string MecanismoAccesoInformacion { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Resultado de la consulta de información: ")]
        public string ResultadoConsulta { get; set; }


        public string Usuario { get; set; }
        public DateTime? FechaCreado { get; set; }
        public DateTime? FechaModifico { get; set; }
        [NotMapped]
        public int NumPaso { get; set; }
        [NotMapped]
        public string NombreObjeto { get; set; }
    }
}
