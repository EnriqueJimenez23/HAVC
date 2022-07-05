using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using CapaDatos.Repositorio.EF6;
using CapaServicios.Servicios;

namespace CapaDominio.EntidadesNegocio
{
    public class Permiso : Entity
    {
        public int PermisoId { get; set; }

        public int Index { get; set; }
        
        public int PermisoPadreId { get; set; }

        [StringLength(512)]
        [Display(Name = "Etiqueta:")]
        public string Etiqueta { get; set; }

        [StringLength(512)]
        [Display(Name = "Controlador:")]
        public string Controlador { get; set; }

        [StringLength(512)]
        [Display(Name = "Acción:")]
        public string Accion { get; set; }

        [Display(Name = "Mostrar en el menú:")]
        public bool EsElementoMenu { get; set; }

        [Display(Name = "Posición en el menú:")]
        public int PosicionMenu { get; set; }

        [StringLength(512)]
        [Display(Name = "Icono:")]
        public string Icono { get; set; }

        [Display(Name = "Activo:")]
        public bool Activo { get; set; }

        public virtual ICollection<Perfil> Perfiles { get; set; }

        public override string ToString()
        {
            PropertyInfo[] propiedades = GetType().GetProperties();
            var stringBuilder = new StringBuilder();

            foreach (var item in propiedades)
            {
                if (ArchivoDeRecursos.Valor_TiposDeDatosExcluir.Contains(item.PropertyType.Name))
                    continue;

                var atributos = item.GetCustomAttributes(typeof(NotMappedAttribute));
                if (atributos != null && atributos.Any())
                    continue;

                var valor = item.GetValue(this, null) ?? string.Empty;
                stringBuilder.AppendLine(string.Format("{0}: {1}", item.Name, valor));
            }

            return stringBuilder.ToString();
        }
    }
}
