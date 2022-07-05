using System;
using System.Data;
using System.IO;
using System.Web;
using OfficeOpenXml;



namespace CapaServicios.Servicios
{
    public class Excel
    {
        
        public static MemoryStream CrearReporteGeneral(string nombreReporte, string parametros, DataTable dataTable, out string nombreArchivo)
        {
            nombreArchivo = $"{DateTime.Now:yyyyMMdd}-{nombreReporte}.xlsx";
            FileInfo directorioPlantillas = new FileInfo(
                $"{HttpContext.Current.Server.MapPath("~/Content/Plantillas/")}\\Reportes.xlsx");
            FileInfo directorioTemporal = new FileInfo(
                $"{HttpContext.Current.Server.MapPath("~/Temp/")}\\{nombreArchivo}");

            using (ExcelPackage excelPackage = new ExcelPackage(directorioTemporal, directorioPlantillas))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];

                worksheet.Cells[2, 2].Value = $"{nombreReporte} - Fecha de generación: {DateTime.Now}";
                worksheet.Name = nombreReporte;
                worksheet.Cells[3, 2].Value = $"Número de registros: {dataTable.Rows.Count}";

                int fila = 6;
                int columna = 2;
                
                foreach (DataColumn dataColumn in dataTable.Columns)
                {
                    worksheet.Cells[fila, columna].Value = dataColumn.ColumnName;
                    worksheet.Column(columna).AutoFit();
                    columna++;
                }

                fila = 7;

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    columna = 2;
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        worksheet.Cells[fila, columna].Value = dataRow[i];
                        if (dataRow[i] is decimal)
                            worksheet.Cells[fila, columna].Style.Numberformat.Format = @"#,##0.00";

                        DateTime temp;
                        if (dataRow.Table.Columns[i].ColumnName.Contains("Fecha") && DateTime.TryParse(dataRow[i].ToString(), out temp))
                        {
                            worksheet.Cells[fila, columna].Style.Numberformat.Format = @"dd/MM/yyyy";
                            worksheet.Cells[fila, columna].Value = temp;
                        }
                           
                        columna++;
                    }

