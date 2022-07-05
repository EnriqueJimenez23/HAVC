using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CapaServicios.Servicios
{
    public static class Mapeo
    {
        public static DataTable CrearDataTable<T>(DataTable dataTable = null, string[] columnasExcluidas = null, bool nombrePropiedadOriginal = false) 
        {
            Type entidad = typeof(T);
            PropertyInfo[] propiedades = entidad.GetProperties();

            if (dataTable == null)
                dataTable = new DataTable();

            foreach (var item in propiedades)
            {
                if (ArchivoDeRecursos.Valor_TiposDeDatosExcluir.Contains(item.PropertyType.Name))
                    continue;

                if (columnasExcluidas != null && columnasExcluidas.Contains(item.Name))
                    continue;

                var atributoNotMapped = item.GetCustomAttributes(typeof(NotMappedAttribute));
                if (atributoNotMapped != null && atributoNotMapped.Any())
                    continue;

                var nombreColumna = item.Name;

                if (!nombrePropiedadOriginal)
                {
                    var atributoDisplay = item.GetCustomAttributes(typeof (DisplayAttribute)).ToList();
                    if (atributoDisplay.Count > 0)
                    {
                        var temp = (DisplayAttribute) atributoDisplay.FirstOrDefault();
                        if (temp != null)
                            nombreColumna = temp.Name.Replace(":", "");
                    }
                }

                if (item.PropertyType.Name == "Int32" || item.PropertyType.Name == "Decimal" || item.PropertyType.Name == "Decimal?")
                {
                    dataTable.Columns.Add(nombreColumna, item.PropertyType);
                }
                else
                {
                    dataTable.Columns.Add(nombreColumna);
                }
            }

            return dataTable;
        }

        public static string ObtenerNombreColumna<T>(Expression<Func<T, object>> expression, bool nombrePropiedadOriginal = false)
        {
            MemberExpression body = expression.Body as MemberExpression;
            if (body == null)
            {
                var op = ((UnaryExpression)expression.Body).Operand;
                body = ((MemberExpression) op);
            }

            var nombrePropiedad = body.Member.Name;

            if (!nombrePropiedadOriginal)
            {
                var atributoDisplay = body.Member.GetCustomAttributes(typeof (DisplayAttribute)).ToList();
                if (atributoDisplay.Count > 0)
                {
                    var temp = (DisplayAttribute) atributoDisplay.FirstOrDefault();
                    if (temp != null)
                        nombrePropiedad = temp.Name.Replace(":", "");
                }
            }


            return nombrePropiedad;
        }
    }
}
