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
    public class Perfil : Entity
    {
        [Key]
        [StringLength(512)]
        [Display(Name = "Nombre del perfil:")]
        public string NombrePerfil { get; set; }

        [Required]
        [Display(Name = "Perfil activo:")]
        public bool Activo { get; set; }

        [StringLength(1024)]
        [Display(Name = "Descripción:")]
        public string Descripcion { get; set; }

        public virtual ICollection<Permiso> Permisos { get; set; }

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
            if (Permisos != null)
            {
                stringBuilder.AppendLine("\nPermisos\n\n");
                foreach (Permiso item in Permisos)
                {
                    stringBuilder.AppendLine(item.ToString());
                }
            }

            return stringBuilder.ToString();
        }
    }
}