                    fila++;
                }

                var stream = new MemoryStream();
                excelPackage.SaveAs(stream);

                return stream;
            }
        }

        public static MemoryStream CrearRPasos(string nombreReporte,string paso, DataTable dataTable, out string nombreArchivo)
        {
            nombreArchivo = $"{DateTime.Now:yyyyMMdd}-{nombreReporte}.xlsx";
            FileInfo directorioPlantillas = new FileInfo(
                $"{HttpContext.Current.Server.MapPath("~/Content/Plantillas/")}\\RepPasos.xlsx");
            FileInfo directorioTemporal = new FileInfo(
                $"{HttpContext.Current.Server.MapPath("~/Temp/")}\\{nombreArchivo}");

            using (ExcelPackage excelPackage = new ExcelPackage(directorioTemporal, directorioPlantillas))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];

                worksheet.Cells[2, 2].Value = $"{nombreReporte} - Fecha de generación: {DateTime.Now}";
                worksheet.Name = nombreReporte;
                int fila = 4;
                int columna = 2;

                foreach (DataColumn dataColumn in dataTable.Columns)
                {
                    worksheet.Cells[fila, columna].Value = dataColumn.ColumnName;
                    worksheet.Column(columna).AutoFit();
                    columna++;
                }

                fila = 5;

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    columna = 2;
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        worksheet.Cells[fila, columna].Value = dataRow[i];
                        if (dataRow[i] is decimal)
                            worksheet.Cells[fila, columna].Style.Numberformat.Format = @"#,##0.00";

                        DateTime temp;
                        if (dataRow.Table.Columns[i].ColumnName.Contains("Fecha") && DateTime.TryParse(dataRow[i].ToString(), out temp))
                        {
                            worksheet.Cells[fila, columna].Style.Numberformat.Format = @"dd/MM/yyyy";
                            worksheet.Cells[fila, columna].Value = temp;
                        }

                        columna++;
                    }

                    fila++;
                }

                var stream = new MemoryStream();
                excelPackage.SaveAs(stream);

                return stream;
            }
        }

        public static MemoryStream CrearActa( DataTable dataPasos, DataTable dataPasos3, out string nombreArchivo)
        {
            nombreArchivo = $"{DateTime.Now:yyyyMMddHHmmss}-Acta.xlsx";
            FileInfo directorioPlantillas = new FileInfo(
                $"{HttpContext.Current.Server.MapPath("~/Content/Plantillas/")}\\Acta.xlsx");
            FileInfo directorioTemporal = new FileInfo(
                $"{HttpContext.Current.Server.MapPath("~/Temp/")}\\{nombreArchivo}");

            using (ExcelPackage excelPackage = new ExcelPackage(directorioTemporal, directorioPlantillas))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];

                worksheet.Name = "Acta";
                worksheet.Cells[4, 2].Value = dataPasos.Rows[0]["Acta"];

                worksheet.Cells[9, 1].Value = dataPasos.Rows[0]["ObjetoConformacion"];
                worksheet.Cells[11, 2].Value = dataPasos.Rows[0]["ObjetoVigilancia"];
                worksheet.Cells[13, 2].Value = dataPasos.Rows[0]["NivelTerritorial"];
                worksheet.Cells[14, 3].Value = dataPasos.Rows[0]["MunicipioActa"];
                worksheet.Cells[14, 7].Value = dataPasos.Rows[0]["DepartamentoActa"];
                worksheet.Cells[15,2].Value = dataPasos.Rows[0]["DuracionActa"];
                worksheet.Cells[17, 4].Value = dataPasos.Rows[0]["Presidir"];
                worksheet.Cells[18, 4].Value = dataPasos.Rows[0]["Secretario"];
                worksheet.Cells[19, 4].Value = dataPasos.Rows[0]["Coordinador"];
                worksheet.Cells[21, 3].Value = dataPasos.Rows[0]["LugarFuncionamiento"];

                int fila = 29;

                foreach (DataRow fuente in dataPasos3.Rows)
                {
                    worksheet.Cells[fila, 1].Value = fuente["Nombres"];
                    worksheet.Cells[fila, 2].Value = fuente["Identificacion"];
                    worksheet.Cells[fila, 3].Value = fuente["LugarResidencia"];
                    worksheet.Cells[fila, 4].Value = fuente["Direccion"];
                    worksheet.Cells[fila, 5].Value = fuente["Telefono"];
                   
                    
                    fila++;                        
                }

                var stream = new MemoryStream();
                excelPackage.SaveAs(stream);

                return stream;
            }
        }

        public static MemoryStream CrearRPaso5(DataTable dataTable, DataTable dt5, out string nombreArchivo)
        {
            nombreArchivo = $"{DateTime.Now:yyyyMMdd}-Paso5.xlsx";
            FileInfo directorioPlantillas = new FileInfo(
                $"{HttpContext.Current.Server.MapPath("~/Content/Plantillas/")}\\RepPaso5.xlsx");
            FileInfo directorioTemporal = new FileInfo(
                $"{HttpContext.Current.Server.MapPath("~/Temp/")}\\{nombreArchivo}");

            using (ExcelPackage excelPackage = new ExcelPackage(directorioTemporal, directorioPlantillas))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];

                worksheet.Cells[2, 2].Value = $"Paso 5 - Fecha de generación: {DateTime.Now}";
                worksheet.Name = "Paso5";

                int fila = 4;
                int columna = 2;

                foreach (DataColumn dataColumn in dataTable.Columns)
                {
                    worksheet.Cells[fila, columna].Value = dataColumn.ColumnName;
                    worksheet.Column(columna).AutoFit();
                    columna++;
                }

                fila = 5;

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    columna = 2;
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        worksheet.Cells[fila, columna].Value = dataRow[i];
                        if (dataRow[i] is decimal)
                            worksheet.Cells[fila, columna].Style.Numberformat.Format = @"#,##0.00";

                        DateTime temp;
                        if (dataRow.Table.Columns[i].ColumnName.Contains("Fecha") && DateTime.TryParse(dataRow[i].ToString(), out temp))
                        {
                            worksheet.Cells[fila, columna].Style.Numberformat.Format = @"dd/MM/yyyy";
                            worksheet.Cells[fila, columna].Value = temp;
                        }

                        columna++;
                    }

                    fila++;
                }


                fila = 8;
                worksheet.Cells[7, 2].Value = "Objetivo";
                worksheet.Cells[7, 3].Value = "Evaluación cualitativa";
                worksheet.Cells[7, 4].Value = "Cumple";
                
                foreach (DataRow dataRow in dt5.Rows)
                {
                    columna = 2;
                    for (int i = 2; i < 5; i++)
                    {
                        worksheet.Cells[fila, columna].Value = dataRow[i];
                        columna++;
                    }

                    fila++;
                }
                var stream = new MemoryStream();
                excelPackage.SaveAs(stream);

                return stream;
            }
        }

        public static MemoryStream CrearInforme1(string nombreReporte, DataTable dtA, DataTable dataPasos, DataTable dataPasos4, DataTable dt5, DataTable dt6, out string nombreArchivo)
        {
            nombreArchivo = $"{DateTime.Now:yyyyMMddHHmmss}-{nombreReporte}.xlsx";
            FileInfo directorioPlantillas = new FileInfo(
                $"{HttpContext.Current.Server.MapPath("~/Content/Plantillas/")}\\Informe.xlsx");
            FileInfo directorioTemporal = new FileInfo(
                $"{HttpContext.Current.Server.MapPath("~/Temp/")}\\{nombreArchivo}");

            using (ExcelPackage excelPackage = new ExcelPackage(directorioTemporal, directorioPlantillas))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];

                worksheet.Name = nombreReporte;
                worksheet.Cells[3, 10].Value = dataPasos.Rows[0]["CodigoProyecto"];
                //Codigo Proyecto
                worksheet.Cells[4, 2].Value = dataPasos.Rows[0]["NombreProyecto"];
                worksheet.Cells[5, 3].Value = dataPasos.Rows[0]["EntidadResponsable"];
                worksheet.Cells[6, 3].Value = dataPasos.Rows[0]["Departamento"];
                worksheet.Cells[7, 3].Value = dataPasos.Rows[0]["Municipio"];
                worksheet.Cells[8, 3].Value = dataPasos.Rows[0]["Pilar"];
                worksheet.Cells[9, 3].Value = dataPasos.Rows[0]["EstadoProyecto"];
                worksheet.Cells[12, 3].Value = dataPasos.Rows[0]["ObjetoVeeduria"];
                worksheet.Cells[13, 3].Value = dataPasos.Rows[0]["Introduccion"];
                worksheet.Cells[14, 3].Value = dataPasos.Rows[0]["Metodologia"];
                worksheet.Cells[15, 3].Value = dataPasos.Rows[0]["ResultadosInf"];
                worksheet.Cells[16, 3].Value = dataPasos.Rows[0]["Recomendaciones"];

                worksheet.Cells[18, 3].Value = dtA.Rows[0]["Problema"];
                worksheet.Cells[19, 3].Value = dtA.Rows[0]["Territorio"];
                worksheet.Cells[20, 3].Value = dtA.Rows[0]["Poblacion"];
                worksheet.Cells[22, 3].Value = dtA.Rows[0]["Invitacion"];
                worksheet.Cells[23, 3].Value = dtA.Rows[0]["ComoMotivar"];
                worksheet.Cells[24, 3].Value = dtA.Rows[0]["CanalConvocatoria"];
                worksheet.Cells[25, 3].Value = dtA.Rows[0]["FechasConvocatoria"];

                worksheet.Cells[26, 3].Value = dtA.Rows[0]["MensajeConvocatoria"];
                worksheet.Cells[28, 3].Value = dtA.Rows[0]["Impactos"];
                worksheet.Cells[29, 3].Value = dtA.Rows[0]["Resultados"];
                worksheet.Cells[30, 3].Value = dtA.Rows[0]["Productos"];
                worksheet.Cells[31, 3].Value = dtA.Rows[0]["Actividades"];
                worksheet.Cells[32, 3].Value = dtA.Rows[0]["Insumos"];

               
                worksheet.Cells[34, 2].Value = "PASO 4.Formulación del plan de trabajo";
                int fila = 35;
                foreach (DataRow fuente in dataPasos4.Rows)
                {
                    worksheet.Cells[fila, 2].Value = fuente["PasoCronograma"];
                    worksheet.Cells[fila, 3].Value = fuente["QueHacer"];
                    worksheet.Cells[fila, 4].Value = fuente["Responsables"];
                    worksheet.Cells[fila, 6].Value = fuente["Recursos"];
                    worksheet.Cells[fila, 6].Value = fuente["Fecha"];
                    fila++;
                }

                fila = fila+1;
                worksheet.Cells[fila, 2].Value = "PASO 5. Establecer criterios de Evaluación";
                fila = fila+1;

                worksheet.Cells[fila, 2].Value = "Objetivo";
                worksheet.Cells[fila, 3].Value = "Evaluación cualitativa";
                worksheet.Cells[fila, 4].Value = "Cumple";
                fila = fila+1;
                foreach (DataRow dataRow in dt5.Rows)
                {
                    int columna = 2;
                    for (int i = 2; i < 5; i++)
                    {
                        worksheet.Cells[fila, columna].Value = dataRow[i];
                        columna++;
                    }
                    fila++;
                }

                fila = fila+1;
                worksheet.Cells[fila, 2].Value = "PASO 6. Recoger y analizar la información obtenida";
                fila = fila+1;
                worksheet.Cells[fila, 2].Value = "Grupo de información";
                worksheet.Cells[fila, 3].Value = "Identificación de la información";
                worksheet.Cells[fila, 4].Value = "Fuente donde tomara la información";
                worksheet.Cells[fila, 5].Value = "Mecanismos para acceder a la información";
                foreach (DataRow dataRow in dt6.Rows)
                {
                    int columna = 2;
                    for (int i = 2; i < 5; i++)
                    {
                        worksheet.Cells[fila, columna].Value = dataRow[i];
                        columna++;
                    }

                    fila++;
                }
                var stream = new MemoryStream();
                excelPackage.SaveAs(stream);

                return stream;
            }
        }

        public static MemoryStream CrearInforme2(string nombreReporte, DataTable dataPasos, DataTable dataPasos4, DataTable dt5, DataTable dt6, out string nombreArchivo)
        {
            nombreArchivo = $"{DateTime.Now:yyyyMMddHHmmss}-{nombreReporte}.xlsx";
            FileInfo directorioPlantillas = new FileInfo(
                $"{HttpContext.Current.Server.MapPath("~/Content/Plantillas/")}\\Informe.xlsx");
            FileInfo directorioTemporal = new FileInfo(
                $"{HttpContext.Current.Server.MapPath("~/Temp/")}\\{nombreArchivo}");

            using (ExcelPackage excelPackage = new ExcelPackage(directorioTemporal, directorioPlantillas))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];

                worksheet.Name = nombreReporte;
                worksheet.Cells[3, 10].Value = dataPasos.Rows[0]["CodigoProyecto"];
                //Codigo Proyecto
                worksheet.Cells[4, 2].Value = dataPasos.Rows[0]["NombreProyecto"];
                worksheet.Cells[5, 3].Value = dataPasos.Rows[0]["EntidadResponsable"];
                worksheet.Cells[6, 3].Value = dataPasos.Rows[0]["Departamento"];
                worksheet.Cells[7, 3].Value = dataPasos.Rows[0]["Municipio"];
                worksheet.Cells[8, 3].Value = dataPasos.Rows[0]["Pilar"];
                worksheet.Cells[9, 3].Value = dataPasos.Rows[0]["EstadoProyecto"];
                worksheet.Cells[12, 3].Value = dataPasos.Rows[0]["ObjetoVeeduria"];
                worksheet.Cells[13, 3].Value = dataPasos.Rows[0]["Introduccion"];
                worksheet.Cells[14, 3].Value = dataPasos.Rows[0]["Metodologia"];
                worksheet.Cells[15, 3].Value = dataPasos.Rows[0]["ResultadosInf"];
                worksheet.Cells[16, 3].Value = dataPasos.Rows[0]["Recomendaciones"];

                worksheet.Cells[18, 3].Value = dataPasos.Rows[0]["Pilar"];
                worksheet.Cells[19, 3].Value = dataPasos.Rows[0]["EstadoProyecto"];
                worksheet.Cells[20, 3].Value = dataPasos.Rows[0]["ObjetoVeeduria"];
                worksheet.Cells[21, 3].Value = dataPasos.Rows[0]["Introduccion"];
                worksheet.Cells[22, 3].Value = dataPasos.Rows[0]["Metodologia"];
                worksheet.Cells[23, 3].Value = dataPasos.Rows[0]["ResultadosInf"];
                worksheet.Cells[24, 3].Value = dataPasos.Rows[0]["Recomendaciones"];

                worksheet.Cells[26, 3].Value = dataPasos.Rows[0]["Pilar"];
                worksheet.Cells[27, 3].Value = dataPasos.Rows[0]["EstadoProyecto"];
                worksheet.Cells[28, 3].Value = dataPasos.Rows[0]["ObjetoVeeduria"];
                worksheet.Cells[29, 3].Value = dataPasos.Rows[0]["Introduccion"];
                worksheet.Cells[30, 3].Value = dataPasos.Rows[0]["Metodologia"];

                int fila = 33;

                foreach (DataRow fuente in dataPasos4.Rows)
                {
                    worksheet.Cells[fila, 2].Value = fuente["PasoCronograma"];
                    worksheet.Cells[fila, 3].Value = fuente["QueHacer"];
                    worksheet.Cells[fila, 4].Value = fuente["Responsables"];
                    worksheet.Cells[fila, 6].Value = fuente["Recursos"];
                    worksheet.Cells[fila, 6].Value = fuente["Fecha"];
                    fila++;
                }

                fila = fila++;
                worksheet.Cells[fila, 2].Value = "PASO 5. Establecer criterios de Evaluación";
                fila = fila++;

                worksheet.Cells[fila, 3].Value = dataPasos.Rows[0]["Pilar"];
                worksheet.Cells[fila + 1, 3].Value = dataPasos.Rows[0]["EstadoProyecto"];
                worksheet.Cells[fila + 2, 3].Value = dataPasos.Rows[0]["ObjetoVeeduria"];
                worksheet.Cells[fila + 3, 3].Value = dataPasos.Rows[0]["Introduccion"];
                worksheet.Cells[fila + 4, 3].Value = dataPasos.Rows[0]["Metodologia"];

                fila = fila + 6;

                worksheet.Cells[fila, 2].Value = "Objetivo";
                worksheet.Cells[fila, 3].Value = "Evaluación cualitativa";
                worksheet.Cells[fila, 4].Value = "Cumple";
                fila = fila++;
                foreach (DataRow dataRow in dt5.Rows)
                {
                    int columna = 2;
                    for (int i = 2; i < 5; i++)
                    {
                        worksheet.Cells[fila, columna].Value = dataRow[i];
                        columna++;
                    }
                    fila++;
                }

                var stream = new MemoryStream();
                excelPackage.SaveAs(stream);

                return stream;
            }
        }

    }
}
