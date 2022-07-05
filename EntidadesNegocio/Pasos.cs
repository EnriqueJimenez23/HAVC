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
    public class Pasos : Entity
    {
        public int PasosId { get; set; }
        public int VeedorId { get; set; }
        public int ProyectoId { get; set; }
        [StringLength(128)]
        [Display(Name = "Subregión PDET:")]
        public string Subregion { get; set; }
        [StringLength(128)]
        [Display(Name = "Departamento:")]
        public string Departamento { get; set; }
        [StringLength(2)]
        public string CodDepartamento { get; set; }
        [StringLength(128)]
        [Display(Name = "Municipio:")]
        public string Municipio { get; set; }
        [StringLength(5)]
        public string CodMunicipio { get; set; }
        [StringLength(512)]
        [Display(Name = "Código del proyecto:")]
        public string CodigoProyecto { get; set; }
        [Display(Name = "Nombre veedor:")]
        public string NombreVeedor { get; set; }
        //PASO 1
        [DataType(DataType.MultilineText)]
        [Display(Name = "Problema:")]
        public string Problema { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Territorio:")]
        public string Territorio { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Población:")]
        public string Poblacion { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "¿Qué es lo que se va a vigilar?:")]
        public string ObjetoVigilar { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "¿Cuál es el objeto de vigilancia?:")]
        public string NombreObjeto { get; set; }

        [Display(Name = "¿Cuál es la Entidad responsable del objeto de vigilancia?")]
        public string EntidadResponsable { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Describa Objeto de la veeduría:")]
        public string ObjetoVeeduria { get; set; }

        //PASO 2
        [DataType(DataType.MultilineText)]
        [Display(Name = "¿A quiénes haría la invitación?:")]
        public string Invitacion { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "¿Cómo motivaría la participación?:")]
        public string ComoMotivar { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "¿Qué canales va a utilizar para la convocatoria?:")]
        public string CanalConvocatoria { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "¿En qué fechas se va a realizar la convocatoria ?: ")]
        public string FechasConvocatoria { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "¿Qué mensaje va a utilizar para la convocatoria?:")]
        public string MensajeConvocatoria { get; set; }
        //PASO 3

        [Display(Name = "Acta No.:")]
        public string Acta { get; set; }

        [Display(Name = "En la ciudad de:")]
        public string Ciudad { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Objeto de conformación:")]
        public string ObjetoConformacion { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Objeto de vigilancia: ")]
        public string ObjetoVigilancia { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Nivel territorial:")]
        public string NivelTerritorial { get; set; }

        [Display(Name = "Domicilio de la veeduría Departamento:")]
        public string CodDepartamentoActa { get; set; }
        public string DepartamentoActa { get; set; }

        [Display(Name = "Domicilio de la veeduría Municipio:")]
        public string CodMunicipioActa { get; set; }
        public string MunicipioActa { get; set; }

        [Display(Name = "Duración de la veeduría:")]
        public string DuracionActa { get; set; }

        [Display(Name = "Para presidir o coordinar La Asamblea fue elegido(a) el(la) señor(a):")]
        public string Presidir { get; set; }
        [Display(Name = "Como Secretario(a) fue elegido(a) el(la) señor(a):")]
        public string Secretario { get; set; }
        [Display(Name = "Como Coordinador de la veeduría fue elegido(a) el señor(a):")]
        public string Coordinador { get; set; }
        [Display(Name = "El lugar de funcionamiento de la veeduría es:")]
        public string LugarFuncionamiento { get; set; }

        //PASO 5
        [DataType(DataType.MultilineText)]
        [Display(Name = "Impactos:")]
        public string Impactos { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Resultados:")]
        public string Resultados { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Productos:")]
        public string Productos { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Actividades: ")]
        public string Actividades { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Insumos: ")]
        public string Insumos { get; set; }

        //PASO 7
        [DataType(DataType.MultilineText)]
        [Display(Name = "Introduccion:")]
        public string Introduccion { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Metodología:")]
        public string Metodologia { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Resultados:")]
        public string ResultadosInf { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Recomendaciones:")]
        public string Recomendaciones { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "¿ A quién entregar el informe?.: ")]
        public string AquienEntregaraInforme { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "¿ Cada cuánto se entrega informe ?: ")]
        public string CadaCuanto { get; set; }


        [NotMapped]
        Veedor veedor { get; set; }
        [NotMapped]
        public string EtapaCronograma { get; set; }

        [NotMapped]
        public int NumPaso { get; set; }
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
