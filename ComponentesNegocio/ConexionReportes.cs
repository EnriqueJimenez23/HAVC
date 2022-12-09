using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
//using Microsoft.SqlServer.Management.Smo;
//using Microsoft.SqlServer.Management.Common;
using System.IO;
/// <summary>
/// Descripción breve de Conexion
/// </summary>
/// 
namespace CapaDominio.EntidadesNegocio
{
    public class ConexionReportes
    {

        public static DataTable informev( int pasoid)
        {
            StringBuilder query = new StringBuilder();
            query.Append("select * from pasos where pasosid= ");
            query.Append(pasoid);
            DataTable dt = GetDT(query.ToString());
            return dt;
        }
        public static DataTable informepaso4(int pasoid)
        {
            StringBuilder query = new StringBuilder();
            query.Append("select * from pasos4 where pasosid= ");
            query.Append(pasoid);
            DataTable dt = GetDT(query.ToString());
            return dt;
        }
        public static DataTable infpaso(int pasoid,int paso)
        {
            StringBuilder query = new StringBuilder();
            query.Append("select * from pasos");
            query.Append(paso);
            query.Append(" where pasosid= ");
            query.Append(pasoid);
            DataTable dt = GetDT(query.ToString());
            return dt;
        }
        public static DataTable ObtenerSubregion()
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT DISTINCT subregion FROM Municipios where subregion <>'' ORDER BY  subregion");
            DataTable dt = GetDT(query.ToString());
            return dt;
        }
        public static DataTable ObtenerDepartamentos(string filtro)
        {
            StringBuilder query = new StringBuilder();
            query.Append($" SELECT DISTINCT CodDepartamento,Departamento FROM Municipios where Subregion='{filtro}' ORDER BY  Departamento ");
            DataTable dt = GetDT(query.ToString());
            return dt;
        }
        public static DataTable ObtenerDepartamentosAll()
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT DISTINCT CodDepartamento,Departamento FROM Municipios ORDER BY  Departamento");
            DataTable dt = GetDT(query.ToString());
            return dt;
        }
        public static DataTable ObtenerMunicipios(string filtro, string subregion)
        {
            StringBuilder query = new StringBuilder();
            query.Append($"SELECT DISTINCT CodMunicipio,Municipio FROM Municipios where CodDepartamento='{filtro}' and Subregion='{subregion}' ORDER BY  Municipio ");
            DataTable dt = GetDT(query.ToString());
            return dt;
        }
        public static DataTable ObtenerMunicipiosD(string filtro)
        {
            StringBuilder query = new StringBuilder();
            query.Append($"SELECT DISTINCT CodMunicipio,Municipio FROM Municipios where CodDepartamento='{filtro}' ORDER BY  Municipio ");
            DataTable dt = GetDT(query.ToString());
            return dt;
        }
        public static DataTable ObtenerMunicipiosAx()
        {
            StringBuilder query = new StringBuilder();
            query.Append($"SELECT DISTINCT CodMunicipio,Municipio FROM Municipios ORDER BY  Municipio ");
            DataTable dt = GetDT(query.ToString());
            return dt;
        }
        public static DataTable ExPasoAll(int id)
        {
            StringBuilder query = new StringBuilder();
            query.Append($"SELECT * FROM  Pasos  where PasosId={id}");
            DataTable dt = GetDT(query.ToString());
            return dt;
        }
        public static DataTable ExPaso1(int id)
        {
            StringBuilder query = new StringBuilder();
            query.Append($"SELECT Pr.NombreProyecto as 'Nombre proyecto', P.CodigoProyecto as 'Código proyecto', P.Problema, P.Territorio, P.Poblacion, P.ObjetoVigilar as '¿Qué es lo que se va a vigilar?',P.NombreObjeto as '¿Cuál es el objeto de vigilancia?', P.EntidadResponsable as '¿Cuál es la Entidad responsable del objeto de vigilancia?', P.ObjetoVeeduria as 'Describa Objeto de la veeduría' FROM   Pasos AS P INNER JOIN Proyecto AS Pr ON P.ProyectoId = Pr.ProyectoId  where P.PasosId={id}");
            DataTable dt = GetDT(query.ToString());
            return dt;
        }

        public static DataTable ExPaso2(int id)
        {
            StringBuilder query = new StringBuilder();
            query.Append($" SELECT Pr.NombreProyecto AS 'Nombre proyecto', P.CodigoProyecto AS 'Código proyecto', P.Invitacion as '¿A quiénes haría la invitación?', P.ComoMotivar as '¿Cómo motivaría la participación?',P.CanalConvocatoria as '¿Qué canales va a utilizar para la convocatoria?',P.FechasConvocatoria as '¿En qué fechas se va a realizar la convocatoria ?', p.MensajeConvocatoria as '¿Qué mensaje va a utilizar para la convocatoria?'\n FROM   Pasos AS P INNER JOIN  Proyecto AS Pr ON P.ProyectoId = Pr.ProyectoId  where P.PasosId={id}");
            DataTable dt = GetDT(query.ToString());
            return dt;
        }
        public static DataTable ExPaso3a(int id)
        {
            StringBuilder query = new StringBuilder();
            query.Append($"SELECT Pr.NombreProyecto AS 'Nombre proyecto', P.CodigoProyecto AS 'Código proyecto',P.Acta as 'Acta No.', P.Ciudad, P.ObjetoConformacion as 'Objeto de conformación', P.ObjetoVigilancia as 'Objeto de vigilancia', P.NivelTerritorial as 'Nivel territorial', P.DepartamentoActa as 'Domicilio de la veeduría Departamento', P.MunicipioActa as 'Domicilio de la veeduría Municipio', P.DuracionActa as'Duración', P.Presidir as 'Para presidir o coordinar La Asamblea fue elegido(a) el(la) señor(a)', P.Secretario as 'Como Secretario(a) fue elegido(a) el(la) señor(a)', P.Coordinador as 'Como Coordinador de la veeduría fue elegido(a) el señor(a)' P.LugarFuncionamiento as 'El lugar de funcionamiento de la veeduría es' FROM   Pasos AS P INNER JOIN Proyecto AS Pr ON P.ProyectoId = Pr.ProyectoId  where P.PasosId={id}");
            DataTable dt = GetDT(query.ToString());
            return dt;
        }
        public static DataTable ExPaso3(int id)
        {
            StringBuilder query = new StringBuilder();
            query.Append($"SELECT Pr.NombreProyecto, P.CodigoProyecto,P.Acta, P.Ciudad, P.ObjetoConformacion, P.ObjetoVigilancia, P.NivelTerritorial  , P.DepartamentoActa, P.MunicipioActa, P.DuracionActa, P.Presidir, P.Secretario, P.Coordinador, P.LugarFuncionamiento FROM   Pasos AS P INNER JOIN Proyecto AS Pr ON P.ProyectoId = Pr.ProyectoId  where P.PasosId={id}");
            DataTable dt = GetDT(query.ToString());
            return dt;
        }
        public static DataTable ExPaso3A(int id)
        {
            StringBuilder query = new StringBuilder();
            query.Append($"SELECT Pr.NombreProyecto, P.CodigoProyecto,  P3.Nombres, P3.Identificacion, P3.LugarResidencia, P3.Direccion, P3.Telefono FROM   Pasos AS P INNER JOIN Proyecto AS Pr ON P.ProyectoId = Pr.ProyectoId INNER JOIN Pasos3 AS P3 ON P.PasosId = P3.PasosId  where P.PasosId={id}");
            DataTable dt = GetDT(query.ToString());
            return dt;
        }
        public static DataTable ExPaso4(int id)
        {
            StringBuilder query = new StringBuilder();
            query.Append($"SELECT Pr.NombreProyecto AS 'Nombre proyecto', P.CodigoProyecto AS 'Código proyecto', P4.PasoCronograma as 'Paso cronograma', P4.QueHacer as 'Qué hay que hacer',P4.Responsables as 'Quiénes son los responsables de la tarea', P4.Recursos as 'Con qué recursos', P4.Fecha as 'Cuándo hacerlo', P4.Avance as 'Avance en porcentaje' FROM   Pasos AS P INNER JOIN Proyecto AS Pr ON P.ProyectoId = Pr.ProyectoId INNER JOIN             Pasos4 AS P4 ON P.PasosId = P4.PasosId where P.PasosId={id}");
            DataTable dt = GetDT(query.ToString());
            return dt;
        }
        public static DataTable ExPaso5(int id)
        {
            StringBuilder query = new StringBuilder();
            query.Append($"SELECT Pr.NombreProyecto AS 'Nombre proyecto', P.CodigoProyecto AS 'Código proyecto', P.Impactos, P.Resultados, P.Productos, P.Actividades, P.Insumos FROM   Pasos AS P INNER JOIN Proyecto AS Pr ON P.ProyectoId = Pr.ProyectoId  where P.PasosId={id}");
            DataTable dt = GetDT(query.ToString());
            return dt;
        }
        public static DataTable ExPaso5A(int id)
        {
            StringBuilder query = new StringBuilder();
            query.Append($"SELECT Pr.NombreProyecto AS 'Nombre proyecto', P.CodigoProyecto AS 'Código proyecto',  P5.ObjetivoProyecto AS 'Objetivo del proyecto', P5.EvaluacionCualitativa AS 'Evaluación cualitativa', P5.Cumple, P5.FechaEvaluacion FROM   Pasos AS P INNER JOIN Proyecto AS Pr ON P.ProyectoId = Pr.ProyectoId INNER JOIN Pasos5 AS P5 ON P.PasosId = P5.PasosId  where P.PasosId={id}");
            DataTable dt = GetDT(query.ToString());
            return dt;
        }
        public static DataTable ExPaso6(int id)
        {
            StringBuilder query = new StringBuilder();
            query.Append($"SELECT Pr.NombreProyecto AS 'Nombre proyecto', P.CodigoProyecto AS 'Código proyecto',P6.GrupoInformacion as 'Grupo de información', P6.InformacionRequerida as 'Identificación de la información requerida', P6.FuenteInformacion as 'Fuente de donde tomará la información', P6.MecanismoAccesoInformacion as 'Mecanismo a utilizar para acceder a la información', P6.ResultadoConsulta as 'Resultado de la consulta de información' FROM   Pasos AS P INNER JOIN Proyecto AS Pr ON P.ProyectoId = Pr.ProyectoId INNER JOIN Pasos6 AS P6 ON P.PasosId = P6.PasosId where P.PasosId={id}");
            DataTable dt = GetDT(query.ToString());
            return dt;
        }
        public static DataTable ExPaso7(int id)
        {
            StringBuilder query = new StringBuilder();
            query.Append($"SELECT Pr.NombreProyecto, P.CodigoProyecto, Pr.Departamento, Pr.Municipio, Pr.Pilar, Pr.EstadoProyecto,Pr.EntidadResponsable ,P.ObjetoVeeduria, P.Introduccion, P.Metodologia, P.ResultadosInf, P.Recomendaciones, P.AquienEntregaraInforme, P.CadaCuanto FROM   Pasos AS P INNER JOIN Proyecto AS Pr ON P.ProyectoId = Pr.ProyectoId where P.PasosId={id}");
            DataTable dt = GetDT(query.ToString());
            return dt;
        }
        public static DataTable ExPaso8(int id)
        {
            StringBuilder query = new StringBuilder();
            query.Append($"SELECT Pr.NombreProyecto AS 'Nombre proyecto', P.CodigoProyecto AS 'Código proyecto', Pasos8.NumeroInforme as 'Número informe', Pasos8.Actividad, Pasos8.PlanSeguimiento as 'Plan de seguimiento', Pasos8.AccionesDespuesSeguimiento as 'Acciones después de seguimiento', Pasos8.FechaProximoSeguimiento as 'Fecha próximo seguimiento' FROM   Pasos AS P INNER JOIN Proyecto AS Pr ON P.ProyectoId = Pr.ProyectoId INNER JOIN              Pasos8 ON P.PasosId = Pasos8.PasosId where P.PasosId={id}");
            DataTable dt = GetDT(query.ToString());
            return dt;
        }
        public static DataTable ExPaso9(int id)
        {
            StringBuilder query = new StringBuilder();
            query.Append($"SELECT Pr.NombreProyecto AS 'Nombre proyecto', P.CodigoProyecto AS 'Código proyecto', P9.MedioDivulgacion as 'Qué medio va a utilizar para comunicar a la comunidad',P9.InformacionPresentar as 'Qué información va a presentar',P9.Herramientas as 'Qué herramienta va a utilizar para dar la información', P9.FechaDivulgacion as 'Fecha de divulgación a la comunidad', P9.AquienInvitara as 'A quién invitará' FROM   Pasos AS P INNER JOIN Proyecto AS Pr ON P.ProyectoId = Pr.ProyectoId INNER JOIN Pasos9 as P9 ON P.PasosId = P9.PasosId where P.PasosId={id}");
            DataTable dt = GetDT(query.ToString());
            return dt;
        }
        public static string GetConnString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["CsWebContext"].ConnectionString;
        }

        public static DataTable GetDT(string Query, List<SqlParameter> ListSqlParams)
        {
            SqlConnection conn = new SqlConnection(GetConnString());
            SqlCommand comm = new SqlCommand(Query, conn);
            comm.CommandTimeout = 720;
            comm.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            SqlParameter sqlParameter;
            int iListCount = ListSqlParams.Count;

            if (iListCount > 0)
            {
                for (int i = 0; i < iListCount; i++)
                {
                    sqlParameter = new SqlParameter();
                    sqlParameter = ListSqlParams[i];
                    comm.Parameters.Add(sqlParameter);
                }
            }
            SqlDataAdapter da = new SqlDataAdapter(comm);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                da.Fill(dt);
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                return dt;
            }
            catch
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                //dt.Columns.Add("Error");
                //dt.Rows.Add(e.Message);
                return dt;
            }

        }
        public static DataTable GetDTQ(string Query, List<SqlParameter> ListSqlParams)
        {
            SqlConnection conn = new SqlConnection(GetConnString());
            SqlCommand comm = new SqlCommand(Query, conn);
            comm.CommandTimeout = 720;
         //   comm.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            SqlParameter sqlParameter;
            int iListCount = ListSqlParams.Count;

            if (iListCount > 0)
            {
                for (int i = 0; i < iListCount; i++)
                {
                    sqlParameter = new SqlParameter();
                    sqlParameter = ListSqlParams[i];
                    comm.Parameters.Add(sqlParameter);
                }
            }
            SqlDataAdapter da = new SqlDataAdapter(comm);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                da.Fill(dt);
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                return dt;
            }
            catch
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                //dt.Columns.Add("Error");
                //dt.Rows.Add(e.Message);
                return dt;
            }

        }


        public static DataTable GetDT(string Query)
        {

            SqlConnection conn = new SqlConnection(GetConnString());
            SqlCommand comm = new SqlCommand(Query, conn);
            comm.CommandTimeout = 720;
            SqlDataAdapter da = new SqlDataAdapter(comm);

            DataTable dt = new DataTable();
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                da.Fill(dt);
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                return dt;
            }
            catch 
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                //dt.Columns.Add("Error");
                //dt.Rows.Add(e.Message);
                return dt;
            }

        }

        public static DataTable GetDT_Query(string query, List<SqlParameter> parametros)
        {
            SqlConnection conn = new SqlConnection(GetConnString());
            DataTable dt = new DataTable();
                SqlCommand comm = new SqlCommand(query, conn);
                if ((parametros != null))
                {
                    foreach (SqlParameter parametro in parametros)
                    {
                        comm.Parameters.Add(parametro);
                    }
                }
                SqlDataAdapter da = new SqlDataAdapter(comm);
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    da.Fill(dt);
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    return dt;
                }
                catch
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    //dt.Columns.Add("Error");
                    //dt.Rows.Add(e.Message);
                    return dt;
                };
            }
        public static DataSet SelectQuerySentence(string query)
        {
            SqlConnection conn = new SqlConnection(GetConnString());
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
            DataSet datos = new DataSet();
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                adapter.Fill(datos);
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                return datos;
            }
            catch
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                return datos;
            };

        }
        public static bool Execute(string Query)
        {
            SqlConnection conn = new SqlConnection(GetConnString());
            SqlCommand comm = new SqlCommand(Query, conn);
            comm.CommandTimeout = 720;

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                comm.ExecuteNonQuery();
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                return true;
            }
            catch
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                return false;
            }

        }

        public static bool Execute(string Query, List<SqlParameter> ListSqlParams)
        {
            SqlConnection conn = new SqlConnection(GetConnString());
            SqlCommand comm = new SqlCommand(Query, conn);
            comm.CommandTimeout = 720;
            comm.CommandType = CommandType.StoredProcedure;
            SqlParameter sqlParameter;
            int iListCount = ListSqlParams.Count;

            if (iListCount > 0)
            {
                for (int i = 0; i < iListCount; i++)
                {
                    sqlParameter = new SqlParameter();
                    sqlParameter = ListSqlParams[i];
                    comm.Parameters.Add(sqlParameter);
                }
            }
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                comm.ExecuteNonQuery();
                if ((conn.State == System.Data.ConnectionState.Open))
                {
                    conn.Close();
                }
                return true;
            }
            catch
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                return false;
            }
        }
        public static bool ExecuteQ(string Query, List<SqlParameter> ListSqlParams)
        {
            SqlConnection conn = new SqlConnection(GetConnString());
            SqlCommand comm = new SqlCommand(Query, conn);
            comm.CommandTimeout = 720;
          //  comm.CommandType = CommandType.StoredProcedure;
            SqlParameter sqlParameter;
            int iListCount = ListSqlParams.Count;

            if (iListCount > 0)
            {
                for (int i = 0; i < iListCount; i++)
                {
                    sqlParameter = new SqlParameter();
                    sqlParameter = ListSqlParams[i];
                    comm.Parameters.Add(sqlParameter);
                }
            }
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                comm.ExecuteNonQuery();
                if ((conn.State == System.Data.ConnectionState.Open))
                {
                    conn.Close();
                }
                return true;
            }
            catch
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                return false;
            }
        }
        public static string Execute(string Query, List<SqlParameter> ListSqlParams, SqlParameter outputParameter)
        {
            SqlConnection conn = new SqlConnection(GetConnString());
            SqlCommand comm = new SqlCommand(Query, conn);
            comm.CommandTimeout = 720;
            comm.CommandType = CommandType.StoredProcedure;
            SqlParameter sqlParameter;
            int iListCount = ListSqlParams.Count;

            string msg = outputParameter.ParameterName;
            if (iListCount > 0)
            {
                for (int i = 0; i < iListCount; i++)
                {
                    sqlParameter = new SqlParameter();
                    sqlParameter = ListSqlParams[i];
                    comm.Parameters.Add(sqlParameter);

                }
                sqlParameter = new SqlParameter();
                sqlParameter = outputParameter;
                comm.Parameters.Add(sqlParameter);
            }
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                comm.ExecuteNonQuery();
                if ((conn.State == System.Data.ConnectionState.Open))
                {
                    conn.Close();
                }
                return comm.Parameters[msg].Value.ToString();
            }
            catch 
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                return "-1";
            }
        }

        public static void ActualizarRegBool(string tabla, string campo, int valor, int id)
        {
            StringBuilder query = new StringBuilder();

            query.Append("Update ");
            query.Append(tabla);
            query.Append(" set ");
            query.Append(campo);
            query.Append(" = ");
            query.Append(valor);
            query.Append(" where Id");
            query.Append(tabla);
            query.Append(" =  ");
            query.Append(id);

            EjecutarDml(query.ToString());
        }
      

     


      
        public static void ActualizarCodUnicoFB(string bas1c, string bas6b, string bas7b, string bas9b, string bas10a, string bas11, string resguardo, string cabildo, string pueblo, string comunidad, int id)
        {
            StringBuilder query = new StringBuilder();
            query.Append("Update FichaBasica set Bas1c=@bas1c,bas6b=@bas6b,bas7b=@bas7b,bas9b=@bas9b,bas10a=@bas10a, bas11=@bas11,");
            query.Append("Resguardo=@resguardo,Cabildo=@cabildo,Pueblo=@pueblo ,Comunidad=@comunidad where idfichabasica= @id");  
            List<SqlParameter> lista = new List<SqlParameter>();
            lista.Add(new SqlParameter("@bas1c", bas1c));
            lista.Add(new SqlParameter("@bas6b", bas6b));
            lista.Add(new SqlParameter("@bas7b", bas7b));
            lista.Add(new SqlParameter("@bas9b", bas9b));
            lista.Add(new SqlParameter("@bas10a", bas10a));
            lista.Add(new SqlParameter("@bas11", bas11));
            lista.Add(new SqlParameter("@resguardo", resguardo));
            lista.Add(new SqlParameter("@cabildo", cabildo));
            lista.Add(new SqlParameter("@pueblo", pueblo));
            lista.Add(new SqlParameter("@comunidad", comunidad));
            lista.Add(new SqlParameter("@id", id));


            ExecuteQ(query.ToString(),lista);
        }



      
        
        public static void ActualizarReg600(string tabla, string campo, string valor, int id)
        {
            StringBuilder query = new StringBuilder();
            if (campo == "Agricultura")
            {if (valor == "1")
                {
                    query.Append("Update Hogar set 602AgriculturaVenta=null,602AgriculturaIntercambio=null,602AgriculturaOtro=null,602AgriculturaCual=null,603Agricultura=null,603AgriculturaCual=null ");
                    query.Append(" where IdHogar= id");
                }
            else
                {
                    query.Append("Update Hogar set 602AgriculturaVenta='6',602AgriculturaIntercambio='6',602AgriculturaOtro='6',602AgriculturaCual='NA',603Agricultura='6',603AgriculturaCual='NA' ");
                    query.Append(" where IdHogar= id");
                }
            }


            EjecutarDml(query.ToString());
        }
        //public static void EliminarReg(string tabla, int id)
        //{
        //    StringBuilder query = new StringBuilder();

        //    query.Append("Delete ");
        //    query.Append(tabla);
        //    query.Append(" where Id");
        //    query.Append(tabla);
        //    query.Append(" =  ");
        //    query.Append(id);

        //    EjecutarDml(query.ToString());
        //}
        public static void EliminarReg(string tabla, string id)
        {
            StringBuilder query = new StringBuilder();

            query.Append("Delete ");
            query.Append(tabla);
            query.Append(" where Cod");
            query.Append(tabla);
            query.Append(" =  '");
            query.Append(id);
            query.Append("'");

            EjecutarDml(query.ToString());
        }
        public static void EjecutarDml(string comando)
        {
            SqlConnection connection = new SqlConnection(GetConnString());
            SqlCommand command = new SqlCommand(comando, connection);
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch 
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
              
            }
            
        }

        public static void BulkInsert(DataTable table)
        {
            using (var bulkInsert = new SqlBulkCopy(GetConnString()))
            {
                bulkInsert.DestinationTableName = table.TableName;
                bulkInsert.WriteToServer(table);
            }
        }
        public static object GetDBValue(object o)
        {
            return o ?? (object)DBNull.Value;
        }

        public static DataTable ConsolidarBase(HttpPostedFileBase reg)
        {

            string tabla = reg.FileName;
            int fin = tabla.LastIndexOf("_");
            tabla = tabla.Substring(0, fin).Trim();
            DataTable dt = new DataTable();
            dt.TableName = tabla;
            using (var sreader = new StreamReader(reg.InputStream))
            {
                char[] sep = {'|'};

                string[] fila = sreader.ReadLine().Split(sep);
                DataRow row;
                foreach (string dc in fila)
                {
                    dt.Columns.Add(new DataColumn(dc));
                }

                while (!sreader.EndOfStream)
                {
                    fila = sreader.ReadLine().Split('|');
                    if (fila.Length == dt.Columns.Count)
                    {
                        row = dt.NewRow();
                        row.ItemArray = fila;
                        dt.Rows.Add(row);
                    }
                }
            }
            return dt;
        }

    }

}
 