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
    public class Proyecto : Entity
    {
        public int ProyectoId { get; set; }
        [StringLength(128)]
        [Display(Name = "Subregión PDET:")]
        public string Subregion { get; set; }
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
        [StringLength(128)]
        [Display(Name = "Punto Acuerdo de Paz:")]
        public string Punto { get; set; }
        [StringLength(128)]
        [Display(Name = "Pilar PDET:")]
        public string Pilar { get; set; }
        [StringLength(512)]
        [Display(Name = "Estrategia:")]
        public string Estrategia { get; set; }
        [StringLength(1024)]
        [Display(Name = "Entidad responsable:")]
        public string EntidadResponsable { get; set; }
        [StringLength(512)]
        [Display(Name = "Sector:")]
        public string Sector { get; set; }
        [StringLength(512)]
        [Display(Name = "Código del proyecto:")]
        public string CodigoProyecto{ get; set; }
        [StringLength(512)]
        [Display(Name = "Título:")]
        public string TItulo{ get; set; }
        [Display(Name = "Descripción:")]
        [DataType(DataType.MultilineText)]
        public string Descripcion{ get; set; }
        [Display(Name = "Nombre del proyecto:")]
        public string NombreProyecto{ get; set; }
        [Display(Name = "Estado:")]
        public string EstadoProyecto{ get; set; }
        [Display(Name = "Monto inversión:")]
        public string MontoInversión{ get; set; }
        [Display(Name = "Fuente recursos:")]
        public string FuenteRecursos{ get; set; }
        public virtual List<ProyectoAvance> ProyectoAvances { get; set; }
        public string Usuario { get; set; }
        public DateTime? FechaCreado { get; set; }

        public override string ToString()
        {
            PropertyInfo[] propiedades = GetType().GetProperties();
            var stringBuilder = new StringBuilder();

            foreach (var item in propiedades)
            {
                if (ArchivoDeRecursos.Valor_TiposDeDatosExcluir.Contains(item.PropertyType.Name))
                    continue;

                var atributos = item.GetCustomAttributes(typeof(NotMappedAttribute));
                if (atributos.Any())
                    continue;

                var valor = item.GetValue(this, null) ?? string.Empty;
                stringBuilder.AppendLine($"{item.Name}: {valor}");
            }

            return stringBuilder.ToString();
        }
    }
}
